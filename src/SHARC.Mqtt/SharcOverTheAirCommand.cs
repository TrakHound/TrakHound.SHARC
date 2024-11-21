// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcOverTheAirCommand
    {
        [JsonPropertyName("bin")]
        public string Bin { get; set; }
    }
}
