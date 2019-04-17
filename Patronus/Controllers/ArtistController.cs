using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patronus.Controllers
{
    public class ArtistController : Controller
    {
        private static PatronusDBEntities db = new PatronusDBEntities();

        public static string GetArtistId(string name)
        {
            Artiste l = db.Artistes.FirstOrDefault(m => m.Nom == name);
            if (l != null)
            {
                return l.IdArtiste;
            }
            Artiste artiste = new Artiste()
            {
                Nom = name
            };
            db.Artistes.Add(artiste);
            db.SaveChanges();

            l = db.Artistes.FirstOrDefault(m => m.Nom == name);
            return l.IdArtiste;
        }
    }
}