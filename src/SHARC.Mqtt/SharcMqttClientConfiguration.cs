using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SHARC.Mqtt
{
    public class SharcMqttClientConfiguration
    {
        [JsonPropertyName("server")]
        public string Server { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("interval")]
        public int Interval { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("qos")]
        public int QoS { get; set; }

        [JsonPropertyName("certificateAuthority")]
        public string CertificateAuthority { get; set; }

        [JsonPropertyName("pemCertificate")]
        public string PemCertificate { get; set; }

        [JsonPropertyName("pemPrivateKey")]
        public string PemPrivateKey { get; set; }

        [JsonPropertyName("allowUntrustedCertificates")]
        public bool AllowUntrustedCertificates { get; set; }

        [JsonPropertyName("useTls")]
        public bool UseTls { get; set; }

        [JsonPropertyName("retryInterval")]
        public int RetryInterval { get; set; }

        [JsonPropertyName("sharcIds")]
        public IEnumerable<string> SharcIds { get; set; }


        public SharcMqttClientConfiguration()
        {
            Server = "localhost";
            Port = 1883;
            QoS = 1;
            RetryInterval = 5000;
        }
    }
}
