using System;
using System.Collections.Generic;
using System.Text;

namespace FundaAssignment.Model
{
    public class Listing
    {
        public int GlobalId { get; set; }
        public Guid Id { get; set; }
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
    }
}
