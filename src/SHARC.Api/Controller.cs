using TrakHound;
using TrakHound.Api;
using TrakHound.Requests;

namespace SHARC.Api
{
    public partial class Controller : TrakHoundApiController
    {
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
                        var observations = (await Client.Entities.GetObservationValues(sensorPath, from, to, skip, take, (SortOrder)sortOrder))?.GetValueOrDefault(sensorPath);
                        if (!observations.IsNullOrEmpty())
                        {
                            return Ok(observations);
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

        [TrakHoundApiSubscribe("{sharcId}/io/{sensorName}")]
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
                            var outputConsumer = new TrakHoundConsumer<IEnumerable<TrakHoundObservation>, TrakHoundApiResponse>(consumer);
                            outputConsumer.OnReceived = (observations) =>
                            {
                                var observationValues = new List<TrakHoundObservationValue>();
                                foreach (var observation in observations)
                                {
                                    var observationValue = new TrakHoundObservationValue();
                                    observationValue.Value = observation.Value;
                                    observationValue.Timestamp = observation.Timestamp;
                                    observationValues.Add(observationValue);
                                }

                                return TrakHoundApiJsonResponse.Ok(observationValues);
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
