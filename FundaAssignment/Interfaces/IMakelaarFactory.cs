using FundaAssignment.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundaAssignment.Interfaces
{
    public interface IMakelaarFactory
    {
        Makelaar CreateMakelaar(int id, string name, int amount);
    }
}
