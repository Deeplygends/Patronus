using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patronus.Data
{
    public class NoteData
    {
        private static PatronusDBEntities db = new PatronusDBEntities();
        public static double GetMeanNote(long idOeuvre)
        {
            var tmp = db.NoteOeuvres.Where(x => x.IdOeuvre == idOeuvre);
            if (tmp.Any())
            {
                var mean = tmp.Average(x => x.Note);
                return Math.Round(mean, 2);
            }

            return 0;
        }
        
    }
}