using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundaAssignment.Services
{
    public class MainService : IMainService
    {
        IFundaClient fundaClient;
        IMakelaarService makelaarService;
        public MainService(IFundaClient fundaClient, IMakelaarService makelaarService)
        {
            this.makelaarService = makelaarService;
            this.fundaClient = fundaClient;
        }

        public async Task<Result> CalculateTopMakelaars()
        {
            var listings = await fundaClient.Query("koop", "amsterdam", null);
            var topAmsterdam = makelaarService.GetTopMakelaars(listings);

            var listingsWithTuin = await fundaClient.Query("koop", "amsterdam", "tuin");
            var topAmsterdamTuin = makelaarService.GetTopMakelaars(listingsWithTuin);

            return new Result { TopAmsterdam = topAmsterdam, TopAmsterdamTuin = topAmsterdamTuin };
        }
    }
}
