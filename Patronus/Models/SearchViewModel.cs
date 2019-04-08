using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public class SearchViewModel
    {
        public string SearchChain { get; set; }
        public List<Oeuvre> Oeuvres { get; set; }
    }
}