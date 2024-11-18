using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System.Security.Cryptography.X509Certificates;

namespace SHARC.Mqtt
{
    public class SharcMqttDiscoveryClient
    {
        private const string _defaultTopicPrefix = "sharc";


        private readonly MqttFactory _mqttFactory;
        private readonly SharcMqttDiscoveryClientConfiguration _configuration;

        private int timeout;


        /// <summary>
        /// Gets the Client Configuration
        /// </summary>
        public SharcMqttDiscoveryClientConfiguration Configuration => _configuration;

        /// <summary>
        /// Raised when the connection to the MQTT broker is established
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> Connected;

        /// <summary>
        /// Raised when the connection to the MQTT broker is disconnected 
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> Disconnected;

        /// <summary>
        /// Raised when the status of the connection to the MQTT broker has changed
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient, SharcMqttConnectionStatus> ConnectionStatusChanged;

        /// <summary>
        /// Raised when an error occurs during connection to the MQTT broker
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient, Exception> ConnectionError;

        /// <summary>
        /// Raised when an Internal Error occurs
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient, Exception> InternalError;

        /// <summary>
        /// Raised when the Client is Starting
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> ClientStarting;

        /// <summary>
        /// Raised when the Client is Started
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> ClientStarted;

        /// <summary>
        /// Raised when the Client is Stopping
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> ClientStopping;

        /// <summary>
        /// Raised when the Client is Stopeed
        /// </summary>
        public event SharcMqttEventHandler<SharcMqttDiscoveryClient> ClientStopped;


        public SharcMqttDiscoveryClient(string server, int port = 1883)
        {
            var configuration = new SharcMqttDiscoveryClientConfiguration();
            configuration.Server = server;
            configuration.Port = port;
            _configuration = configuration;

            _mqttFactory = new MqttFactory();
        }

        public SharcMqttDiscoveryClient(SharcMqttDiscoveryClientConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration == null) _configuration = new SharcMqttDiscoveryClientConfiguration();

            _mqttFactory = new MqttFactory();
        }


        public async Task<IEnumerable<SharcDiscoveryModel>> Run(int timeout = 5000)
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

                var models = new Dictionary<string, SharcDiscoveryModel>();

                using (var mqttClient = _mqttFactory.CreateMqttClient())
                {
                    mqttClient.ApplicationMessageReceivedAsync += (o) => ProcessMessage(models, o);

                    // Connect to the MQTT Client
                    await mqttClient.ConnectAsync(clientOptions);

                    Connected?.Invoke(this);

                    // Subscribe to MQTT Topics
                    await Subscribe(mqttClient);

                    ClientStarted?.Invoke(this);

                    await Task.Delay(timeout);
                }

                return models.Values;
            }
            catch (Exception ex)
            {
                if (ConnectionError != null) ConnectionError.Invoke(this, ex);
            }

            ClientStopped?.Invoke(this);

            return null;
        }


        private async Task Subscribe(IMqttClient mqttClient)
        {
            var topicFilters = new List<MqttTopicFilter>();

            // Availability
            var availabilityTopic = new MqttTopicFilter();
            availabilityTopic.Topic = $"{_defaultTopicPrefix}/+/evt/avail";
            availabilityTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(availabilityTopic);

            // Boot Counter
            var bootCounterTopic = new MqttTopicFilter();
            bootCounterTopic.Topic = $"{_defaultTopicPrefix}/+/evt/rc";
            bootCounterTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(bootCounterTopic);

            // Network Interface
            var networkInterrfaceTopic = new MqttTopicFilter();
            networkInterrfaceTopic.Topic = $"{_defaultTopicPrefix}/+/evt/net";
            networkInterrfaceTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(networkInterrfaceTopic);

            // Device Information
            var deviceInformationTopic = new MqttTopicFilter();
            deviceInformationTopic.Topic = $"{_defaultTopicPrefix}/+/evt/ver";
            deviceInformationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(deviceInformationTopic);

            // MQTT Information
            var mqttInformationTopic = new MqttTopicFilter();
            mqttInformationTopic.Topic = $"{_defaultTopicPrefix}/+/evt/mqtt";
            mqttInformationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(mqttInformationTopic);

            // Sensor Configuration
            var sensorConfigurationTopic = new MqttTopicFilter();
            sensorConfigurationTopic.Topic = $"{_defaultTopicPrefix}/+/evt/sensor";
            sensorConfigurationTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(sensorConfigurationTopic);

            // User Data
            var userDataTopic = new MqttTopicFilter();
            userDataTopic.Topic = $"{_defaultTopicPrefix}/+/evt/user";
            userDataTopic.QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)_configuration.QoS;
            topicFilters.Add(userDataTopic);

            var subscribeOptions = new MqttClientSubscribeOptions();
            subscribeOptions.TopicFilters = topicFilters;

            await mqttClient.SubscribeAsync(subscribeOptions);
        }

        private Task ProcessMessage(Dictionary<string, SharcDiscoveryModel> models, MqttApplicationMessageReceivedEventArgs args)
        {
            if (args != null && args.ApplicationMessage != null && args.ApplicationMessage.Topic != null)
            {
                try
                {
                    var json = System.Text.Encoding.UTF8.GetString(args.ApplicationMessage.PayloadSegment);

                    var topic = args.ApplicationMessage.Topic;

                    var sharcId = GetSharcId(topic);
                    if (sharcId != null)
                    {
                        var model = models.GetValueOrDefault(sharcId);
                        if (model == null)
                        {
                            model = new SharcDiscoveryModel();
                            model.SharcId = sharcId;
                            models.Add(sharcId, model);
                        }

                        var eventTopicSuffix = GetTopicPart(topic, "evt");
                        switch (eventTopicSuffix)
                        {
                            // Availability
                            case "avail":
                                var availability = Json.Convert<SharcMqttEvent<bool>>(json);
                                if (availability != null) model.Availability = availability.Value;
                                break;

                            // Boot Counter
                            case "rc":
                                var bootCounter = Json.Convert<SharcMqttEvent<SharcBootCounter>>(json);
                                if (bootCounter != null) model.Boot = bootCounter.Value;
                                break;

                            // Network Interface
                            case "net":
                                var networkInterface = Json.Convert<SharcMqttEvent<SharcNetworkInterface>>(json);
                                if (networkInterface != null) model.Network = networkInterface.Value;
                                break;

                            // Device Information
                            case "ver":
                                var deviceInformation = Json.Convert<SharcMqttEvent<SharcDeviceInformation>>(json);
                                if (deviceInformation != null) model.Device = deviceInformation.Value;
                                break;

                            // MQTT Information
                            case "mqtt":
                                var mqttInformation = Json.Convert<SharcMqttEvent<SharcMqttInformation>>(json);
                                if (mqttInformation != null) model.Mqtt = mqttInformation.Value;
                                break;

                            // Sensor Configuration
                            case "sensor":
                                var sensorConfiguration = Json.Convert<SharcMqttEvent<SharcSensorConfiguration>>(json);
                                if (sensorConfiguration != null) model.Sensor = sensorConfiguration.Value;
                                break;

                            //// User Data
                            //case "user":
                            //    var userData = Json.Convert<SharcMqttEvent<SharcUserData>>(json);
                            //    if (userData != null) model.User = userData.Value;
                            //    break;
                        }
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
