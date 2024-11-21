// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    [TrakHoundObject]
    [TrakHoundDefinition("SHARC.NetworkInterface")]
    public class TrakHoundSharcNetworkInterfaceModel
    {
        [JsonPropertyName("type")]
        [TrakHoundString("type")]
        [TrakHoundDefinition("SHARC.NetworkInterface.Type")]
        public string Type { get; set; }

        [JsonPropertyName("static")]
        [TrakHoundBoolean("static")]
        [TrakHoundDefinition("SHARC.NetworkInterface.Static")]
        public bool Static { get; set; }

        [JsonPropertyName("ip")]
        [TrakHoundString("ip")]
        [TrakHoundDefinition("SHARC.NetworkInterface.IpAddress")]
        public string IpAddress { get; set; }

        [JsonPropertyName("gw")]
        [TrakHoundString("gw")]
        [TrakHoundDefinition("SHARC.NetworkInterface.Gateway")]
        public string Gateway { get; set; }

        [JsonPropertyName("mask")]
        [TrakHoundString("mask")]
        [TrakHoundDefinition("SHARC.NetworkInterface.SubnetMask")]
        public string SubnetMask { get; set; }

        [JsonPropertyName("dns")]
        [TrakHoundString("dns")]
        [TrakHoundDefinition("SHARC.NetworkInterface.DNS")]
        public string DNS { get; set; }

        [JsonPropertyName("mac")]
        [TrakHoundString("mac")]
        [TrakHoundDefinition("SHARC.NetworkInterface.MacAddress")]
        public string MacAddress { get; set; }

        [JsonPropertyName("quality")]
        [TrakHoundNumber(Name = "quality")]
        [TrakHoundDefinition("SHARC.NetworkInterface.Quality")]
        public int Quality { get; set; }

        [JsonPropertyName("ssid")]
        [TrakHoundString("ssid")]
        [TrakHoundDefinition("SHARC.NetworkInterface.SSID")]
        public string SSID { get; set; }

        [JsonPropertyName("lan_fallback")]
        [TrakHoundNumber(Name = "lan_fallback")]
        [TrakHoundDefinition("SHARC.NetworkInterface.LanFallback")]
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
