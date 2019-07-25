using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;

namespace FundaAssignment.Services
{
    public class MakelaarService : IMakelaarService
    {
        IMakelaarFactory makelaarFactory;

        public MakelaarService(IMakelaarFactory makelaarFactory)
        {
            this.makelaarFactory = makelaarFactory;
        }

        public IEnumerable<Makelaar> GetTopMakelaars(IEnumerable<Listing> listings, int amount = 10)
        {
            var topMakelaars = listings.GroupBy(l => (l.MakelaarId, l.MakelaarNaam))
                .OrderByDescending(group => group.Count())
                .Select(m => makelaarFactory.CreateMakelaar(m.Key.MakelaarId, m.Key.MakelaarNaam))
                .Take(amount);

            return topMakelaars;
        }
    }
}