using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Patronus.Controllers;
using Patronus.Models;

namespace Patronus.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        [TestMethod]
        public void TestSearchFullName()
        {
            SearchController controller = new SearchController();

            ViewResult result = controller.Search("Tarzan") as ViewResult;
            Assert.IsNotNull(result);
      
            SearchViewModel model = result.Model as SearchViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Oeuvres.First().Label, "Tarzan");

            Assert.AreEqual(model.Oeuvres.First().TypeOeuvre.LabelType, "Cinema");

        }

        [TestMethod]
        public void TestPartialName()
        {
            SearchController controller = new SearchController();

            ViewResult result = controller.Search("Tarz") as ViewResult;
            Assert.IsNotNull(result);

            SearchViewModel model = result.Model as SearchViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Oeuvres.Any(),true);
            

        }
    }
}
