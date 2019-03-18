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
        [HttpGet]
        public string Movie(string title)
        {
            return GetOMDbResult("", title, "", null, "", "", "");
        }

        public string GetOMDbResult(string idIMDb, string title, string type, int? year, string plot, string datatype, string callback)
        {
            string omdbkey = System.Configuration.ConfigurationManager.AppSettings["OMDbApiKey"];
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://www.omdbapi.com/?apikey=" + omdbkey + "&s=" + title));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
      }
}
