using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FundaAssignment
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddHttpClient<IFundaClient, FundaClient>(client =>
                        client.BaseAddress = new Uri("https://funda.nl"));
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();

                })
                .Build();

            using (host)
            {
                await host.StartAsync();

                Console.WriteLine("Press Ctrl + C to exit");
                await host.WaitForShutdownAsync();
            }
        }
    }
}
