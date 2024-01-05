using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcMqttInformationModel
    {
        [JsonPropertyName("address")]
        [TrakHoundString("address")]
        public string Address { get; set; }

        [JsonPropertyName("port")]
        [TrakHoundNumber("port")]
        public int Port { get; set; }

        [JsonPropertyName("user")]
        [TrakHoundString("user")]
        public string User { get; set; }

        [JsonPropertyName("anonymous")]
        [TrakHoundBoolean("anonymous")]
        public bool Anonymous { get; set; }


        public TrakHoundSharcMqttInformationModel() { }

        public TrakHoundSharcMqttInformationModel(SharcMqttInformation mqttInformation)
        {
            if (mqttInformation != null)
            {
                Address = mqttInformation.Address;
                Port = mqttInformation.Port;
                User = mqttInformation.User;
                Anonymous = mqttInformation.Anonymous;
            }
        }
    }
}
