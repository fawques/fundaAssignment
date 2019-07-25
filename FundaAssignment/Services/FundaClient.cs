using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;

namespace FundaAssignment.Services
{
    public class FundaClient : IFundaClient
    {
        HttpClient httpClient;

        public FundaClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Listing>> Query(string type, string location, string extra)
        {
            if (type != "koop" || location != "amsterdam")
            {
                throw new System.NotImplementedException();
            }
            // Do query, as many as needed to get the data
            // Extract listings from data

            return new List<Listing>();
        }
    }
}