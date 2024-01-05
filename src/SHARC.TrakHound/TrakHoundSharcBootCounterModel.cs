using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcBootCounterModel
    {
        [JsonPropertyName("power_on")]
        [TrakHoundNumber("power_on")]
        public int PowerOn { get; set; }

        [JsonPropertyName("hard_reset")]
        [TrakHoundNumber("hard_reset")]
        public int HardReset { get; set; }

        [JsonPropertyName("watchdog_reset")]
        [TrakHoundNumber("watchdog_reset")]
        public int WatchdogReset { get; set; }

        [JsonPropertyName("deep_sleep")]
        [TrakHoundNumber("deep_sleep")]
        public int DeepSleep { get; set; }

        [JsonPropertyName("soft_reset")]
        [TrakHoundNumber("soft_reset")]
        public int SoftReset { get; set; }


        public TrakHoundSharcBootCounterModel() { }

        public TrakHoundSharcBootCounterModel(SharcBootCounter bootCounter)
        {
            if (bootCounter != null)
            {
                PowerOn = bootCounter.PowerOn;
                HardReset = bootCounter.HardReset;
                WatchdogReset = bootCounter.WatchdogReset;
                DeepSleep = bootCounter.DeepSleep;
                SoftReset = bootCounter.SoftReset;
            }
        }
    }
}
