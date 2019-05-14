using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Patronus.Data;

namespace Patronus.Models
{
    public partial class Oeuvre
    {
        public double mean
        {
            get { return NoteData.GetMeanNote(IdOeuvre); }
        }

        public string Meandeci
        {
            get { return ((mean % 1)*15).ToString() + "px"; }
        }
    }
}