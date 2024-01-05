using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcMqttEvent<TValue>
    {
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }

        [JsonPropertyName("v")]
        public TValue Value { get; set; }
    }
}
