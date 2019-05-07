using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patronus.Controllers
{
    public class TypeOeuvreController : Controller
    {
        private static PatronusDBEntities db = new PatronusDBEntities();

        public static long GetTypeOeuvreId(string type)
        {
            TypeOeuvre l = db.TypeOeuvres.FirstOrDefault(m => m.LabelType == type);
            if (l != null)
            {
                return l.IdTypeOeuvre;
            }
            TypeOeuvre t = new TypeOeuvre()
            {
                LabelType = type
            };
            db.TypeOeuvres.Add(t);
            db.SaveChanges();

            l = db.TypeOeuvres.FirstOrDefault(m => m.LabelType == type);
            return l.IdTypeOeuvre;
        }
    }
}