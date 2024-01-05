using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcUserData
    {
        /// <summary>
        /// User data
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }
    }
}
