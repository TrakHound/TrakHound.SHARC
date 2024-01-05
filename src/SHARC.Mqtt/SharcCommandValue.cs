using System.Text.Json.Serialization;

namespace SHARC.Mqtt
{
    public class SharcCommandValue
    {
        [JsonPropertyName("device.ota")]
        public SharcOverTheAirCommand DeviceOTA { get; set; }

        [JsonPropertyName("device.network.wlan")]
        public bool DeviceNetworkWLAN { get; set; }

        [JsonPropertyName("device.network.lan")]
        public bool DeviceNetworkLAN { get; set; }

        [JsonPropertyName("device.reset")]
        public bool DeviceReset { get; set; }

        [JsonPropertyName("device.reset.mqtt")]
        public bool DeviceResetMqtt { get; set; }

        [JsonPropertyName("device.reset.ble")]
        public bool DeviceResetBle { get; set; }

        [JsonPropertyName("device.save")]
        public bool DeviceSave { get; set; }

        [JsonPropertyName("device.save.mqtt")]
        public bool DeviceSaveMqtt { get; set; }

        [JsonPropertyName("device.save.ble")]
        public bool DeviceSaveBle { get; set; }


        [JsonPropertyName("di.counter.reset")]
        public bool DigitalInputCounterReset { get; set; }

        [JsonPropertyName("io.publish")]
        public bool InputOutputPublish { get; set; }

        [JsonPropertyName("ud.set")]
        public SharcUserData UserDataSet { get; set; }
    }
}
