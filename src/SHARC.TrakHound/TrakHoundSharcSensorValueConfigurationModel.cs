﻿using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcSensorValueConfigurationModel
    {
        [JsonPropertyName("deadband")]
        [TrakHoundNumber("deadband", DefinitionId = "SHARC.SensorValue.Deadband")]
        public int Deadband { get; set; }

        [JsonPropertyName("calibrate")]
        [TrakHoundString("calibrate", DefinitionId = "SHARC.SensorValue.Calibrate")]
        public string Calibrate { get; set; }

        [JsonPropertyName("calibrated_range")]
        [TrakHoundSet("calibrated_range", DefinitionId = "SHARC.SensorValue.CalibratedRange")]
        public IEnumerable<int> CalibratedRange { get; set; }

        [JsonPropertyName("convert")]
        [TrakHoundString("convert", DefinitionId = "SHARC.SensorValue.Convert")]
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
