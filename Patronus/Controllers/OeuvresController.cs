using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Patronus.Data;
using Patronus.Models;

namespace Patronus.Controllers
{
    public class OeuvresController : Controller
    {
        private PatronusDBEntities db = new PatronusDBEntities();

        // GET: Oeuvres
        public ActionResult Index()
        {
            var oeuvres = db.Oeuvres.Include(o => o.AspNetUser).Include(o => o.TypeOeuvre);
            return View(oeuvres.OrderBy(m=>m.Label).ToList());
        }

        // GET: Oeuvres/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oeuvre oeuvre = db.Oeuvres.Find(id);
            if (oeuvre == null)
            {
                return HttpNotFound();
            }
            return View(oeuvre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(long IdOeuvre, string Commentaire, string Note)
        {
            Oeuvre oeuvre = db.Oeuvres.Find(IdOeuvre);

            if (ModelState.IsValid)
            {
                NoteOeuvre noteOeuvre = new NoteOeuvre();
                noteOeuvre.IdUser = User.Identity.GetUserId();
                noteOeuvre.IdOeuvre = oeuvre.IdOeuvre;
                noteOeuvre.Note = Double.Parse(Note);
                noteOeuvre.Commentaire = Commentaire;
                db.NoteOeuvres.Add(noteOeuvre);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", "Oeuvres", routeValues: new { id = IdOeuvre });
                }
                catch(DbUpdateException e)
                {
                    db.NoteOeuvres.Remove(noteOeuvre);
                    db.Entry(noteOeuvre).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Oeuvres", routeValues: new { id = IdOeuvre });
                }  
                
            }
            return RedirectToAction("Details","Oeuvres", routeValues:new { id = IdOeuvre });
        }

        // GET: Oeuvres/Create
        public ActionResult Create()
        {
            ViewBag.IdTypeOeuvre = new SelectList(db.TypeOeuvres, "IdTypeOeuvre", "LabelType");
            return View();
        }

        // POST: Oeuvres/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOeuvre,IdTypeOeuvre,Label,Description,DateCreation,UrlImage")] Oeuvre oeuvre)
        {
            if (ModelState.IsValid)
            {
                oeuvre.DateAjout = DateTime.Now;
                oeuvre.IdContributeur = User.Identity.GetUserId();
                db.Oeuvres.Add(oeuvre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdContributeur = new SelectList(db.AspNetUsers, "Id", "Email", oeuvre.IdContributeur);
            ViewBag.IdTypeOeuvre = new SelectList(db.TypeOeuvres, "IdTypeOeuvre", "LabelType", oeuvre.IdTypeOeuvre);
            return View(oeuvre);
        }

        // GET: Oeuvres/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oeuvre oeuvre = db.Oeuvres.Find(id);
            if (oeuvre == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContributeur = new SelectList(db.AspNetUsers, "Id", "Email", oeuvre.IdContributeur);
            ViewBag.IdTypeOeuvre = new SelectList(db.TypeOeuvres, "IdTypeOeuvre", "LabelType", oeuvre.IdTypeOeuvre);
            return View(oeuvre);
        }

        // POST: Oeuvres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOeuvre,IdTypeOeuvre,Label,Description,DateCreation,DateAjout,IdContributeur,IdAPI,UrlImage")] Oeuvre oeuvre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oeuvre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContributeur = new SelectList(db.AspNetUsers, "Id", "Email", oeuvre.IdContributeur);
            ViewBag.IdTypeOeuvre = new SelectList(db.TypeOeuvres, "IdTypeOeuvre", "LabelType", oeuvre.IdTypeOeuvre);
            return View(oeuvre);
        }

        // GET: Oeuvres/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oeuvre oeuvre = db.Oeuvres.Find(id);
            if (oeuvre == null)
            {
                return HttpNotFound();
            }
            return View(oeuvre);
        }

        // POST: Oeuvres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Oeuvre oeuvre = db.Oeuvres.Find(id);
            db.Oeuvres.Remove(oeuvre);
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
        public ActionResult GetMean(long idOeuvre)
        {
           

            double mean = NoteData.GetMeanNote(idOeuvre);

            return Json(mean, JsonRequestBehavior.AllowGet);

        }
    }
}
