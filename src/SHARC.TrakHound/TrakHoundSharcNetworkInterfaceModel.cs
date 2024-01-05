using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(DefinitionId = "SHARC.NetworkInterface")]
    public class TrakHoundSharcNetworkInterfaceModel
    {
        [JsonPropertyName("type")]
        [TrakHoundString("type", DefinitionId = "SHARC.NetworkInterface.Type")]
        public string Type { get; set; }

        [JsonPropertyName("static")]
        [TrakHoundBoolean("static", DefinitionId = "SHARC.NetworkInterface.Static")]
        public bool Static { get; set; }

        [JsonPropertyName("ip")]
        [TrakHoundString("ip", DefinitionId = "SHARC.NetworkInterface.IpAddress")]
        public string IpAddress { get; set; }

        [JsonPropertyName("gw")]
        [TrakHoundString("gw", DefinitionId = "SHARC.NetworkInterface.Gateway")]
        public string Gateway { get; set; }

        [JsonPropertyName("mask")]
        [TrakHoundString("mask", DefinitionId = "SHARC.NetworkInterface.SubnetMask")]
        public string SubnetMask { get; set; }

        [JsonPropertyName("dns")]
        [TrakHoundString("dns", DefinitionId = "SHARC.NetworkInterface.DNS")]
        public string DNS { get; set; }

        [JsonPropertyName("mac")]
        [TrakHoundString("mac", DefinitionId = "SHARC.NetworkInterface.MacAddress")]
        public string MacAddress { get; set; }

        [JsonPropertyName("quality")]
        [TrakHoundNumber("quality", DefinitionId = "SHARC.NetworkInterface.Quality")]
        public int Quality { get; set; }

        [JsonPropertyName("ssid")]
        [TrakHoundString("ssid", DefinitionId = "SHARC.NetworkInterface.SSID")]
        public string SSID { get; set; }

        [JsonPropertyName("lan_fallback")]
        [TrakHoundNumber("lan_fallback", DefinitionId = "SHARC.NetworkInterface.LanFallback")]
        public int LanFallback { get; set; }


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
                LanFallback = networkInterface.LanFallbackSeconds;
            }
        }
    }
}
