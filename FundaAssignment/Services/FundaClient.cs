using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using FundaAssignment.Model.API;
using JackLeitch.RateGate;
using Newtonsoft.Json;

namespace FundaAssignment.Services
{
    public class FundaClient : IFundaClient
    {
        HttpClient httpClient;
        string baseAddress;

        public FundaClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            // This should come from IConfiguration
            baseAddress = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/";
        }

        public async Task<IEnumerable<Listing>> Query(string type, string location, string extra)
        {
            if (type != "koop" || location != "amsterdam")
            {
                throw new NotImplementedException();
            }

            string query = $"/{location}/";
            if (extra != null)
            {
                query += $"{extra}/";
            }

            var listings = await QueryAllPages(type, query);

            return listings;
        }

        async Task<IEnumerable<Listing>> QueryAllPages(string type, string query)
        {
            List<Listing> listings = new List<Listing>();

            int page = 1;
            AanbodDto responseModel;

            using (var rateGate = new RateGate(100, TimeSpan.FromMinutes(1)))
            {
                do
                {
                    rateGate.WaitToProceed();
                    responseModel = await QuerySinglePage(type, query, page);
                    listings.AddRange(responseModel.Objects);
                    page++;

                } while (page <= responseModel.Paging.AantalPaginas);
            }

            return listings;
        }

        async Task<AanbodDto> QuerySinglePage(string type, string query, int page)
        {
            Uri uri = BuildUri(type, query, page);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AanbodDto>(jsonString);
        }

        Uri BuildUri(string type, string query, int page)
        {
            UriBuilder uriBuilder = new UriBuilder(baseAddress);
            var queryBuilder = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryBuilder["type"] = type;
            queryBuilder["zo"] = query;
            queryBuilder["page"] = page.ToString();
            queryBuilder["pageSize"] = "25";

            uriBuilder.Query = queryBuilder.ToString();
            return uriBuilder.Uri;
        }
    }
}