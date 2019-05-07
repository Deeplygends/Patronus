using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public class JsonDeezer
    {
        public List<DeezerTrack> data { get; set; }
        public int count { get; set; }
    }
}