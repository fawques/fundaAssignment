using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;

namespace FundaAssignment.Services
{
    public class MakelaarService : IMakelaarService
    {
        IFundaClient fundaClient;
        IMakelaarFactory makelaarFactory;

        public MakelaarService(IFundaClient fundaClient, IMakelaarFactory makelaarFactory)
        {
            this.fundaClient = fundaClient;
            this.makelaarFactory = makelaarFactory;
        }

        public async Task<IEnumerable<Makelaar>> GetTopMakelaars(int amount = 10)
        {
            var listings = await fundaClient.Query("koop", "amsterdam", null);
            var topMakelaars = listings.GroupBy(l => (l.MakelaarId, l.MakelaarNaam))
                .OrderByDescending(group => group.Count())
                .Select(m => makelaarFactory.CreateMakelaar(m.Key.MakelaarId, m.Key.MakelaarNaam))
                .Take(amount);

            return topMakelaars;
        }
    }
}