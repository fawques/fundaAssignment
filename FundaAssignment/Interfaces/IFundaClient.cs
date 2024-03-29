﻿using FundaAssignment.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaAssignment.Interfaces
{
    public interface IFundaClient
    {
        Task<IEnumerable<Listing>> Query(string type, string location, string extra);
    }
}