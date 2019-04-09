using Patronus.Models;
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
            
            model.Oeuvres = db.Oeuvres.Where(x => x.Label.Contains(model.SearchChain)).ToList();
            return View(model);
        }


    }
}