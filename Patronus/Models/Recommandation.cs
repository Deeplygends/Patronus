using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public class Recommandation
    {
        public List<Oeuvre> Movies { get; set; }
        public List<Oeuvre> Series { get; set; }
        public List<Oeuvre> Albums { get; set; }
        public List<Oeuvre> Tracks { get; set; }
        public List<Oeuvre> BestNote { get; set; }

    }
}