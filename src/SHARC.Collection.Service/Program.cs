using TrakHound;
using TrakHound.Clients;
using TrakHound.Logging;
using TrakHound.Services;
using TrakHound.Volumes;

namespace SHARC.Collection
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // Set Function Parameters
            IDictionary<string, string> parameters = null;
            if (args != null && args.Length > 2) parameters = Json.Convert<IDictionary<string, string>>(args[2]);

            // Create new TrakHoundClient based on the Instance BaseUrl and Router
            var clientConfiguration = new TrakHoundHttpClientConfiguration("localhost", 8472);
            //var clientConfiguration = new TrakHoundHttpClientConfiguration("localhost", 8475);

            var client = new TrakHoundHttpClient(clientConfiguration, null);

            var volumePath = Path.Combine(AppContext.BaseDirectory, "volume");
            var volume = new TrakHoundVolume("volume", volumePath);

            var instanceInformation = await client.System.Instances.GetHostInformation();

            var serviceConfiguration = new TrakHoundServiceConfiguration();

            // Create a new instance of the Function
            var service = new Service(serviceConfiguration, client, volume);
            service.InstanceId = instanceInformation?.Id;
            service.LogReceived += ServiceLogReceived;

            await service.Start();

            Console.ReadLine();

            await service.Stop();

            Console.WriteLine("Exiting..");
            await Task.Delay(1000);

            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }

        private static void ServiceLogReceived(object sender, TrakHoundLogItem item)
        {
            Console.WriteLine($"{item.Timestamp.ToLocalDateTime()} : {item.LogLevel} : {item.Code} : {item.Message}");
        }
    }
}