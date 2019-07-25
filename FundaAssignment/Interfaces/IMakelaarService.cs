using FundaAssignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IMakelaarService
    {
        Task<IEnumerable<Makelaar>> GetTopMakelaars(int amount = 10);
    }
}