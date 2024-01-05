using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcNetworkInterfaceModel
    {
        [JsonPropertyName("type")]
        [TrakHoundString("type")]
        public string Type { get; set; }

        [JsonPropertyName("static")]
        [TrakHoundBoolean("static")]
        public bool Static { get; set; }

        [JsonPropertyName("ip")]
        [TrakHoundString("ip")]
        public string IpAddress { get; set; }

        [JsonPropertyName("gw")]
        [TrakHoundString("gw")]
        public string Gateway { get; set; }

        [JsonPropertyName("mask")]
        [TrakHoundString("mask")]
        public string SubnetMask { get; set; }

        [JsonPropertyName("dns")]
        [TrakHoundString("dns")]
        public string DNS { get; set; }

        [JsonPropertyName("mac")]
        [TrakHoundString("mac")]
        public string MacAddress { get; set; }

        [JsonPropertyName("quality")]
        [TrakHoundNumber("quality")]
        public int Quality { get; set; }

        [JsonPropertyName("ssid")]
        [TrakHoundString("ssid")]
        public string SSID { get; set; }

        [JsonPropertyName("lan_fallback")]
        [TrakHoundNumber("lan_fallback")]
        public int LanFallbackSeconds { get; set; }


        public TrakHoundSharcNetworkInterfaceModel() { }

        public TrakHoundSharcNetworkInterfaceModel(SharcNetworkInterface networkInterface)
        {
            if (networkInterface != null)
            {
                Type = networkInterface.Type;
                Static = networkInterface.Static;
                IpAddress = networkInterface.IpAddress;
                Gateway = networkInterface.Gateway;
                SubnetMask = networkInterface.SubnetMask;
                DNS = networkInterface.DNS;
                MacAddress = networkInterface.MacAddress;
                Quality = networkInterface.Quality;
                SSID = networkInterface.SSID;
                LanFallbackSeconds = networkInterface.LanFallbackSeconds;
            }
        }
    }
}
