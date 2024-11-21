// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    [TrakHoundObject]
    [TrakHoundDefinition("SHARC.DeviceInformation")]
    public class TrakHoundSharcDeviceInformationModel
    {
        [JsonPropertyName("mfg")]
        [TrakHoundString("mfg")]
        [TrakHoundDefinition("SHARC.DeviceInformation.Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("model")]
        [TrakHoundString("model")]
        [TrakHoundDefinition("SHARC.DeviceInformation.Model")]
        public string Model { get; set; }

        [JsonPropertyName("serial")]
        [TrakHoundString("serial")]
        [TrakHoundDefinition("SHARC.DeviceInformation.Serial")]
        public string Serial { get; set; }

        [JsonPropertyName("hw")]
        [TrakHoundString("hw")]
        [TrakHoundDefinition("SHARC.DeviceInformation.HardwareVersion")]
        public string HardwareVersion { get; set; }

        [JsonPropertyName("fw")]
        [TrakHoundString("fw")]
        [TrakHoundDefinition("SHARC.DeviceInformation.FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("sw")]
        [TrakHoundString("sw")]
        [TrakHoundDefinition("SHARC.DeviceInformation.SoftwareVersion")]
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
