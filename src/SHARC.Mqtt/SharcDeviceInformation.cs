using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcDeviceInformation
    {
        /// <summary>
        /// Hardware manufacturer
        /// </summary>
        [JsonPropertyName("mfg")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Hardware model
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// Hardware serial number
        /// </summary>
        [JsonPropertyName("serial")]
        public string Serial { get; set; }

        /// <summary>
        /// Hardware version
        /// </summary>
        [JsonPropertyName("gw")]
        public string HardwareVersion { get; set; }

        /// <summary>
        /// Firmware version
        /// </summary>
        [JsonPropertyName("fw")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Software version
        /// </summary>
        [JsonPropertyName("sw")]
        public string SoftwareVersion { get; set; }
    }
}
