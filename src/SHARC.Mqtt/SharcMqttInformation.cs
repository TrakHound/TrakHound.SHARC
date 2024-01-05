using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcMqttInformation
    {
        /// <summary>
        /// Broker address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; }

        /// <summary>
        /// Broker port
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [JsonPropertyName("pass")]
        public string Password { get; set; }

        /// <summary>
        /// Is connection anonymous
        /// </summary>
        [JsonPropertyName("anonymous")]
        public bool Anonymous { get; set; }
    }
}
