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
