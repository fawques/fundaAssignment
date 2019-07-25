using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundaAssignment.Services
{
    public class MakelaarFactory : IMakelaarFactory
    {
        public Makelaar CreateMakelaar(int id, string name)
        {
            return new Makelaar
            {
                MakelaarId = id,
                MakelaarNaam = name,
            };
        }
    }
}
