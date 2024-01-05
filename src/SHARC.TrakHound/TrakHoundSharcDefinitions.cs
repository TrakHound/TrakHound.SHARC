using TrakHound.Entities;

namespace SHARC.TrakHound
{
    public class TrakHoundSharcDefinitions
    {
        public static readonly ITrakHoundDefinitionEntity Sharc = new TrakHoundDefinitionEntity("SHARC", "The SHARC is a universal sensor adapter that simplifies industrial sensor connectivity and data acquisition. The SHARC powers and measures signals from the connected sensor and publishes the data to an MQTT broker.");
        
        public static readonly ITrakHoundDefinitionEntity Availability = new TrakHoundDefinitionEntity("SHARC.Availability", "Availability, published upon connection. Device disconnect message is published by MQTT broker LWT mechanism.");

        public static readonly ITrakHoundDefinitionEntity BootCounter = new TrakHoundDefinitionEntity("SHARC.BootCounter", "Reboot causes and counters, published upon connection.");
        public static readonly ITrakHoundDefinitionEntity BootCounter_PowerOn = new TrakHoundDefinitionEntity("SHARC.BootCounter.PowerOn", "Power_On count.");
        public static readonly ITrakHoundDefinitionEntity BootCounter_HardReset = new TrakHoundDefinitionEntity("SHARC.BootCounter.HardReset", "Hard count.");
        public static readonly ITrakHoundDefinitionEntity BootCounter_WatchdogReset = new TrakHoundDefinitionEntity("SHARC.BootCounter.WatchdogReset", "WDT count.");
        public static readonly ITrakHoundDefinitionEntity BootCounter_DeepSleep = new TrakHoundDefinitionEntity("SHARC.BootCounter.DeepSleep", "Deep_Sleep count.");
        public static readonly ITrakHoundDefinitionEntity BootCounter_SoftReset = new TrakHoundDefinitionEntity("SHARC.BootCounter.SoftReset", "Soft count.");

        public static readonly ITrakHoundDefinitionEntity NetworkInterface = new TrakHoundDefinitionEntity("SHARC.NetworkInterface", "Current network interface values, published upon connection.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_Type = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.Type", "Current network type of WLAN or LAN.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_Static = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.Static", "Whether the IP configuration is static or dynamic.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_IpAddress = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.IpAddress", "Device IP Address.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_Gateway = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.Gateway", "Gateway IP Address.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_SubnetMask = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.SubnetMask", "Subnet mask.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_DNS = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.DNS", "DNS Server IP Address.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_MacAddress = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.MacAddress", "Device Hardware Address.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_Quality = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.Quality", "Connection quality.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_SSID = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.SSID", "(WLAN only) Wifi network name.");
        public static readonly ITrakHoundDefinitionEntity NetworkInterface_LanFallback = new TrakHoundDefinitionEntity("SHARC.NetworkInterface.LanFallback", "(WLAN only) Seconds to wait for Wifi connection before rebooting into LAN. Zero to disable this feature.");

        public static readonly ITrakHoundDefinitionEntity DeviceInformation = new TrakHoundDefinitionEntity("SHARC.DeviceInformation", "Device information, published upon connection.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_Manufacturer = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.Manufacturer", "Hardware manufacturer.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_Model = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.Model", "Hardware model.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_Serial = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.Serial", "Hardware serial number.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_HardwareVersion = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.HardwareVersion", "Hardware version.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_FirmwareVersion = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.FirmwareVersion", "Firmware version.");
        public static readonly ITrakHoundDefinitionEntity DeviceInformation_SoftwareVersion = new TrakHoundDefinitionEntity("SHARC.DeviceInformation.SoftwareVersion", "Software version.");

        public static readonly ITrakHoundDefinitionEntity MqttInformation = new TrakHoundDefinitionEntity("SHARC.MqttInformation", "MQTT connection information, published upon connection.");
        public static readonly ITrakHoundDefinitionEntity MqttInformation_Address = new TrakHoundDefinitionEntity("SHARC.MqttInformation.Address", "Broker address.");
        public static readonly ITrakHoundDefinitionEntity MqttInformation_Port = new TrakHoundDefinitionEntity("SHARC.MqttInformation.Port", "Broker port.");
        public static readonly ITrakHoundDefinitionEntity MqttInformation_User = new TrakHoundDefinitionEntity("SHARC.MqttInformation.User", "Username.");
        public static readonly ITrakHoundDefinitionEntity MqttInformation_Anonymous = new TrakHoundDefinitionEntity("SHARC.MqttInformation.Anonymous", "Is connection anonymous.");

        public static readonly ITrakHoundDefinitionEntity Sensor = new TrakHoundDefinitionEntity("SHARC.Sensor", "Values read from sensors.");


        public static IEnumerable<ITrakHoundDefinitionEntity> GetAll()
        {
            var definitions = new List<ITrakHoundDefinitionEntity>();

            definitions.Add(Sharc);

            definitions.Add(Availability);

            definitions.Add(BootCounter);
            definitions.Add(BootCounter_PowerOn);
            definitions.Add(BootCounter_HardReset);
            definitions.Add(BootCounter_WatchdogReset);
            definitions.Add(BootCounter_DeepSleep);
            definitions.Add(BootCounter_SoftReset);

            definitions.Add(NetworkInterface);
            definitions.Add(NetworkInterface_Type);
            definitions.Add(NetworkInterface_Static);
            definitions.Add(NetworkInterface_IpAddress);
            definitions.Add(NetworkInterface_Gateway);
            definitions.Add(NetworkInterface_SubnetMask);
            definitions.Add(NetworkInterface_DNS);
            definitions.Add(NetworkInterface_MacAddress);
            definitions.Add(NetworkInterface_Quality);
            definitions.Add(NetworkInterface_SSID);
            definitions.Add(NetworkInterface_LanFallback);

            definitions.Add(DeviceInformation);
            definitions.Add(DeviceInformation_Manufacturer);
            definitions.Add(DeviceInformation_Model);
            definitions.Add(DeviceInformation_Serial);
            definitions.Add(DeviceInformation_HardwareVersion);
            definitions.Add(DeviceInformation_FirmwareVersion);
            definitions.Add(DeviceInformation_SoftwareVersion);

            definitions.Add(MqttInformation);
            definitions.Add(MqttInformation_Address);
            definitions.Add(MqttInformation_Port);
            definitions.Add(MqttInformation_User);
            definitions.Add(MqttInformation_Anonymous);

            definitions.Add(Sensor);

            return definitions;
        }
    }
}
