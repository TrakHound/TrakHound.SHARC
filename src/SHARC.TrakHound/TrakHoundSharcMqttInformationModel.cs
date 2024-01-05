using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(DefinitionId = "SHARC.MqttInformation")]
    public class TrakHoundSharcMqttInformationModel
    {
        [JsonPropertyName("address")]
        [TrakHoundString("address", DefinitionId = "SHARC.MqttInformation.Address")]
        public string Address { get; set; }

        [JsonPropertyName("port")]
        [TrakHoundNumber("port", DefinitionId = "SHARC.MqttInformation.Port")]
        public int Port { get; set; }

        [JsonPropertyName("user")]
        [TrakHoundString("user", DefinitionId = "SHARC.MqttInformation.User")]
        public string User { get; set; }

        [JsonPropertyName("anonymous")]
        [TrakHoundBoolean("anonymous", DefinitionId = "SHARC.MqttInformation.Anonymous")]
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
