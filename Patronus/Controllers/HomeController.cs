using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patronus.Controllers
{
    public class HomeController : Controller
    {

        private PatronusDBEntities db = new PatronusDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MovieSearch(string oeuvres)
        {
            List<Oeuvre> o = new List<Oeuvre>();
            //parse list and search in bdd
            if (!String.IsNullOrEmpty(oeuvres))
            {
                String[] ids = oeuvres.Split(',');
                foreach(String id in ids)
                {
                    long i = long.Parse(id);
                    Oeuvre oe = db.Oeuvres.FirstOrDefault(m => m.IdOeuvre == i);
                    o.Add(oe);
                }
            }
            
            return View(o);
        }
    }
}