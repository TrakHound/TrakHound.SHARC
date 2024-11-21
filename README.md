# TrakHound.SHARC
Collect & View Data from SHARC Sensors. https://www.mriiot.com/sharc

## Live Demo
View a live running demo at:
- https://www.trakhound.com/demo/sharc
- https://www.trakhound.com/demo/sharc/sensors/48e7290b118c/analyze

## UI
### Query Raw Values
Query Raw sensor values using the **Raw** query type. This displays each data point within the selected time range.

![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot%202024-11-21%20140921.png)

### Query Aggregate Values
Query Aggregate sensor values using the **Aggregate** query type. This displays data using the selected aggregate type (Mean, Median, Max, Min) with the selected Aggregate Window.

![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot 2024-11-21 140921.png)

### View Live Values
![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot 2024-11-21 141102.png)

## TrakHound Packages
<table>
    <thead>
        <tr>
            <th style="text-align: left;min-width: 100px;">Type</th>
            <th style="text-align: left;min-width: 100px;">Name</th>
            <th style="text-align: left;">Source</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Bundle</td>
            <td>SHARC.Bundle</td>
            <td><a href="https://github.com/TrakHound/TrakHound.SHARC/tree/main/bundle">/bundle</a></td>
        </tr> 
        <tr>
            <td>App</td>
            <td>SHARC.App</td>
            <td><a href="https://github.com/TrakHound/TrakHound.SHARC/tree/main/src/SHARC.App">/src/SHARC.App</a></td>
        </tr>        
        <tr>
            <td>Api</td>
            <td>SHARC.Api</td>
            <td><a href="https://github.com/TrakHound/TrakHound.SHARC/tree/main/src/SHARC.Api">/src/SHARC.Api</a></td>
        </tr>
        <tr>
            <td>Api</td>
            <td>SHARC.Collection.Api</td>
            <td><a href="https://github.com/TrakHound/TrakHound.SHARC/tree/main/src/SHARC.Collection.Api">/src/SHARC.Collection.Api</a></td>
        </tr>
        <tr>
            <td>Service</td>
            <td>SHARC.Collection.Service</td>
            <td><a href="https://github.com/TrakHound/TrakHound.SHARC/tree/main/src/SHARC.Collection.Service">/src/SHARC.Collection.Service</a></td>
        </tr>  
    </tbody>
</table>

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
