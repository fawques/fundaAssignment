using System;
using System.Collections.Generic;
using System.Text;

namespace FundaAssignment.Model.API
{
    public class AanbodDto
    {
        public class PagingDto
        {
            public int AantalPaginas { get; set; }
            public int HuidigePagina { get; set; }
            public string VolgendeUrl { get; set; }
            public string VorigeUrl { get; set; }
        }

        public int TotaalAantalObjecten { get; set; }

        public PagingDto Paging { get; set; }

        public List<Listing> Objects { get; set; }
    }
}
