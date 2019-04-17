using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public class SearchViewModel
    {
        public string SearchChainOeuvres { get; set; }

        public string SearchChainArtistes { get; set; }
        public List<Oeuvre> Oeuvres { get; set; }
    }
}