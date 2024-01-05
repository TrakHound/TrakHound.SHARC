using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcNetworkInterface
    {
        /// <summary>
        /// Current network type of WLAN or LAN
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Whetehr the IP configuratoin is static or dynamic
        /// </summary>
        [JsonPropertyName("static")]
        public bool Static { get; set; }

        /// <summary>
        /// Device IP Address
        /// </summary>
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gateway IP Address
        /// </summary>
        [JsonPropertyName("gw")]
        public string Gateway { get; set; }

        /// <summary>
        /// Subnet Mask
        /// </summary>
        [JsonPropertyName("mask")]
        public string SubnetMask { get; set; }

        /// <summary>
        /// DNS Server IP Addresss
        /// </summary>
        [JsonPropertyName("dns")]
        public string DNS { get; set; }

        /// <summary>
        /// Device Hardware Address
        /// </summary>
        [JsonPropertyName("mac")]
        public string MacAddress { get; set; }

        /// <summary>
        /// Connection Quality
        /// </summary>
        [JsonPropertyName("quality")]
        public int Quality { get; set; }

        /// <summary>
        /// (WLAN only) Wifi network name
        /// </summary>
        [JsonPropertyName("ssid")]
        public string SSID { get; set; }

        /// <summary>
        /// (WLAN only) Seconds to wait for Wifi connection before rebooting into LAN. Zero to disable this feature.
        /// </summary>
        [JsonPropertyName("lan_fallback_s")]
        public int LanFallbackSeconds { get; set; }
    }
}
