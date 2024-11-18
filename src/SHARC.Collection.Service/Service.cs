using SHARC.Mqtt;
using TrakHound;
using TrakHound.Clients;
using TrakHound.Entities;
using TrakHound.Requests;
using TrakHound.Services;
using TrakHound.Volumes;

namespace SHARC.Collection
{
    public class Service : TrakHoundService
    {
        private const string _mqttLogName = "MQTT";
        private const string _sharcLogName = "SHARC";


        private readonly Dictionary<string, SensorConfiguration> _sensorConfigurations = new Dictionary<string, SensorConfiguration>(); // SharcId => SensorConfiguration
        private readonly Dictionary<string, SharcMqttClient> _sensorClients = new Dictionary<string, SharcMqttClient>(); // SharcId => SharcMqttClient
        private readonly Dictionary<string, bool> _sensorAvailability = new Dictionary<string, bool>(); // SharcId => Availability

        private ITrakHoundVolumeListener _configurationListener;
        private bool _started;


        public Service(ITrakHoundServiceConfiguration configuration, ITrakHoundClient client, ITrakHoundVolume volume) : base(configuration, client, volume) { }


        protected async override Task OnStartAsync()
        {
            _ = Task.Run(PublishDefinitions);

            _sensorConfigurations.Clear();
            _sensorClients.Clear();
            _sensorAvailability.Clear();

            var configurationFiles = await Volume.ListFiles();
            if (!configurationFiles.IsNullOrEmpty())
            {
                foreach (var configurationFile in configurationFiles)
                {
                    Log(TrakHoundLogLevel.Information, $"Configuration File Found : {configurationFile}");

                    await LoadConfiguration(configurationFile, true);
                }
            }

            _configurationListener = Volume.CreateListener("/");
            _configurationListener.Changed += ConfigurationChanged;
            _configurationListener.Start();

            _started = true;
        }

        protected override void OnStop()
        {
            _started = false;

            if (_configurationListener != null) _configurationListener.Dispose();

            foreach (var sharcClient in _sensorClients)
            {
                sharcClient.Value.Stop();
            }

            _sensorConfigurations.Clear();
            _sensorClients.Clear();
            _sensorAvailability.Clear();
        }

        private async void ConfigurationChanged(string path, TrakHoundVolumeOnChangeType changeType)
        {
            Log(TrakHoundLogLevel.Information, $"Configuration File Changed : Sensor : {path} => {changeType}");
            await LoadConfiguration(path, _started);
        }

        private async Task LoadConfiguration(string path, bool start)
        {
            var configuration = await Volume.ReadJson<SensorConfiguration>(path);
            if (configuration != null && !string.IsNullOrEmpty(configuration.Id) && !string.IsNullOrEmpty(configuration.Server))
            {
                Log(TrakHoundLogLevel.Information, $"Configuration File Read Successfully : {configuration.Id}");

                var existingClient = _sensorClients.GetValueOrDefault(configuration.Id);
                if (existingClient != null) existingClient.Stop();

                _sensorClients.Remove(configuration.Id);
                _sensorConfigurations.Remove(configuration.Id);
                _sensorAvailability.Remove(configuration.Id);

                if (configuration.Enabled)
                {
                    Log(TrakHoundLogLevel.Information, $"Starting MQTT Client : {configuration.Id}");

                    var sharcClient = new SharcMqttClient(configuration.Id, configuration.Server, configuration.Port);
                    sharcClient.Connected += SharcClientConnected;
                    sharcClient.Disconnected += SharcClientDisconnected;
                    sharcClient.ConnectionError += SharcClientConnectionError;
                    sharcClient.AvailabilityReceived += AvailabilityReceivedReceived;
                    sharcClient.DeviceInformationReceived += DeviceInformationReceived;
                    sharcClient.MqttInformationReceived += MqttInformationReceived;
                    sharcClient.NetworkInterfaceReceived += NetworkInterfaceReceived;
                    sharcClient.BootCounterReceived += BootCounterReceived;
                    sharcClient.SensorConfigurationReceived += SensorConfigurationReceived;
                    sharcClient.DistinctSensorValueReceived += DistinctValueReceived;
                    sharcClient.AggregateCalibratedConvertedSensorValueReceived += SharcClientAggregateCalibratedConvertedSensorValueReceived;

                    _sensorConfigurations.Add(configuration.Id, configuration);
                    _sensorClients.Add(configuration.Id, sharcClient);

                    if (start) sharcClient.Start();
                }
                else
                {
                    Log(TrakHoundLogLevel.Information, $"Configuration Not Enabled : {configuration.Id}");

                    // Update Enabled
                    await Client.Entities.PublishBoolean(TrakHoundPath.Combine(GetBasePath(configuration), "Enabled"), false);
                }
            }
        }


        private async Task PublishDefinitions()
        {
            var transaction = new TrakHoundEntityTransaction();

            foreach (var entry in TrakHoundSharcDefinitions.GetAll()) transaction.Add(entry);

            await Client.Entities.Publish(transaction);
        }


        private void SharcClientConnected(SharcMqttClient client)
        {
            Log(_mqttLogName, TrakHoundLogLevel.Information, $"MQTT Client Connected to {client.Configuration.Server}:{client.Configuration.Port} :: SHARC ID = {client.Configuration.SharcId}");
        }

        private void SharcClientDisconnected(SharcMqttClient client)
        {
            Log(_mqttLogName, TrakHoundLogLevel.Warning, $"MQTT Client Disconnected from {client.Configuration.Server}:{client.Configuration.Port} :: SHARC ID = {client.Configuration.SharcId}");
        }

        private void SharcClientConnectionError(SharcMqttClient client, Exception e)
        {
            Log(_mqttLogName, TrakHoundLogLevel.Error, $"MQTT Client Connection Error : {e.Message}", e.HResult.ToString());
        }

        private async void AvailabilityReceivedReceived(SharcMqttClient client, SharcMqttEvent<bool> e)
        {
            _sensorAvailability.Remove(client.SharcId);
            _sensorAvailability.Add(client.SharcId, e.Value);

            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = e.Value;
                sharcModel.Description = configuration.Description;

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC Availability Received : {e.Value} : Enabled = {configuration.Enabled}");

                await Client.Entities.Publish(sharcModel, false);
            }
        }

        private async void DeviceInformationReceived(SharcMqttClient client, SharcMqttEvent<SharcDeviceInformation> e)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = _sensorAvailability.GetValueOrDefault(client.SharcId);
                sharcModel.ver = new TrakHoundSharcDeviceInformationModel(e.Value);

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC Device Information Received : {e.Value.ToJson()}");

                await Client.Entities.Publish(sharcModel, true);
            }
        }

        private async void MqttInformationReceived(SharcMqttClient client, SharcMqttEvent<SharcMqttInformation> e)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = _sensorAvailability.GetValueOrDefault(client.SharcId);
                sharcModel.mqtt = new TrakHoundSharcMqttInformationModel(e.Value);

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC MQTT Information Received : {e.Value.ToJson()}");

                await Client.Entities.Publish(sharcModel, true);
            }
        }

        private async void NetworkInterfaceReceived(SharcMqttClient client, SharcMqttEvent<SharcNetworkInterface> e)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = _sensorAvailability.GetValueOrDefault(client.SharcId);
                sharcModel.net = new TrakHoundSharcNetworkInterfaceModel(e.Value);

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC Network Interface Received : {e.Value.ToJson()}");

                await Client.Entities.Publish(sharcModel, true);
            }
        }

        private async void BootCounterReceived(SharcMqttClient client, SharcMqttEvent<SharcBootCounter> e)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = _sensorAvailability.GetValueOrDefault(client.SharcId);
                sharcModel.rc = new TrakHoundSharcBootCounterModel(e.Value);

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC Boot Counter Received : {e.Value.ToJson()}");

                await Client.Entities.Publish(sharcModel, true);
            }
        }

        private async void SensorConfigurationReceived(SharcMqttClient client, SharcMqttEvent<SharcSensorConfiguration> e)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var sharcModel = new TrakHoundSharcModel();
                sharcModel.Path = GetBasePath(client.SharcId);
                sharcModel.SharcId = client.SharcId;
                sharcModel.Enabled = configuration.Enabled;
                sharcModel.Availability = _sensorAvailability.GetValueOrDefault(client.SharcId);
                sharcModel.sensor = new TrakHoundSharcSensorConfigurationModel(e.Value);

                Log(_sharcLogName, TrakHoundLogLevel.Debug, $"SHARC Sensor Received : {e.Value.ToJson()}");

                await Client.Entities.Publish(sharcModel, true);
            }
        }

        private async void DistinctValueReceived(SharcMqttClient client, SharcDistinctSensorEvent data)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var entities = new List<ITrakHoundEntity>();

                var basePath = GetBasePath(client.SharcId);

                var ioObject = new TrakHoundObjectEntity(TrakHoundPath.Combine(basePath, "io"), TrakHoundObjectContentType.Directory, sourceUuid: "debug");
                if (EntityFilter.Add(ioObject)) entities.Add(ioObject);

                if (data != null && data.Value != null)
                {
                    var sensorObjectPath = TrakHoundPath.Combine(ioObject.Path, data.SensorName);
                    var sensorObject = new TrakHoundObjectEntity(sensorObjectPath, TrakHoundObjectContentType.Observation, sourceUuid: "debug");
                    if (EntityFilter.Add(sensorObject)) entities.Add(sensorObject);

                    var observation = new TrakHoundObjectObservationEntity(sensorObject.Uuid, data.Value.Value, sourceUuid: "debug", dataType: (int)TrakHoundObservationDataType.Float);
                    if (EntityFilter.Add(observation)) entities.Add(observation);
                }

                await Client.System.Entities.Publish(entities);
            }
        }

        private async void SharcClientAggregateCalibratedConvertedSensorValueReceived(SharcMqttClient client, SharcAggregateCalibratedConvertedSensorValue data)
        {
            var configuration = _sensorConfigurations.GetValueOrDefault(client.SharcId);
            if (configuration != null)
            {
                var entities = new List<ITrakHoundEntity>();

                var basePath = GetBasePath(client.SharcId);

                var ioObject = new TrakHoundObjectEntity(TrakHoundPath.Combine(basePath, "io"), TrakHoundObjectContentType.Directory, sourceUuid: "debug");
                if (EntityFilter.Add(ioObject)) entities.Add(ioObject);

                if (data.Value != null)
                {
                    foreach (var value in data.Value)
                    {
                        if (value.Value != null)
                        {
                            var sensorName = value.Key;
                            var sensorObjectPath = TrakHoundPath.Combine(ioObject.Path, sensorName);
                            var sensorObject = new TrakHoundObjectEntity(sensorObjectPath, TrakHoundObjectContentType.Observation, sourceUuid: "debug");
                            if (EntityFilter.Add(sensorObject)) entities.Add(sensorObject);

                            var observation = new TrakHoundObjectObservationEntity(sensorObject.Uuid, value.Value.Value, sourceUuid: "debug", dataType: (int)TrakHoundObservationDataType.Float);
                            if (EntityFilter.Add(observation)) entities.Add(observation);
                        }
                    }
                }

                await Client.System.Entities.Publish(entities);
            }
        }


        private string GetBasePath(string sharcId)
        {
            if (!string.IsNullOrEmpty(sharcId))
            {
                var configuration = _sensorConfigurations.GetValueOrDefault(sharcId);
                return GetBasePath(configuration);
            }

            return null;
        }

        private string GetBasePath(SensorConfiguration configuration)
        {
            if (configuration != null)
            {
                var basePath = "/sharc";
                if (!string.IsNullOrEmpty(configuration.BasePath)) basePath = TrakHoundPath.ToRoot(configuration.BasePath);

                var name = configuration.Id;
                if (!string.IsNullOrEmpty(configuration.Name)) name = configuration.Name;

                return TrakHoundPath.Combine(basePath, name);
            }

            return null;
        }
    }
}
