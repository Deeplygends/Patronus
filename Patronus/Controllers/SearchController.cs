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
            s.Artistes = new List<Artiste>();
            return View(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filter">Valeurs : all, movie, music, artist</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchPattern(SearchViewModel model, string filter)
        {
            model.Oeuvres = new List<Oeuvre>();
            model.Artistes = new List<Artiste>();
            //maj avec api
            if (filter == "movie" || filter == "all")
            {
                List<Oeuvre> mo = new OMDbController().GetAllMovies(model.SearchChainOeuvres);
            }

            if(filter == "music" || filter == "all")
            {
                //api deezer
            }
            //requete de toutes les oeuvres
            if (filter != "artist")
            {
                model.Oeuvres = db.Oeuvres.Where(x => x.Label.Contains(model.SearchChainOeuvres)).ToList();

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

            }
            //filtre sur les oeuvres
            if (filter == "movie")
            {
                model.Oeuvres = model.Oeuvres.Where(m => m.TypeOeuvre.LabelType == "movie" || m.TypeOeuvre.LabelType == "episode" || m.TypeOeuvre.LabelType == "series").ToList();
            }

            if (filter == "music")
            {
                model.Oeuvres = model.Oeuvres.Where(m => m.TypeOeuvre.LabelType == "music" || m.TypeOeuvre.LabelType == "track" || m.TypeOeuvre.LabelType == "album").ToList();
            }

            //model.Oeuvres = db.Oeuvres.Where(x => x.Label.Contains(model.SearchChainOeuvres)).ToList();
            if (filter == "artist" || filter == "all")
            {
                //uniquement une recherche en BDD, pas de maj avec les apis
                model.Artistes = db.Artistes.Where(m => String.Concat(m.Nom.ToLower(), " " + m.Prenom.ToLower()).Contains(model.SearchChainOeuvres.ToLower()) || String.Concat(m.Prenom.ToLower(), " " + m.Nom.ToLower()).Contains(model.SearchChainOeuvres.ToLower())).ToList();
            }

            return View(model);
        }


    }
}