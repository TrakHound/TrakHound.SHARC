using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcDistinctSensorEvent : SharcMqttEvent<SharcDistinctSensorValue>
    {
        [JsonIgnore]
        public string SensorName { get; set; }
    }

    public class SharcDistinctSensorValue
    {
        [JsonPropertyName("u")]
        public string Units { get; set; }

        [JsonPropertyName("v")]
        public double Value { get; set; }
    }
}
