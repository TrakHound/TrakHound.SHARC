using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcMqttCommand<TValue>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("v")]
        public TValue Value { get; set; }
    }
}
