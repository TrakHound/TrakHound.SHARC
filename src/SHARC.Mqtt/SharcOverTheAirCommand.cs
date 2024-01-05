using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcOverTheAirCommand
    {
        [JsonPropertyName("bin")]
        public string Bin { get; set; }
    }
}
