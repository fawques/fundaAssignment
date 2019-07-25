using FundaAssignment.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IMainService
    {
        Task<Result> CalculateTopMakelaars();
    }
}
