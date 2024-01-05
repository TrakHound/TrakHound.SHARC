using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(DefinitionId = "SHARC.BootCounter")]
    public class TrakHoundSharcBootCounterModel
    {
        [JsonPropertyName("power_on")]
        [TrakHoundNumber("power_on", DefinitionId = "SHARC.BootCounter.PowerOn")]
        public int PowerOn { get; set; }

        [JsonPropertyName("hard_reset")]
        [TrakHoundNumber("hard_reset", DefinitionId = "SHARC.BootCounter.HardReset")]
        public int HardReset { get; set; }

        [JsonPropertyName("watchdog_reset")]
        [TrakHoundNumber("watchdog_reset", DefinitionId = "SHARC.BootCounter.WatchdogReset")]
        public int WatchdogReset { get; set; }

        [JsonPropertyName("deep_sleep")]
        [TrakHoundNumber("deep_sleep", DefinitionId = "SHARC.BootCounter.DeepSleep")]
        public int DeepSleep { get; set; }

        [JsonPropertyName("soft_reset")]
        [TrakHoundNumber("soft_reset", DefinitionId = "SHARC.BootCounter.SoftReset")]
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
