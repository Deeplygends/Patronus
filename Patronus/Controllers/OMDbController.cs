using Patronus.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Patronus.Controllers
{
    public class OMDbController : Controller
    {
        private PatronusDBEntities db = new PatronusDBEntities();

        public string GetOMDbResult(string title, string type, int? year, string plot, string datatype, string callback, int? page, bool several)
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
            if (type != "" && (type == "movie" || type == "series" || type == "episode"))
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
            using (Stream stream = WebResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }

        /// <summary>
        /// Parses the json object for the Movie (json object obtained after a single request) to an Oeuvre object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Oeuvre ParseOeuvreSingle(string json, string type)
        {
            var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);
            Oeuvre movie = new Oeuvre()
            {
                IdTypeOeuvre = db.TypeOeuvres.FirstOrDefault(m => m.LabelType == type).IdTypeOeuvre,
                Label = JSONObj["Title"],
                Description = JSONObj["Plot"],
                DateCreation = DateTime.ParseExact(JSONObj["Released"], "dd MMM yyyy", CultureInfo.InvariantCulture),
                IdAPI = JSONObj["imdbID"]
                //add the poster url
            };
            if (ModelState.IsValid)
            {
                db.Oeuvres.Add(movie);
                db.SaveChanges();
            }
            else
            {
                //
            }
            movie = db.Oeuvres.FirstOrDefault(m => m.IdAPI == JSONObj["imdbID"]);
            //Get the different genres
            String[] genres = JSONObj["Genre"].Split(',');
            foreach (String g in genres)
            {
                movie.Themes.Add(db.Themes.Find(ThemeController.GetThemeId(g)));
            }
            //Director
            String[] directors = JSONObj["Director"].Split(',');
            foreach (String d in directors)
            {
                Participe participe = new Participe()
                {
                    IdArtiste = ArtistController.GetArtistId(d),
                    IdOeuvre = movie.IdOeuvre,
                    role = "director"
                };
                if (ModelState.IsValid)
                {
                    db.Participes.Add(participe);
                    db.SaveChanges();
                }
                else
                {
                    //
                }
                movie.Participes.Add(participe);
            }
            //Writer
            String[] writers = JSONObj["Writer"].Split(',');
            foreach (String w in writers)
            {
                Participe participe = new Participe()
                {
                    IdArtiste = ArtistController.GetArtistId(w),
                    IdOeuvre = movie.IdOeuvre,
                    role = "writer"
                };
                if (ModelState.IsValid)
                {
                    db.Participes.Add(participe);
                    db.SaveChanges();
                }
                else
                {
                    //
                }
                movie.Participes.Add(participe);
            }

            //Actors
            String[] actors = JSONObj["Actors"].Split(',');
            foreach (String a in actors)
            {
                Participe participe = new Participe()
                {
                    IdArtiste = ArtistController.GetArtistId(a),
                    IdOeuvre = movie.IdOeuvre,
                    role = "actor"
                };
                if (ModelState.IsValid)
                {
                    db.Participes.Add(participe);
                    db.SaveChanges();
                }
                else
                {
                    //
                }
                movie.Participes.Add(participe);
            }

            return movie;
        }

    }
}