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
        public double GetMeanNote(Oeuvre oeuvre)
        {

            return db.NoteOeuvres.Where(x => x.IdOeuvre == oeuvre.IdOeuvre).Average(x => x.Note);

        }
        
    }
}