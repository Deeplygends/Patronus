using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Data
{
    public class NoteData
    {
        private PatronusDBEntities db = new PatronusDBEntities();
        public double GetMeanNote(long idOeuvre)
        {

            return Math.Round(db.NoteOeuvres.Where(x => x.IdOeuvre == idOeuvre).Average(x => x.Note),2);

        }
        
    }
}