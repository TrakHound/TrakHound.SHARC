using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC
{
    public class TrakHoundSharcSensorValueConfigurationModel
    {
        [JsonPropertyName("deadband")]
        [TrakHoundNumber(Name = "deadband")]
        [TrakHoundDefinition("SHARC.SensorValue.Deadband")]
        public int Deadband { get; set; }

        [JsonPropertyName("calibrate")]
        [TrakHoundString("calibrate")]
        [TrakHoundDefinition("SHARC.SensorValue.Calibrate")]
        public string Calibrate { get; set; }

        [JsonPropertyName("calibrated_range")]
        [TrakHoundSet("calibrated_range")]
        [TrakHoundDefinition("SHARC.SensorValue.CalibratedRange")]
        public IEnumerable<int> CalibratedRange { get; set; }

        [JsonPropertyName("convert")]
        [TrakHoundString("convert")]
        [TrakHoundDefinition("SHARC.SensorValue.Convert")]
        public string Convert { get; set; }


        public TrakHoundSharcSensorValueConfigurationModel() { }

        public TrakHoundSharcSensorValueConfigurationModel(SharcSensorValueConfiguration sensorConfiguration)
        {
            if (sensorConfiguration != null)
            {
                Deadband = sensorConfiguration.Deadband;
                Calibrate = sensorConfiguration.Calibrate;
                CalibratedRange = sensorConfiguration.CalibratedRange;
                Convert = sensorConfiguration.Convert;
            }
        }
    }
}
