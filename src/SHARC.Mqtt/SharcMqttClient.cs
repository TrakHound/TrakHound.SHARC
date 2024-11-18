using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System.Security.Cryptography.X509Certificates;

namespace SHARC.Mqtt
{
    public class SharcMqttClient
    {
        private const string _defaultTopicPrefix = "sharc";


        private readonly MqttFactory _mqttFactory;
        private readonly IMqttClient _mqttClient;
        private readonly SharcMqttClientConfiguration _configuration;
        private readonly string _sharcId;

        private CancellationTokenSource _stop;
        private SharcMqttConnectionStatus _connectionStatus;
        private long _lastResponse;


        /// <summary>
        /// Gets the SHARC ID
        /// </summary>
        public string SharcId => _sharcId;

        /// <summary>
        /// Gets the Client Configuration
        /// </summary>
        public SharcMqttClientConfiguration Configuration => _configuration;

        /// <summary>
        /// Gets the Unix Timestamp (in Milliseconds) since the last response from the MTConnect Agent
        /// </summary>
        public long LastResponse => _lastResponse;

        /// <summary>
        /// Gets the status of the connection to the MQTT broker
        /// </summary>
        public SharcMqttConnectionStatus ConnectionStatus => _connectionStatus;

        /// <summary>
        /// Raised when the connection to the MQTT broker is established
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> Connected;

        /// <summary>
        /// Raised when the connection to the MQTT broker is disconnected 
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> Disconnected;

        /// <summary>
        /// Raised when the status of the connection to the MQTT broker has changed
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttConnectionStatus> ConnectionStatusChanged;

        /// <summary>
        /// Raised when an error occurs during connection to the MQTT broker
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, Exception> ConnectionError;

        /// <summary>
        /// Raised when an Internal Error occurs
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, Exception> InternalError;

        /// <summary>
        /// Raised when a Device is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<bool>> AvailabilityReceived;

        /// <summary>
        /// Raised when an Observation is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcBootCounter>> BootCounterReceived;

        /// <summary>
        /// Raised when an Asset is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcNetworkInterface>> NetworkInterfaceReceived;

        /// <summary>
        /// Raised when an MTConnectDevices Document is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcDeviceInformation>> DeviceInformationReceived;

        /// <summary>
        /// Raised when an MTConnectSreams Document is received from a Current Request
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcMqttInformation>> MqttInformationReceived;

        /// <summary>
        /// Raised when an MTConnectSreams Document is received from the Samples Stream
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcSensorConfiguration>> SensorConfigurationReceived;

        /// <summary>
        /// Raised when an MTConnectAssets Document is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcAggregateSensorValue> AggregateSensorValueReceived;

        /// <summary>
        /// Raised when an MTConnectAssets Document is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcDistinctSensorEvent> DistinctSensorValueReceived;

        /// <summary>
        /// Raised when an MTConnectAssets Document is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcAggregateCalibratedConvertedSensorValue> AggregateCalibratedConvertedSensorValueReceived;

        /// <summary>
        /// Raised when an MTConnectDevices Document is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient, SharcMqttEvent<SharcUserData>> UserDataReceived;

        /// <summary>
        /// Raised when any Response from the Client is received
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> ResponseReceived;

        /// <summary>
        /// Raised when the Client is Starting
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> ClientStarting;

        /// <summary>
        /// Raised when the Client is Started
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> ClientStarted;

        /// <summary>
        /// Raised when the Client is Stopping
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> ClientStopping;

        /// <summary>
        /// Raised when the Client is Stopeed
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttClient> ClientStopped;


        public SharcMqttClient(string sharcId, string server, int port = 1883)
        {
            _sharcId = sharcId;

            var configuration = new SharcMqttClientConfiguration();
            configuration.SharcId = sharcId;
            configuration.Server = server;
            configuration.Port = port;
            _configuration = configuration;

            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClient.ApplicationMessageReceivedAsync += ProcessMessage;
        }

        public SharcMqttClient(SharcMqttClientConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration == null) _configuration = new SharcMqttClientConfiguration();

            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttClient.ApplicationMessageReceivedAsync += ProcessMessage;
        }


        public void Start()
        {
            _stop = new CancellationTokenSource();

            ClientStarting?.Invoke(this);

            _ = Task.Run(Worker, _stop.Token);
        }

        public void Stop()
        {
            ClientStopping?.Invoke(this);

            if (_stop != null) _stop.Cancel();
        }

        public void Dispose()
        {
            if (_mqttClient != null) _mqttClient.Dispose();
        }


        //private Task ClientConnectedAsync(MqttClientConnectedEventArgs arg)
        //{
        //    Connected?.Invoke(this, arg);
        //    return Task.CompletedTask;
        //}

        //private Task ClientDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        //{
        //    Disconnected?.Invoke(this, arg);
        //    return Task.CompletedTask;
        //}


        private async Task Worker()
        {
            do
            {
                try
                {
                    try
                    {
                        // Declare new MQTT Client Options with Tcp Server
                        var clientOptionsBuilder = new MqttClientOptionsBuilder().WithTcpServer(_configuration.Server, _configuration.Port);

                        clientOptionsBuilder.WithCleanSession(false);

                        // Set Client ID
                        if (!string.IsNullOrEmpty(_configuration.ClientId))
                        {
                            clientOptionsBuilder.WithClientId(_configuration.ClientId);
                        }

                        var certificates = new List<X509Certificate2>();

                        // Add CA (Certificate Authority)
                        if (!string.IsNullOrEmpty(_configuration.CertificateAuthority))
                        {
                            certificates.Add(new X509Certificate2(GetFilePath(_configuration.CertificateAuthority)));
                        }

                        // Add Client Certificate & Private Key
                        if (!string.IsNullOrEmpty(_configuration.PemCertificate) && !string.IsNullOrEmpty(_configuration.PemPrivateKey))
                        {

#if NET5_0_OR_GREATER
                            certificates.Add(new X509Certificate2(X509Certificate2.CreateFromPemFile(GetFilePath(_configuration.PemCertificate), GetFilePath(_configuration.PemPrivateKey)).Export(X509ContentType.Pfx)));
#else
                    throw new Exception("PEM Certificates Not Supported in .NET Framework 4.8 or older");
#endif

                            clientOptionsBuilder.WithTls(new MqttClientOptionsBuilderTlsParameters()
                            {
                                UseTls = true,
                                SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                                IgnoreCertificateRevocationErrors = _configuration.AllowUntrustedCertificates,
                                IgnoreCertificateChainErrors = _configuration.AllowUntrustedCertificates,
                                AllowUntrustedCertificates = _configuration.AllowUntrustedCertificates,
                                Certificates = certificates
                            });
                        }

                        // Add Credentials
                        if (!string.IsNullOrEmpty(_configuration.Username) && !string.IsNullOrEmpty(_configuration.Password))
                        {
                            if (_configuration.UseTls)
                            {
                                clientOptionsBuilder.WithCredentials(_configuration.Username, _configuration.Password).WithTls();
                            }
                            else
                            {
                                clientOptionsBuilder.WithCredentials(_configuration.Username, _configuration.Password);
                            }
                        }

                        // Build MQTT Client Options
                        var clientOptions = clientOptionsBuilder.Build();

                        // Connect to the MQTT Client
                        await _mqttClient.ConnectAsync(clientOptions);

                        Connected?.Invoke(this);

                        // Subscribe to MQTT Topics
                        await Subscribe();

                        ClientStarted?.Invoke(this);

                        while (_mqttClient.IsConnected && !_stop.IsCancellationRequested)
                        {
                            await Task.Delay(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ConnectionError != null) ConnectionError.Invoke(this, ex);
                    }

                    Disconnected?.Invoke(this);

                    await Task.Delay(_configuration.RetryInterval, _stop.Token);
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    InternalError?.Invoke(this, ex);
                }

            } while (!_stop.Token.IsCancellationRequested);


            try
            {
                // Disconnect from the MQTT Client
                if (_mqttClient != null) await _mqttClient.DisconnectAsync(MqttClientDisconnectOptionsReason.NormalDisconnection);
            }
            catch { }


            ClientStopped?.Invoke(this);
        }


        private async Task Subscribe()
        {
            var topicFilters = new List<MqttTopicFilter>();

            // Availability
            var availabilityTopic = new MqttTopicFilter();
            availabilityTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/avail";
            availabilityTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(availabilityTopic);

            // Boot Counter
            var bootCounterTopic = new MqttTopicFilter();
            bootCounterTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/rc";
            bootCounterTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(bootCounterTopic);

            // Network Interface
            var networkInterrfaceTopic = new MqttTopicFilter();
            networkInterrfaceTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/net";
            networkInterrfaceTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(networkInterrfaceTopic);

            // Device Information
            var deviceInformationTopic = new MqttTopicFilter();
            deviceInformationTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/ver";
            deviceInformationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(deviceInformationTopic);

            // MQTT Information
            var mqttInformationTopic = new MqttTopicFilter();
            mqttInformationTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/mqtt";
            mqttInformationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(mqttInformationTopic);

            // Sensor Configuration
            var sensorConfigurationTopic = new MqttTopicFilter();
            sensorConfigurationTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/sensor";
            sensorConfigurationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(sensorConfigurationTopic);

            // Sensor Value
            var sensorValueTopic = new MqttTopicFilter();
            sensorValueTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/io/#";
            sensorValueTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(sensorValueTopic);

            // User Data
            var userDataTopic = new MqttTopicFilter();
            userDataTopic.Topic = $"{_defaultTopicPrefix}/{_sharcId}/evt/user";
            userDataTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(userDataTopic);

            var subscribeOptions = new MqttClientSubscribeOptions();
            subscribeOptions.TopicFilters = topicFilters;

            await _mqttClient.SubscribeAsync(subscribeOptions, _stop.Token);
        }

        private Task ProcessMessage(MqttApplicationMessageReceivedEventArgs args)
        {
            if (args != null && args.ApplicationMessage != null && args.ApplicationMessage.Topic != null)
            {
                try
                {
                    var json = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment);

                    var topic = args.ApplicationMessage.Topic;

                    var sharcId = GetSharcId(topic);

                    var eventTopicSuffix = GetTopicPart(topic, "evt");
                    switch (eventTopicSuffix)
                    {
                        // Availability
                        case "avail":
                            var availability = Json.Convert<SharcMqttEvent<bool>>(json);
                            if (AvailabilityReceived != null && availability != null) AvailabilityReceived.Invoke(this, availability);
                            break;

                        // Boot Counter
                        case "rc":
                            var bootCounter = Json.Convert<SharcMqttEvent<SharcBootCounter>>(json);
                            if (BootCounterReceived != null && bootCounter != null) BootCounterReceived.Invoke(this, bootCounter);
                            break;

                        // Network Interface
                        case "net":
                            var networkInterface = Json.Convert<SharcMqttEvent<SharcNetworkInterface>>(json);
                            if (NetworkInterfaceReceived != null && networkInterface != null) NetworkInterfaceReceived.Invoke(this, networkInterface);
                            break;

                        // Device Information
                        case "ver":
                            var deviceInformation = Json.Convert<SharcMqttEvent<SharcDeviceInformation>>(json);
                            if (DeviceInformationReceived != null && deviceInformation != null) DeviceInformationReceived.Invoke(this, deviceInformation);
                            break;

                        // MQTT Information
                        case "mqtt":
                            var mqttInformation = Json.Convert<SharcMqttEvent<SharcMqttInformation>>(json);
                            if (MqttInformationReceived != null && mqttInformation != null) MqttInformationReceived.Invoke(this, mqttInformation);
                            break;

                        // Sensor Configuration
                        case "sensor":
                            var sensorConfiguration = Json.Convert<SharcMqttEvent<SharcSensorConfiguration>>(json);
                            if (SensorConfigurationReceived != null && sensorConfiguration != null) SensorConfigurationReceived.Invoke(this, sensorConfiguration);
                            break;

                        // Sensor Value
                        case "io":

                            var sensorName = GetTopicPart(topic, "io");
                            if (sensorName != null)
                            {
                                var distinctSensorValue = Json.Convert<SharcDistinctSensorEvent>(json);
                                if (distinctSensorValue != null)
                                {
                                    distinctSensorValue.SensorName = sensorName;
                                    if (DistinctSensorValueReceived != null) DistinctSensorValueReceived.Invoke(this, distinctSensorValue);
                                }
                            }
                            else
                            {
                                var aggregateSensorValue = Json.Convert<SharcAggregateSensorValue>(json);
                                if (aggregateSensorValue != null)
                                {
                                    if (AggregateSensorValueReceived != null && aggregateSensorValue != null) AggregateSensorValueReceived.Invoke(this, aggregateSensorValue);
                                }
                                else
                                {
                                    var aggregateCCSensorValue = Json.Convert<SharcAggregateCalibratedConvertedSensorValue>(json);
                                    if (aggregateCCSensorValue != null)
                                    {
                                        if (AggregateCalibratedConvertedSensorValueReceived != null && aggregateCCSensorValue != null) AggregateCalibratedConvertedSensorValueReceived.Invoke(this, aggregateCCSensorValue);
                                    }
                                }
                            }
                            break;

                        // User Data
                        case "user":
                            var userData = Json.Convert<SharcMqttEvent<SharcUserData>>(json);
                            if (UserDataReceived != null && userData != null) UserDataReceived.Invoke(this, userData);
                            break;
                    }
                }
                catch { }
            }

            return Task.CompletedTask;
        }


        private static string GetSharcId(string topic)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                var s = topic.IndexOf('/');
                s++;
                var e = topic.IndexOf('/', s);

                if (e > 0 && e < topic.Length - 1)
                {
                    return topic.Substring(s, e - s);
                }
            }

            return null;
        }

        private static string GetTopicPart(string topic, string part)
        {
            if (!string.IsNullOrEmpty(topic) && !string.IsNullOrEmpty(part))
            {
                var x = 0;
                var s = 0;
                var e = 0;
                string p;

                do
                {
                    s = topic.IndexOf('/', s);
                    s++;
                    e = topic.IndexOf('/', s);

                    if (e < 0 || e > topic.Length - 1) break;

                    p = topic.Substring(s, e - s);
                    if (p == part) break;
                    x++;
                }
                while (e < topic.Length - 1);

                if (e > 0 && e < topic.Length - 1) 
                {
                    s = topic.IndexOf('/', e + 1);

                    if (s > 0) return topic.Substring(e + 1, s - e - 1);
                    else return topic.Substring(e + 1);
                }
            }

            return null;
        }

        private static string GetFilePath(string path)
        {
            var x = path;
            if (!Path.IsPathRooted(x))
            {
                x = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, x);
            }

            return x;
        }
    }
}
