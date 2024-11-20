using System.Text.Json.Serialization;
using TrakHound;
using TrakHound.Api;
using TrakHound.Requests;
using TrakHound.Serialization;

namespace SHARC.Api
{
    public partial class Controller : TrakHoundApiController
    {
        class SharcSensorInformation
        {
            [JsonPropertyName("name")]
            [TrakHoundName]
            public string Name { get; set; }

            [JsonPropertyName("description")]
            [TrakHoundMetadata("Description")]
            public string Description { get; set; }

            [JsonPropertyName("units")]
            [TrakHoundMetadata("Units")]
            public string Units { get; set; }
        }

        class ValueResponse
        {
            [JsonPropertyName("v")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public double Value { get; set; }

            [JsonPropertyName("t")]
            public DateTime Timestamp { get; set; }
        }

        class AggregateResponse
        {
            [JsonPropertyName("s")]
            public DateTime Start { get; set; }

            [JsonPropertyName("e")]
            public DateTime End { get; set; }

            [JsonPropertyName("v")]
            [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
            public double Value { get; set; }
        }



        [TrakHoundApiQuery]
        public async Task<TrakHoundApiResponse> GetList()
        {
            var models = await Client.Entities.Get<TrakHoundSharcModel>($"type=SHARC");
            if (!models.IsNullOrEmpty())
            {
                return Ok(models);
            }
            else
            {
                return NotFound("No SHARCs Found");
            }
        }

        [TrakHoundApiQuery("{sharcId}")]
        public async Task<TrakHoundApiResponse> GetInformation([FromRoute] string sharcId)
        {
            if (!string.IsNullOrEmpty(sharcId))
            {
                var model = await Client.Entities.GetSingle<TrakHoundSharcModel>($"type=SHARC&meta@SharcId={sharcId}");
                if (model != null)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound("SHARC Not Found");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io")]
        public async Task<TrakHoundApiResponse> GetSensorInformation([FromRoute] string sharcId)
        {
            if (!string.IsNullOrEmpty(sharcId))
            {
                var objs = await Client.Entities.GetObjects($"type=SHARC&meta@SharcId={sharcId}/io/*");
                if (objs != null)
                {
                    return Ok(objs.Select(o => o.Name));
                }
                else
                {
                    return NotFound("SHARC Not Found");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io/{sensorName}")]
        public async Task<TrakHoundApiResponse> GetSensorInformation([FromRoute] string sharcId, [FromRoute] string sensorName)
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var sensorPath = $"type=SHARC&meta@SharcId={sharcId}/io/{sensorName}";
                var model = await Client.Entities.GetSingle<SharcSensorInformation>(sensorPath);
                if (model != null)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound("SHARC Sensor Not Found");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io/{sensorName}/values")]
        public async Task<TrakHoundApiResponse> QuerySensorValues(
            [FromRoute] string sharcId, 
            [FromRoute] string sensorName,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 1000,
            [FromQuery] int sortOrder = (int)SortOrder.Ascending
            )
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var sharcObj = await Client.Entities.GetObject($"type=SHARC&meta@SharcId={sharcId}");
                if (sharcObj != null)
                {
                    var sensorPath = TrakHoundPath.Combine(sharcObj.Path, "io", sensorName);

                    var sensorObj = await Client.Entities.GetObject(sensorPath);
                    if (sensorObj != null)
                    {
                        var observations = (await Client.Entities.GetObservationValues(sensorObj.Path, from, to, skip, take, (SortOrder)sortOrder))?.GetValueOrDefault(sensorObj.Path);
                        if (!observations.IsNullOrEmpty())
                        {
                            var valueResponses = new List<ValueResponse>(observations.Count());
                            foreach (var observation in observations)
                            {
                                var valueResponse = new ValueResponse();
                                valueResponse.Value = observation.Value.ToDouble();
                                valueResponse.Timestamp = observation.Timestamp;
                                valueResponses.Add(valueResponse);
                            }

                            return Ok(valueResponses);
                        }
                        else
                        {
                            return NotFound($"No Sensor Values Found between '{from.ToString("o")}' and '{to.ToString("o")}'");
                        }
                    }
                    else
                    {
                        return NotFound($"SHARC Sensor Not Found : ID = {sharcId} : SensorName = {sensorName}");
                    }
                }
                else
                {
                    return NotFound($"SHARC Not Found : ID = {sharcId}");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io/{sensorName}/aggregate")]
        public async Task<TrakHoundApiResponse> AggregateSensorValues(
            [FromRoute] string sharcId,
            [FromRoute] string sensorName,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] string aggregateType,
            [FromQuery] string aggregateWindow
            )
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var sharcObj = await Client.Entities.GetObject($"type=SHARC&meta@SharcId={sharcId}");
                if (sharcObj != null)
                {
                    var sensorPath = TrakHoundPath.Combine(sharcObj.Path, "io", sensorName);

                    var sensorObj = await Client.Entities.GetObject(sensorPath);
                    if (sensorObj != null)
                    {
                        var type = aggregateType.ConvertEnum<TrakHoundAggregateType>();
                        var window = (long)aggregateWindow.ToTimeSpan().TotalNanoseconds;

                        var aggregateWindows = await Client.System.Entities.Objects.Observation.AggregateWindowByObject(sensorPath, type, window, from.ToUnixTime(), to.ToUnixTime());
                        if (!aggregateWindows.IsNullOrEmpty())
                        {
                            var results = new List<AggregateResponse>();
                            foreach (var x in aggregateWindows)
                            {
                                var result = new AggregateResponse();
                                result.Start = x.Start.ToDateTime();
                                result.End = x.End.ToDateTime();
                                result.Value = x.Value;
                                results.Add(result);
                            }

                            return Ok(results);
                        }
                        else
                        {
                            return NotFound($"No Sensor Values Found between '{from.ToString("o")}' and '{to.ToString("o")}'");
                        }
                    }
                    else
                    {
                        return NotFound($"SHARC Sensor Not Found : ID = {sharcId} : SensorName = {sensorName}");
                    }
                }
                else
                {
                    return NotFound($"SHARC Not Found : ID = {sharcId}");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io/{sensorName}/count")]
        public async Task<TrakHoundApiResponse> QuerySensorValueCount(
            [FromRoute] string sharcId,
            [FromRoute] string sensorName,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to
            )
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var sharcObj = await Client.Entities.GetObject($"type=SHARC&meta@SharcId={sharcId}");
                if (sharcObj != null)
                {
                    var sensorPath = TrakHoundPath.Combine(sharcObj.Path, "io", sensorName);

                    var sensorObj = await Client.Entities.GetObject(sensorPath);
                    if (sensorObj != null)
                    {
                        var count = (await Client.System.Entities.Objects.Observation.CountByObject(sensorPath, from.ToUnixTime(), to.ToUnixTime()))?.FirstOrDefault();
                        if (count != null)
                        {
                            return Ok(count.Count.ToString());
                        }
                        else
                        {
                            return NotFound($"No Sensor Values Found between '{from.ToString("o")}' and '{to.ToString("o")}'");
                        }
                    }
                    else
                    {
                        return NotFound($"SHARC Sensor Not Found : ID = {sharcId} : SensorName = {sensorName}");
                    }
                }
                else
                {
                    return NotFound($"SHARC Not Found : ID = {sharcId}");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiSubscribe("{sharcId}/io/{sensorName}/values")]
        public async Task<ITrakHoundConsumer<TrakHoundApiResponse>> SubscribeSensorValue([FromRoute] string sharcId, [FromRoute] string sensorName, [FromQuery] int interval = 1000)
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var sharcObj = await Client.Entities.GetObject($"type=SHARC&meta@SharcId={sharcId}");
                if (sharcObj != null)
                {
                    var sensorPath = TrakHoundPath.Combine(sharcObj.Path, "io", sensorName);

                    var sensorObj = await Client.Entities.GetObject(sensorPath);
                    if (sensorObj != null)
                    {
                        var consumer = await Client.Entities.SubscribeObservations(sensorPath);
                        if (consumer != null)
                        {
                            var outputConsumer = new TrakHoundConsumer<TrakHoundObservation, TrakHoundApiResponse>(consumer);
                            outputConsumer.OnReceived = (observation) =>
                            {
                                var valueResponse = new ValueResponse();
                                valueResponse.Value = observation.Value.ToDouble();
                                valueResponse.Timestamp = observation.Timestamp;

                                return TrakHoundApiJsonResponse.Ok(valueResponse);
                            };
                            return outputConsumer;
                        }
                    }
                }
            }

            return null;
        }
    }
}
