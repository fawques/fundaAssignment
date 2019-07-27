using FundaAssignment.Interfaces;
using FundaAssignment.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net;
using System.Net.Http;
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
                    services.AddTransient<IMakelaarService, MakelaarService>();
                    services.AddTransient<IMakelaarFactory, MakelaarFactory>();

                    var retryPolicy =
                    services.AddHttpClient<IFundaClient, FundaClient>()
                         .AddPolicyHandler((svc, request) => Policy<HttpResponseMessage>
                            .Handle<HttpRequestException>().Or<TaskCanceledException>()
                            .OrResult(response => response.StatusCode == HttpStatusCode.Unauthorized)
                            .WaitAndRetryAsync(new[]
                            {
                                // Rate limited, give the API some space
                                TimeSpan.FromSeconds(5),
                                TimeSpan.FromSeconds(10),
                                TimeSpan.FromSeconds(15),
                                TimeSpan.FromSeconds(30)
                                // If after a whole minute it still returns 401, this is not a rate limit issue and it should fail
                            }, onRetry: (outcome, timespan, retryAttempt, context) =>
                                {
                                    svc.GetService<ILogger<FundaClient>>()
                                        .LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                                })
                    );
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();

                })
                .Build();

            using (host)
            {
                await host.StartAsync();
                var service = host.Services.GetRequiredService<IMakelaarService>();
                var fundaClient = host.Services.GetRequiredService<IFundaClient>();

                var listings = await fundaClient.Query("koop", "amsterdam", null);
                service.GetTopMakelaars(listings);

                Console.WriteLine("Press Ctrl + C to exit");
                await host.WaitForShutdownAsync();
            }
        }
    }
}
