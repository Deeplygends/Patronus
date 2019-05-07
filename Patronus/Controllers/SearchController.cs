using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Patronus.Controllers
{

    public class SearchController : Controller
    {
        private PatronusDBEntities db = new PatronusDBEntities();

        public ActionResult SearchPattern()
        {
            SearchViewModel s = new SearchViewModel();
            s.Oeuvres = new List<Oeuvre>();
            return View(s);
        }
        [HttpPost]
        public ActionResult SearchPattern(SearchViewModel model)
        {
            //model.Oeuvres = db.Oeuvres.Where(x => x.Label.Contains(model.SearchChainOeuvres)).ToList();

            model.Oeuvres = db.Oeuvres.Where(x => x.Label.Contains(model.SearchChainOeuvres)).ToList();
            var data = DeezerController.GetDeezerResult(model.SearchChainOeuvres);
            var artisteList = db.Artistes
                .Where(x => x.Nom.ToUpper().Contains(model.SearchChainOeuvres.ToUpper()) || x.Prenom.ToUpper().Contains(model.SearchChainOeuvres.ToUpper()))
                .Select(x => x.IdArtiste)
                .ToList();

            var oeuvresByArtist = db.Oeuvres
                .Where(
                    y => db.Participes
                    .Where(x => artisteList.Contains(x.IdArtiste))
                    .Select(x => x.IdOeuvre)
                    .ToList()
                    .Contains(y.IdOeuvre)).ToList();

            model.Oeuvres.AddRange(oeuvresByArtist);
            return View(model);
        }

        

    }
}