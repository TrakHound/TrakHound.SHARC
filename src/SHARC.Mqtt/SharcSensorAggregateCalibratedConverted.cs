// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcSensorAggregateCalibratedConverted
    {
        /// <summary>
        /// value
        /// </summary>
        [JsonPropertyName("v")]
        public double Value { get; set; }

        /// <summary>
        /// units
        /// </summary>
        [JsonPropertyName("u")]
        public string Units { get; set; }
    }
}
