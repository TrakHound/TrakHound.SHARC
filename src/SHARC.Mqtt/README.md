
## SHARC.Mqtt
Used to subscribe to an MQTT Broker to read SHARC data

```c#
using SHARC.Mqtt;

var client = new SharcMqttClient("mosquitto.spb.mtcup.org", 1884, "409151d72b34");
client.Connected += (s, args) => Console.WriteLine("MQTT Broker Connected");
client.AvailabilityReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.BootCounterReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.NetworkInterfaceReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.DeviceInformationReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.MqttInformationReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.SensorConfigurationReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.DistinctSensorValueReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.AggregateSensorValueReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.AggregateCalibratedConvertedSensorValueReceived += (s, p) =>
{
    Console.WriteLine(p);
};
client.UserDataReceived += (s, p) =>
{
    Console.WriteLine(p);
};

client.Start();

Console.WriteLine("Started");
Console.ReadLine();

client.Stop();
```