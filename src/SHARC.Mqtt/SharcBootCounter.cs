using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcBootCounter
    {
        /// <summary>
        /// Power_On count
        /// </summary>
        [JsonPropertyName("power_on")]
        public int PowerOn { get; set; }

        /// <summary>
        /// Hard count
        /// </summary>
        [JsonPropertyName("hard_reset")]
        public int HardReset { get; set; }

        /// <summary>
        /// WDT count
        /// </summary>
        [JsonPropertyName("watchdog_reset")]
        public int WatchdogReset { get; set; }

        /// <summary>
        /// Deep_Sleep count
        /// </summary>
        [JsonPropertyName("deep_sleep")]
        public int DeepSleep { get; set; }

        /// <summary>
        /// Soft count
        /// </summary>
        [JsonPropertyName("soft_reset")]
        public int SoftReset { get; set; }
    }
}
