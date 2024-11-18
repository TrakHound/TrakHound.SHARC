using MQTTnet;
using SHARC.Mqtt;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrakHound;
using TrakHound.Api;

namespace SHARC.Collection
{
    public class Controller : TrakHoundApiController
    {
        private const string _fileExtension = ".json";
        private readonly MqttFactory _mqttFactory;


        public Controller()
        {
            _mqttFactory = new MqttFactory();
        }


        [TrakHoundApiQuery("discover")]
        public async Task<TrakHoundApiResponse> Discover([FromQuery] string server, [FromQuery] int port = 1883, [FromQuery] int timeout = 5000)
        {
            if (!string.IsNullOrEmpty(server))
            {
                var sharcClient = new SharcMqttDiscoveryClient(server, port);
                var sharcs = await sharcClient.Run(timeout);
                if (!sharcs.IsNullOrEmpty())
                {
                    return Ok(sharcs);
                }
                else
                {
                    return NotFound("No SHARCs Found");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [TrakHoundApiQuery]
        public async Task<TrakHoundApiResponse> Get()
        {
            var files = await Volume.ListFiles();
            if (!files.IsNullOrEmpty())
            {
                var configurations = new List<SensorConfiguration>();

                foreach (var file in files)
                {
                    var configuration = await ReadConfiguration(file);
                    if (configuration != null)
                    {
                        configurations.Add(configuration);
                    }
                }

                return Ok(configurations);
            }
            else
            {
                return NotFound();
            }
        }

        [TrakHoundApiQuery("{sharcId}")]
        public async Task<TrakHoundApiResponse> Get([FromRoute] string sharcId)
        {
            var path = $"{sharcId}{_fileExtension}";
            var configuration = await ReadConfiguration(path);
            if (configuration != null)
            {
                return Ok(configuration);
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<SensorConfiguration> ReadConfiguration(string path)
        {
            var json = await Volume.ReadString(path);
            if (!string.IsNullOrEmpty(json))
            {
                var configuration = Json.Convert<SensorConfiguration>(json);
                if (configuration != null)
                {
                    return configuration;
                }
            }

            return null;
        }

        [TrakHoundApiPublish("{sharcId}/enable")]
        public async Task<TrakHoundApiResponse> Enable([FromRoute] string sharcId)
        {
            var path = $"{sharcId}{_fileExtension}";
            var configuration = await ReadConfiguration(path);
            if (configuration != null)
            {
                configuration.Enabled = true;

                if (await Volume.WriteJson(path, configuration))
                {
                    return Ok(configuration);
                }
                else
                {
                    return InternalError();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [TrakHoundApiPublish("{sharcId}/disable")]
        public async Task<TrakHoundApiResponse> Disable([FromRoute] string sharcId)
        {
            var path = $"{sharcId}{_fileExtension}";
            var configuration = await ReadConfiguration(path);
            if (configuration != null)
            {
                configuration.Enabled = false;

                if (await Volume.WriteJson(path, configuration))
                {
                    return Ok(configuration);
                }
                else
                {
                    return InternalError();
                }
            }
            else
            {
                return NotFound();
            }
        }


        [TrakHoundApiPublish("{sharcId}")]
        public async Task<TrakHoundApiResponse> Publish(
            [FromRoute] string sharcId,
            [FromQuery] string server, 
            [FromQuery] int port = 1883,
            [FromQuery] bool enabled = false,
            [FromQuery] string basePath = null,
            [FromQuery] string name = null,
            [FromQuery] string description = null
            )
        {
            if (!string.IsNullOrEmpty(sharcId) && !string.IsNullOrEmpty(server))
            {
                var configuration = new SensorConfiguration();
                configuration.Id = sharcId;
                configuration.Enabled = enabled;
                configuration.Server = server;
                configuration.Port = port;
                configuration.BasePath = basePath;
                configuration.Name = name;
                configuration.Description = description;

                if (await Volume.WriteJson($"{configuration.Id}{_fileExtension}", configuration))
                {
                    return Created(configuration.ToJson(true));
                }
                else
                {
                    return InternalError();
                }
            }

            return BadRequest();
        }

        [TrakHoundApiDelete("{sharcId}")]
        public async Task<TrakHoundApiResponse> Delete([FromRoute] string sharcId)
        {
            var path = $"{sharcId}{_fileExtension}";
            if (await Volume.Delete(path))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
