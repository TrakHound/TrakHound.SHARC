using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(BasePath = "sharc")]
    public class TrakHoundSharcModel
    {
        [JsonIgnore]
        [TrakHoundName]
        public string SharcId { get; set; }

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
