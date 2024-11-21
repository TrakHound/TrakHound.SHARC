// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using TrakHound.Requests;

namespace SHARC
{
    public class TrakHoundSharcDefinitions
    {
        public static readonly TrakHoundDefinitionEntry Sharc = new TrakHoundDefinitionEntry("SHARC", "The SHARC is a universal sensor adapter that simplifies industrial sensor connectivity and data acquisition. The SHARC powers and measures signals from the connected sensor and publishes the data to an MQTT broker.");
        
        public static readonly TrakHoundDefinitionEntry Availability = new TrakHoundDefinitionEntry("SHARC.Availability", "Availability, published upon connection. Device disconnect message is published by MQTT broker LWT mechanism.");

        public static readonly TrakHoundDefinitionEntry BootCounter = new TrakHoundDefinitionEntry("SHARC.BootCounter", "Reboot causes and counters, published upon connection.");
        public static readonly TrakHoundDefinitionEntry BootCounter_PowerOn = new TrakHoundDefinitionEntry("SHARC.BootCounter.PowerOn", "Power_On count.");
        public static readonly TrakHoundDefinitionEntry BootCounter_HardReset = new TrakHoundDefinitionEntry("SHARC.BootCounter.HardReset", "Hard count.");
        public static readonly TrakHoundDefinitionEntry BootCounter_WatchdogReset = new TrakHoundDefinitionEntry("SHARC.BootCounter.WatchdogReset", "WDT count.");
        public static readonly TrakHoundDefinitionEntry BootCounter_DeepSleep = new TrakHoundDefinitionEntry("SHARC.BootCounter.DeepSleep", "Deep_Sleep count.");
        public static readonly TrakHoundDefinitionEntry BootCounter_SoftReset = new TrakHoundDefinitionEntry("SHARC.BootCounter.SoftReset", "Soft count.");

        public static readonly TrakHoundDefinitionEntry NetworkInterface = new TrakHoundDefinitionEntry("SHARC.NetworkInterface", "Current network interface values, published upon connection.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_Type = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.Type", "Current network type of WLAN or LAN.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_Static = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.Static", "Whether the IP configuration is static or dynamic.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_IpAddress = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.IpAddress", "Device IP Address.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_Gateway = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.Gateway", "Gateway IP Address.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_SubnetMask = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.SubnetMask", "Subnet mask.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_DNS = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.DNS", "DNS Server IP Address.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_MacAddress = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.MacAddress", "Device Hardware Address.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_Quality = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.Quality", "Connection quality.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_SSID = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.SSID", "(WLAN only) Wifi network name.");
        public static readonly TrakHoundDefinitionEntry NetworkInterface_LanFallback = new TrakHoundDefinitionEntry("SHARC.NetworkInterface.LanFallback", "(WLAN only) Seconds to wait for Wifi connection before rebooting into LAN. Zero to disable this feature.");

        public static readonly TrakHoundDefinitionEntry DeviceInformation = new TrakHoundDefinitionEntry("SHARC.DeviceInformation", "Device information, published upon connection.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_Manufacturer = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.Manufacturer", "Hardware manufacturer.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_Model = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.Model", "Hardware model.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_Serial = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.Serial", "Hardware serial number.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_HardwareVersion = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.HardwareVersion", "Hardware version.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_FirmwareVersion = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.FirmwareVersion", "Firmware version.");
        public static readonly TrakHoundDefinitionEntry DeviceInformation_SoftwareVersion = new TrakHoundDefinitionEntry("SHARC.DeviceInformation.SoftwareVersion", "Software version.");

        public static readonly TrakHoundDefinitionEntry MqttInformation = new TrakHoundDefinitionEntry("SHARC.MqttInformation", "MQTT connection information, published upon connection.");
        public static readonly TrakHoundDefinitionEntry MqttInformation_Address = new TrakHoundDefinitionEntry("SHARC.MqttInformation.Address", "Broker address.");
        public static readonly TrakHoundDefinitionEntry MqttInformation_Port = new TrakHoundDefinitionEntry("SHARC.MqttInformation.Port", "Broker port.");
        public static readonly TrakHoundDefinitionEntry MqttInformation_User = new TrakHoundDefinitionEntry("SHARC.MqttInformation.User", "Username.");
        public static readonly TrakHoundDefinitionEntry MqttInformation_Anonymous = new TrakHoundDefinitionEntry("SHARC.MqttInformation.Anonymous", "Is connection anonymous.");

        public static readonly TrakHoundDefinitionEntry Sensor = new TrakHoundDefinitionEntry("SHARC.Sensor", "Values read from sensors.");


        public static IEnumerable<TrakHoundDefinitionEntry> GetAll()
        {
            var definitions = new List<TrakHoundDefinitionEntry>();

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
