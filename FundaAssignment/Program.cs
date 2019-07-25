using FundaAssignment.Interfaces;
using FundaAssignment.Services;
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
                var service = host.Services.GetRequiredService<MakelaarService>();
                var fundaClient = host.Services.GetRequiredService<IFundaClient>();

                var listings = await fundaClient.Query("koop", "amsterdam", null);
                service.GetTopMakelaars(listings);

                Console.WriteLine("Press Ctrl + C to exit");
                await host.WaitForShutdownAsync();
            }
        }
    }
}
