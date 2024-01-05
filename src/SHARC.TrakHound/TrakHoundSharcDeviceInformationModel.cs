using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(DefinitionId = "SHARC.DeviceInformation")]
    public class TrakHoundSharcDeviceInformationModel
    {
        [JsonPropertyName("mfg")]
        [TrakHoundString("mfg", DefinitionId = "SHARC.DeviceInformation.Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("model")]
        [TrakHoundString("model", DefinitionId = "SHARC.DeviceInformation.Model")]
        public string Model { get; set; }

        [JsonPropertyName("serial")]
        [TrakHoundString("serial", DefinitionId = "SHARC.DeviceInformation.Serial")]
        public string Serial { get; set; }

        [JsonPropertyName("hw")]
        [TrakHoundString("hw", DefinitionId = "SHARC.DeviceInformation.HardwareVersion")]
        public string HardwareVersion { get; set; }

        [JsonPropertyName("fw")]
        [TrakHoundString("fw", DefinitionId = "SHARC.DeviceInformation.FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("sw")]
        [TrakHoundString("sw", DefinitionId = "SHARC.DeviceInformation.SoftwareVersion")]
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
