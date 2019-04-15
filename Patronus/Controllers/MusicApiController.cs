using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Patronus.Controllers
{
    public class MusicApiController : ApiController
    {
        [HttpGet]
        public string Music(string title)
        {
            return new DeezerController().GetDeezerResult(title);

        }


    }
}
