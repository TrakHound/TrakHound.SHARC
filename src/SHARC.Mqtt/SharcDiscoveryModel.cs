using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcDiscoveryModel
    {
        [JsonPropertyName("sharcId")]
        public string SharcId { get; set; }

        [JsonPropertyName("availability")]
        public bool Availability { get; set; }

        [JsonPropertyName("mqtt")]
        public SharcMqttInformation Mqtt { get; set; }

        [JsonPropertyName("ver")]
        public SharcDeviceInformation Device { get; set; }

        [JsonPropertyName("net")]
        public SharcNetworkInterface Network { get; set; }

        [JsonPropertyName("rc")]
        public SharcBootCounter Boot { get; set; }

        [JsonPropertyName("sensor")]
        public SharcSensorConfiguration Sensor { get; set; }
    }
}
