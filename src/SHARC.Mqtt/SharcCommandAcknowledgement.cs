using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcCommandAcknowledgement
    {
        /// <summary>
        /// Correlation to command
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Command execution result code
        /// </summary>
        [JsonPropertyName("rc")]
        public int ResultCode { get; set; }
    }
}
