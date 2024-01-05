using SHARC.TrakHound;
using TrakHound;
using TrakHound.Api;
using TrakHound.Clients;
using TrakHound.Entities;
using TrakHound.Requests;

namespace SHARC
{
    public partial class Api : TrakHoundApi
    {
        public Api(ITrakHoundApiConfiguration configuration, ITrakHoundHostClient client) : base(configuration, client) { }


        [TrakHoundApiQuery]
        public async Task<ITrakHoundApiResponse> GetList()
        {
            var objs = await Client.GetObjects("sharc/*");
            if (objs != null)
            {
                return Ok(objs.Select(o => o.Name));
            }
            else
            {
                return NotFound("No SHARCs Found (sharc/*)");
            }
        }

        [TrakHoundApiQuery("{sharcId}")]
        public async Task<ITrakHoundApiResponse> GetInformation([FromRoute] string sharcId)
        {
            if (!string.IsNullOrEmpty(sharcId))
            {
                var path = TrakHoundPath.Combine("sharc", sharcId);

                var model = await Client.GetByPath<TrakHoundSharcModel>(path);
                if (model != null)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound("Station Not Found");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery("{sharcId}/io/{sensorName}")]
        public async Task<ITrakHoundApiResponse> QuerySensorValues(
            [FromRoute] string sharcId, 
            [FromRoute] string sensorName,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 1000
            )
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var path = TrakHoundPath.Combine("sharc", sharcId, "io", sensorName);

                var observations = await Client.GetObservationsByPath(path, from, to, skip, take);
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
                return BadRequest();
            }
        }

        [TrakHoundApiSubscribe("{sharcId}/io/{sensorName}")]
        public async Task<ITrakHoundConsumer<ITrakHoundApiResponse>> SubscribeSensorValue([FromRoute] string sharcId, [FromRoute] string sensorName, [FromQuery] int interval = 1000)
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(sensorName))
            {
                var path = TrakHoundPath.Combine("sharc", sharcId, "io", sensorName);
                var obj = await Client.GetObject(path);
                if (obj != null)
                {
                    var entityConsumer = await Client.Entities.Objects.Observations.SubscribeByObjectUuid(obj.Uuid);
                    if (entityConsumer != null)
                    {
                        var outputConsumer = new TrakHoundConsumer<ITrakHoundObjectObservationEntity, ITrakHoundApiResponse>(entityConsumer);
                        outputConsumer.OnReceived = (entity) =>
                        {
                            var observation = new TrakHoundObservation();
                            observation.Value = entity.Value;
                            observation.Timestamp = entity.Timestamp.ToDateTime();

                            return TrakHoundApiJsonResponse.Ok(observation);
                        };
                        return outputConsumer;
                    }
                }
            }

            return null;
        }
    }
}
