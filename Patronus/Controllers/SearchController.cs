using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Patronus.Models;

namespace Patronus.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Search(String name)
        {
            return View();
        }
    }
}