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
            var tmp = db.NoteOeuvres.Where(x => x.IdOeuvre == idOeuvre);
            if (tmp.Count() > 0)
            {
                var mean = tmp.Average(x => x.Note);
                return Math.Round(mean, 2);
            }

            return 0;
        }

        public double GetMeanNoteArtiste(string idArtiste)
        {
            var tmp = db.NoteArtistes.Where(x => x.IdArtiste == idArtiste);
            if (tmp.Count() > 0)
            {
                var mean = tmp.Average(x => x.Note);
                return Math.Round(mean, 2);
            }

            return 0;
        }

    }
}