using FundaAssignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IMakelaarService
    {
        IEnumerable<Makelaar> GetTopMakelaars(IEnumerable<Listing> listings, int amount = 10);
    }
}