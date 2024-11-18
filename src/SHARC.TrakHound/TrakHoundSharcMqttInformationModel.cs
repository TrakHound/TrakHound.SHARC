using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    [TrakHoundObject]
    [TrakHoundDefinition("SHARC.MqttInformation")]
    public class TrakHoundSharcMqttInformationModel
    {
        [JsonPropertyName("address")]
        [TrakHoundString("address")]
        [TrakHoundDefinition("SHARC.MqttInformation.Address")]
        public string Address { get; set; }

        [JsonPropertyName("port")]
        [TrakHoundNumber(Name = "port")]
        [TrakHoundDefinition("SHARC.MqttInformation.Port")]
        public int Port { get; set; }

        [JsonPropertyName("user")]
        [TrakHoundString("user")]
        [TrakHoundDefinition("SHARC.MqttInformation.User")]
        public string User { get; set; }

        [JsonPropertyName("anonymous")]
        [TrakHoundBoolean("anonymous")]
        [TrakHoundDefinition("SHARC.MqttInformation.Anonymous")]
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
