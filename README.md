# TrakHound.SHARC
Collect & View Data from SHARC Sensors. 

SHARC: https://www.mriiot.com/sharc

TrakHound: https://github.com/TrakHound/TrakHound

## Live Demo
View a live running demo at:
- https://www.trakhound.com/demo/sharc
- https://www.trakhound.com/demo/sharc/sensors/48e7290b118c/analyze

## Get Started
- Install TrakHound Instance : https://github.com/TrakHound/TrakHound/releases/download/v0.1.3/trakhound-instance-0.1.3-install.exe
- Browse to TrakHound Admin UI : http://localhost:8472/_admin/packages
- Install the **SHARC.Bundle** package
- Browse to SHARC.App : http://localhost:8472/sharc
- Click **Add Sensor**, enter MQTT Broker and click **Test Connection**, select SHARC from list
- Click on **View** in sensors table

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

## User Interface
### Query Raw Values
Query Raw sensor values using the **Raw** query type. This displays each data point within the selected time range.

![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot%202024-11-21%20140921.png)

### Query Aggregate Values
Query Aggregate sensor values using the **Aggregate** query type. This displays data using the selected aggregate type (Mean, Median, Max, Min) with the selected Aggregate Window.

![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot%202024-11-21%20140921.png)

### View Live Values
![IMAGE](https://raw.githubusercontent.com/TrakHound/TrakHound.SHARC/refs/heads/main/img/Screenshot%202024-11-21%20141102.png)

## Storage
Data values are stored using TrakHound Observations and will require a Driver that supports it. See below for example:

### InfluxDB
- Install & Setup InfluxDB : https://www.influxdata.com
- Install & Configure **TrakHound.InfluxDB.Drivers** using TrakHound Admin UI

