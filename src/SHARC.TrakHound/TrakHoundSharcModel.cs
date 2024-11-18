using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    [TrakHoundObject]
    [TrakHoundDefinition("SHARC")]
    public class TrakHoundSharcModel
    {
        [JsonPropertyName("sharcId")]
        [TrakHoundName]
        [TrakHoundMetadata]
        public string SharcId { get; set; }

        [JsonIgnore]
        [TrakHoundPath]
        public string Path { get; set; }

        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [TrakHoundBoolean]
        public bool Enabled { get; set; }

        [JsonPropertyName("description")]
        [TrakHoundString]
        public string Description { get; set; }

        [JsonPropertyName("avail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [TrakHoundBoolean]
        //[TrakHoundObservation(DataType = TrakHound.Entities.TrakHoundObservationDataType.Boolean)]
        [TrakHoundDefinition("SHARC.Availability")]
        public bool Availability { get; set; }

        [TrakHoundObject]
        public TrakHoundSharcMqttInformationModel mqtt { get; set; }

        [TrakHoundObject]
        public TrakHoundSharcDeviceInformationModel ver { get; set; }

        [TrakHoundObject]
        public TrakHoundSharcNetworkInterfaceModel net { get; set; }

        [TrakHoundObject]
        public TrakHoundSharcBootCounterModel rc { get; set; }

        [TrakHoundObject]
        public TrakHoundSharcSensorConfigurationModel sensor { get; set; }
    }
}
