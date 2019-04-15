using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patronus.Controllers
{
    public class ThemeController : Controller
    {
        private static PatronusDBEntities db = new PatronusDBEntities();

        public static long GetThemeId(string label)
        {
            Theme l = db.Themes.FirstOrDefault(m => m.Label == label);
            if (l != null)
            {
                return l.IdTheme;
            }
            Theme theme = new Theme()
            {
                Label = label
            };
            db.Themes.Add(theme);
            db.SaveChanges();

            l = db.Themes.FirstOrDefault(m => m.Label == label);
            return l.IdTheme;
        }
    }
}