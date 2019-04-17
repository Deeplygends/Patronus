using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Patronus.Controllers
{
    public class DeezerController : Controller
    {
        public string GetDeezerResult(string title)
        {
            string parameters = "";

            if (!String.IsNullOrEmpty(title))
            {
                parameters += "track:\"" + title + "\"";
            }
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.deezer.com/search?q=" + parameters));

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