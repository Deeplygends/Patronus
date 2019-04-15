using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Patronus.Controllers
{
    public class MovieApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="several">True if the user wants several results.</param>
        /// <returns></returns>
        [HttpGet]
        public string Movie(string title, string type, int? year, string plot, string datatype, string callback, int? page, bool several)
        {
            return new OMDbController().GetOMDbResult(title, type, year, plot, datatype, callback, page, several);
        }


    }
}
