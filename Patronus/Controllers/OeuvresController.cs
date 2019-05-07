using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            return View(oeuvres.ToList());
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
    }
}
