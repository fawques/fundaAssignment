using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FundaAssignment;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
public class SampleService : IHostedService
{
    IFundaClient fundaClient;

    public SampleService(IFundaClient fundaClient)
    {
        this.fundaClient = fundaClient;
    }

    public void Do()
    {
        System.Console.WriteLine("Do");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public async Task<IEnumerable<Markelaar>> GetTopMarkelaars()
    {
        return new List<Markelaar>();
    }
}