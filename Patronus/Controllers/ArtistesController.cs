using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Patronus.Data;
using Patronus.Models;

namespace Patronus.Controllers
{
    public class ArtistesController : Controller
    {
        private static  PatronusDBEntities dbs = new PatronusDBEntities();
        private PatronusDBEntities db = new PatronusDBEntities();

        // GET: Artistes
        public ActionResult Index()
        {
            return View(db.Artistes.ToList());
        }

        // GET: Artistes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artistes.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }
            List<long> idoeuvres = db.Participes.Where(m => m.IdArtiste == id).Select(m=>m.IdOeuvre).ToList();
            artiste.OeuvresRealisees = db.Oeuvres.Where(m => idoeuvres.Contains(m.IdOeuvre)).ToList();
            return View(artiste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string IdArtiste, string Commentaire, string Note)
        {
            Artiste artiste = db.Artistes.Find(IdArtiste);

            if (ModelState.IsValid)
            {
                NoteArtiste noteArtiste = new NoteArtiste();
                noteArtiste.IdUser = User.Identity.GetUserId();
                noteArtiste.IdArtiste = artiste.IdArtiste;
                noteArtiste.Note = Double.Parse(Note);
                noteArtiste.Commentaire = Commentaire;
                db.NoteArtistes.Add(noteArtiste);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", "Artistes", routeValues: new { id = IdArtiste });
                }
                catch (DbUpdateException e)
                {
                    db.NoteArtistes.Remove(noteArtiste);
                    db.Entry(noteArtiste).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Artistes", routeValues: new { id = IdArtiste });
                }

            }
            return RedirectToAction("Details", "Artistes", routeValues: new { id = IdArtiste });
        }

        // GET: Artistes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artistes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdArtiste,Nom")] Artiste artiste)
        {
            artiste.IdArtiste = artiste.Nom;
            if (ModelState.IsValid)
            {
                db.Artistes.Add(artiste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artiste);
        }

        // GET: Artistes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artistes.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }
            return View(artiste);
        }

        // POST: Artistes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdArtiste,Nom,Prenom,DateNaissance,Description")] Artiste artiste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artiste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artiste);
        }

        // GET: Artistes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artiste artiste = db.Artistes.Find(id);
            if (artiste == null)
            {
                return HttpNotFound();
            }
            return View(artiste);
        }

        // POST: Artistes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Artiste artiste = db.Artistes.Find(id);
            db.Artistes.Remove(artiste);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult GetMean(string idArtiste)
        {
            var noteData = new NoteData();

            double mean = noteData.GetMeanNoteArtiste(idArtiste);

            return Json(mean, JsonRequestBehavior.AllowGet);

        }

        public static string GetArtistId(string name)
        {
            Artiste l = dbs.Artistes.FirstOrDefault(m => m.Nom == name);
            if (l != null)
            {
                return l.IdArtiste;
            }
            Artiste artiste = new Artiste()
            {
                IdArtiste = name,
                Nom = name,
                Prenom = ""
            };
            dbs.Artistes.Add(artiste);
            dbs.Entry(artiste).State = System.Data.Entity.EntityState.Added;
            dbs.SaveChanges();


            l = dbs.Artistes.FirstOrDefault(m => m.Nom == name);
            return l.IdArtiste;
        }
    }
}
