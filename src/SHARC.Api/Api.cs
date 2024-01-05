using SHARC.TrakHound;
using TrakHound;
using TrakHound.Api;
using TrakHound.Clients;

namespace SHARC
{
    public partial class Api : TrakHoundApi
    {
        public Api(ITrakHoundApiConfiguration configuration, ITrakHoundHostClient client) : base(configuration, client) { }


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

        [TrakHoundApiSubscribe("status")]
        public async Task<ITrakHoundConsumer<ITrakHoundApiResponse>> SubscribeStatus([FromQuery] string path, [FromQuery] int interval = 1000)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var modelConsumer = await Client.Subscribe<TrakHoundSharcModel>(path, interval);
                if (modelConsumer != null)
                {
                    var consumer = new TrakHoundConsumer<IEnumerable<TrakHoundSharcModel>, ITrakHoundApiResponse>(modelConsumer);
                    consumer.InitialValue = TrakHoundApiJsonResponse.Ok(modelConsumer.InitialValue);
                    consumer.OnReceived = (models) =>
                    {
                        return TrakHoundApiJsonResponse.Ok(models);
                    };
                    return consumer;
                }
            }

            return null;
        }
    }
}
