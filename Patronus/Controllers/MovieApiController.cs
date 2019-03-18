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
        public string Movie(string title, bool several)
        {
            return GetOMDbResult(title, "", null, "", "", "", null, several);
        }

        public string GetOMDbResult(string title, string type, int? year, string plot, string datatype, string callback,int? page, bool several)
        {
            string omdbkey = System.Configuration.ConfigurationManager.AppSettings["OMDbApiKey"];
            string parameters = "apikey=" + omdbkey;
            if (several)
            {
                parameters += "&s=" + title;
            }
            else
            {
                parameters += "&t=" + title;
            }
            if(type != "" && (type == "movie" || type == "series" || type == "episode"))
            {
                parameters += "&type=" + type;
            }
            if (year != null)
            {
                parameters += "&y=" + year;
            }
            if (plot != "" && !several && (plot == "full" || plot == "short"))
            {
                parameters += "&plot=" + plot;
            }
            if (datatype != "" && (datatype == "json" || datatype == "xml"))
            {
                parameters += "&r=" + datatype;
            }
            if (callback != "")
            {
                parameters += "&callback=" + callback;
            }
            if (page != null && several)
            {
                parameters += "&page=" + page;
            }
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://www.omdbapi.com/?" + parameters));

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
