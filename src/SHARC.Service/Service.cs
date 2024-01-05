using SHARC.Mqtt;
using SHARC.TrakHound;
using TrakHound;
using TrakHound.Clients;
using TrakHound.Entities;
using TrakHound.Services;

namespace SHARC
{
    public class Service : TrakHoundService
    {
        private readonly SharcMqttClient _sharcClient;
        private bool _availability;


        public Service(ITrakHoundServiceConfiguration configuration, ITrakHoundHostClient client) : base(configuration, client)
        {
            _sharcClient = new SharcMqttClient("mosquitto.spb.mtcup.org", 1884, "409151d72b34");
            _sharcClient.AvailabilityReceived += AvailabilityReceivedReceived;
            _sharcClient.DeviceInformationReceived += DeviceInformationReceived;
            _sharcClient.MqttInformationReceived += MqttInformationReceived;
            _sharcClient.NetworkInterfaceReceived += NetworkInterfaceReceived;
            _sharcClient.BootCounterReceived += BootCounterReceived;
            _sharcClient.SensorConfigurationReceived += SensorConfigurationReceived;
            _sharcClient.AggregateCalibratedConvertedSensorValueReceived += SharcClientAggregateCalibratedConvertedSensorValueReceived;
        }


        protected override void OnStart()
        {
            _sharcClient.Start();
            _ = Task.Run(PublishDefinitions);
        }

        protected override void OnStop()
        {
            _sharcClient.Stop();
        }


        private async Task PublishDefinitions()
        {
            await Client.Entities.Definitions.Publish(TrakHoundSharcDefinitions.GetAll());
        }


        private async void AvailabilityReceivedReceived(string sharcId, SharcMqttEvent<bool> e)
        {
            _availability = e.Value;

            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;

            await Client.Publish(sharcModel, true);
        }

        private async void DeviceInformationReceived(string sharcId, SharcMqttEvent<SharcDeviceInformation> e)
        {
            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;
            sharcModel.ver = new TrakHoundSharcDeviceInformationModel(e.Value);

            await Client.Publish(sharcModel, true);
        }

        private async void MqttInformationReceived(string sharcId, SharcMqttEvent<SharcMqttInformation> e)
        {
            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;
            sharcModel.mqtt = new TrakHoundSharcMqttInformationModel(e.Value);

            await Client.Publish(sharcModel, true);
        }

        private async void NetworkInterfaceReceived(string sharcId, SharcMqttEvent<SharcNetworkInterface> e)
        {
            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;
            sharcModel.net = new TrakHoundSharcNetworkInterfaceModel(e.Value);

            await Client.Publish(sharcModel, true);
        }

        private async void BootCounterReceived(string sharcId, SharcMqttEvent<SharcBootCounter> e)
        {
            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;
            sharcModel.rc = new TrakHoundSharcBootCounterModel(e.Value);

            await Client.Publish(sharcModel, true);
        }

        private async void SensorConfigurationReceived(string sharcId, SharcMqttEvent<SharcSensorConfiguration> e)
        {
            var sharcModel = new TrakHoundSharcModel();
            sharcModel.SharcId = sharcId;
            sharcModel.Availability = _availability;
            sharcModel.sensor = new TrakHoundSharcSensorConfigurationModel(e.Value);

            await Client.Publish(sharcModel, true);
        }

        private async void SharcClientAggregateCalibratedConvertedSensorValueReceived(string sharcId, SharcAggregateCalibratedConvertedSensorValue data)
        {
            var valueObject = await Client.PublishObject($"sharc/{sharcId}/io", TrakHoundObjectContentType.Directory);
            if (valueObject != null)
            {
                var entities = new List<ITrakHoundEntity>();

                if (data.Value != null)
                {
                    foreach (var value in data.Value)
                    {
                        if (value.Value != null)
                        {
                            var sensorName = value.Key;
                            var sensorObjectId = $"{valueObject.Uuid}:{sensorName}".ToMD5Hash();
                            var sensorObject = new TrakHoundObjectEntity(sensorName, TrakHoundObjectContentType.Observation, id: sensorObjectId, parentUuid: valueObject.Uuid);
                            entities.Add(sensorObject);
                            entities.Add(new TrakHoundObjectObservationEntity(sensorObject.Uuid, value.Value.Value, dataType: (int)TrakHoundObjectObservationDataType.Float));
                        }
                    }
                }

                await Client.Entities.Publish(entities);
            }
        }
    }
}
