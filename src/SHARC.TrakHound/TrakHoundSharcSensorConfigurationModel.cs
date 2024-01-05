using SHARC.Mqtt;
using System.Text.Json.Serialization;
using TrakHound.Serialization;

namespace SHARC.TrakHound
{
    [TrakHoundObject(DefinitionId = "SHARC.Sensor")]
    public class TrakHoundSharcSensorConfigurationModel
    {
        [JsonPropertyName("aggregate")]
        [TrakHoundBoolean("aggregate", DefinitionId = "SHARC.Sensor.Aggregate")]
        public bool Aggregate { get; set; }

        [JsonPropertyName("calibrate")]
        [TrakHoundBoolean("calibrate", DefinitionId = "SHARC.Sensor.Calibrate")]
        public bool Calibrate { get; set; }

        [JsonPropertyName("convert")]
        [TrakHoundBoolean("convert", DefinitionId = "SHARC.Sensor.Convert")]
        public bool Convert { get; set; }

        [JsonPropertyName("s0")]
        [TrakHoundObject("s0")]
        public TrakHoundSharcSensorValueConfigurationModel S0 { get; set; }

        [JsonPropertyName("s1")]
        [TrakHoundObject("s1")]
        public TrakHoundSharcSensorValueConfigurationModel S1 { get; set; }

        [JsonPropertyName("s2")]
        [TrakHoundObject("s2")]
        public TrakHoundSharcSensorValueConfigurationModel S2 { get; set; }

        [JsonPropertyName("s3")]
        [TrakHoundObject("s3")]
        public TrakHoundSharcSensorValueConfigurationModel S3 { get; set; }


        public TrakHoundSharcSensorConfigurationModel() { }

        public TrakHoundSharcSensorConfigurationModel(SharcSensorConfiguration sensorConfiguration)
        {
            if (sensorConfiguration != null)
            {
                Aggregate = sensorConfiguration.Aggregate;
                Calibrate = sensorConfiguration.Calibrate;
                Convert = sensorConfiguration.Convert;

                if (sensorConfiguration.S0 != null) S0 = new TrakHoundSharcSensorValueConfigurationModel(sensorConfiguration.S0);
                if (sensorConfiguration.S1 != null) S1 = new TrakHoundSharcSensorValueConfigurationModel(sensorConfiguration.S1);
                if (sensorConfiguration.S2 != null) S2 = new TrakHoundSharcSensorValueConfigurationModel(sensorConfiguration.S2);
                if (sensorConfiguration.S3 != null) S3 = new TrakHoundSharcSensorValueConfigurationModel(sensorConfiguration.S3);
            }
        }
    }
}
