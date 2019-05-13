using Microsoft.AspNet.Identity;
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
            var model = GetRecommandation();
            ViewBag.BestNote = GetBestNote();
            return View(model);
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

        public List<Oeuvre> GetRecommandation()
        {
            Dictionary<Oeuvre, double> dicoOeuvre = new Dictionary<Oeuvre, double>();
            List<Oeuvre> oeuvres = new List<Oeuvre>();
            var id = User.Identity.GetUserId();
            if (id != null)
            {
                var user = db.AspNetUsers.Find(id);
                var userOeuvre = user.NoteOeuvres.OrderByDescending(x => x.Note).Select(x => x.Oeuvre).ToList();
                foreach (var oeuvre in userOeuvre.Take(5))
                {
                    var users = oeuvre.NoteOeuvres.Where(x => x.IdUser != id).OrderByDescending(x => x.Note)
                        .Select(x => x.AspNetUser);
                    foreach (var u in users)
                    {
                        var favoriUser = u.NoteOeuvres.OrderByDescending(x => x.Note).ToList();
                        foreach (var o in favoriUser.Take(5))
                        {
                            if (dicoOeuvre.Keys.Contains(o.Oeuvre))
                            {
                                dicoOeuvre[o.Oeuvre] += o.Note;
                            }
                            else
                            {
                                dicoOeuvre.Add(o.Oeuvre, o.Note);
                            }
                        }
                    }
                }


                return dicoOeuvre.OrderByDescending(x => x.Value).Select(x => x.Key).Take(5).ToList();
            }
            return new List<Oeuvre>();
        }

        public List<Oeuvre> GetBestNote()
        {
            var o = new List<Oeuvre>();

            o = db.Oeuvres.Where(x => x.NoteOeuvres.Count > 0).
            return o;
        }
    }
}