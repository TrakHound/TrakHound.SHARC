using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcDeviceInformationModel
    {
        [JsonPropertyName("mfg")]
        [TrakHoundString("mfg")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("model")]
        [TrakHoundString("model")]
        public string Model { get; set; }

        [JsonPropertyName("serial")]
        [TrakHoundString("serial")]
        public string Serial { get; set; }

        [JsonPropertyName("hw")]
        [TrakHoundString("hw")]
        public string HardwareVersion { get; set; }

        [JsonPropertyName("fw")]
        [TrakHoundString("fw")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("sw")]
        [TrakHoundString("sw")]
        public string SoftwareVersion { get; set; }


        public TrakHoundSharcDeviceInformationModel() { }

        public TrakHoundSharcDeviceInformationModel(SharcDeviceInformation deviceInformation)
        {
            if (deviceInformation != null)
            {
                Manufacturer = deviceInformation.Manufacturer;
                Model = deviceInformation.Model;
                Serial = deviceInformation.Serial;
                HardwareVersion = deviceInformation.HardwareVersion;
                FirmwareVersion = deviceInformation.FirmwareVersion;
                SoftwareVersion = deviceInformation.SoftwareVersion;
            }
        }
    }
}
