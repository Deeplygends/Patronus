using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Models
{
    public class DeezerTrack
    {
       

        public int id { get; set; }
        public string title { get; set; }
        public double duration { get; set; }

        public string picture_xl { get; set; }

        public DeezerArtiste artist { get; set; }
        public DeezerAlbum album { get; set; }
    }
}