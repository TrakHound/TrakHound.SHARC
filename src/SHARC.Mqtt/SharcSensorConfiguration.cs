using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcSensorConfiguration
    {
        [JsonPropertyName("aggregate")]
        public bool Aggregate { get; set; }

        [JsonPropertyName("calibrate")]
        public bool Calibrate { get; set; }

        [JsonPropertyName("convert")]
        public bool Convert { get; set; }

        [JsonPropertyName("s0")]
        public SharcSensorValueConfiguration S0 { get; set; }

        [JsonPropertyName("s1")]
        public SharcSensorValueConfiguration S1 { get; set; }

        [JsonPropertyName("s2")]
        public SharcSensorValueConfiguration S2 { get; set; }

        [JsonPropertyName("s3")]
        public SharcSensorValueConfiguration S3 { get; set; }
    }
}
