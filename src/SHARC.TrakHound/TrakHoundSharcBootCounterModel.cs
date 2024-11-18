using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    [TrakHoundObject]
    [TrakHoundDefinition("SHARC.Availability")]
    public class TrakHoundSharcBootCounterModel
    {
        [JsonPropertyName("power_on")]
        [TrakHoundNumber(Name = "power_on")]
        [TrakHoundDefinition("SHARC.BootCounter.PowerOn")]
        public int PowerOn { get; set; }

        [JsonPropertyName("hard_reset")]
        [TrakHoundNumber(Name = "hard_reset")]
        [TrakHoundDefinition("SHARC.BootCounter.HardReset")]
        public int HardReset { get; set; }

        [JsonPropertyName("watchdog_reset")]
        [TrakHoundNumber(Name = "watchdog_reset")]
        [TrakHoundDefinition("SHARC.BootCounter.WatchdogReset")]
        public int WatchdogReset { get; set; }

        [JsonPropertyName("deep_sleep")]
        [TrakHoundNumber(Name = "deep_sleep")]
        [TrakHoundDefinition("SHARC.BootCounter.DeepSleep")]
        public int DeepSleep { get; set; }

        [JsonPropertyName("soft_reset")]
        [TrakHoundNumber(Name = "soft_reset")]
        [TrakHoundDefinition("SHARC.BootCounter.SoftReset")]
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
