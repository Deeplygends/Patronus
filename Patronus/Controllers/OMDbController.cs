using Patronus.Models;
using System;
using System.Collections;
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

        // <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="several">True if the user wants several results.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Movie(string title, string type, int? year, string plot, string datatype, string callback, int? page, bool several)
        {
            string movies = new OMDbController().GetOMDbResult(title, type, year, plot, datatype, callback, page, several);
            List<long> oeuvres = new List<long>();
            if (!several)
            {
                oeuvres.Add(ParseOeuvreSingle(movies, type).IdOeuvre);
            }
            else
            {
                var jsonObj = new JavaScriptSerializer().Deserialize<Dictionary<string, Object>>(movies);
                if (jsonObj.Count == 2)
                {
                    //error
                }
                else
                {
                    ArrayList list = (ArrayList)jsonObj["Search"];
                    foreach(var obj in list)
                    {
                        var di = (Dictionary<string, Object>)obj;
                        string id = (string)di["imdbID"];
                        string mo = new OMDbController().GetOMDbResultByID(id, type, year, plot, datatype, callback, page);
                        oeuvres.Add(ParseOeuvreSingle(mo, type).IdOeuvre);
                    }
                }
            }
            return RedirectToAction("MovieSearch", "Home", new { oeuvres = String.Join(",",oeuvres) });
        }

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

        public string GetOMDbResultByID(string id, string type, int? year, string plot, string datatype, string callback, int? page)
        {
            string omdbkey = System.Configuration.ConfigurationManager.AppSettings["OMDbApiKey"];
            string parameters = "apikey=" + omdbkey;
            
                parameters += "&i=" + id;
            
            if (type != "" && (type == "movie" || type == "series" || type == "episode"))
            {
                parameters += "&type=" + type;
            }
            if (year != null)
            {
                parameters += "&y=" + year;
            }
            if (plot != "" && (plot == "full" || plot == "short"))
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


        public List<Oeuvre> GetAllMovies(string title)
        {
            string movie = GetOMDbResult(title, "movie", null, "short", "json", null, null, true);
            string ep = GetOMDbResult(title, "episode", null, "short", "json", null, null, true);
            string ser = GetOMDbResult(title, "series", null, "short", "json", null, null, true);

            List<long> idmovie = new List<long>();
            List<long> idep = new List<long>();
            List<long> idser = new List<long>();

            var jsonObj = new JavaScriptSerializer().Deserialize<Dictionary<string, Object>>(movie);
            if (jsonObj.Count == 2)
            {
                //error
            }
            else
            {
                ArrayList list = (ArrayList)jsonObj["Search"];
                foreach (var obj in list)
                {
                    var di = (Dictionary<string, Object>)obj;
                    string id = (string)di["imdbID"];
                    string mo = new OMDbController().GetOMDbResultByID(id, "movie", null, "short", "json", null, null);
                    idmovie.Add(ParseOeuvreSingle(mo, "movie").IdOeuvre);
                }
            }

            var jsonObj1 = new JavaScriptSerializer().Deserialize<Dictionary<string, Object>>(ep);
            if (jsonObj1.Count == 2)
            {
                //error
            }
            else
            {
                ArrayList list = (ArrayList)jsonObj1["Search"];
                foreach (var obj in list)
                {
                    var di = (Dictionary<string, Object>)obj;
                    string id = (string)di["imdbID"];
                    string mo = new OMDbController().GetOMDbResultByID(id, "episode", null, "short", "json", null, null);
                    idep.Add(ParseOeuvreSingle(mo, "episode").IdOeuvre);
                }
            }

            var jsonObj2 = new JavaScriptSerializer().Deserialize<Dictionary<string, Object>>(ser);
            if (jsonObj2.Count == 2)
            {
                //error
            }
            else
            {
                ArrayList list = (ArrayList)jsonObj2["Search"];
                foreach (var obj in list)
                {
                    var di = (Dictionary<string, Object>)obj;
                    string id = (string)di["imdbID"];
                    string mo = new OMDbController().GetOMDbResultByID(id, "series", null, "short", "json", null, null);
                    idser.Add(ParseOeuvreSingle(mo, "series").IdOeuvre);
                }
            }

            List<Oeuvre> o = new List<Oeuvre>();
            //parse list and search in bdd
            foreach (long id in idmovie)
            {
                Oeuvre oe = db.Oeuvres.FirstOrDefault(m => m.IdOeuvre == id);
                o.Add(oe);
            }
            foreach (long id in idep)
            {
                Oeuvre oe = db.Oeuvres.FirstOrDefault(m => m.IdOeuvre == id);
                o.Add(oe);
            }
            foreach (long id in idser)
            {
                Oeuvre oe = db.Oeuvres.FirstOrDefault(m => m.IdOeuvre == id);
                o.Add(oe);
            }

            return o;
        }
        /// <summary>
        /// Parses the json object for the Movie (json object obtained after a single request) to an Oeuvre object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Oeuvre ParseOeuvreSingle(string json, string type)
        {
            var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(CleanJson(json));

            //create or get id type oeuvre 

            string idapi = JSONObj["imdbID"];
            Oeuvre movie = db.Oeuvres.FirstOrDefault(m => m.IdAPI == idapi);
            if(movie == null)
            {
                movie = new Oeuvre()
                {
                    IdTypeOeuvre = TypeOeuvreController.GetTypeOeuvreId(type),
                    Label = JSONObj["Title"],
                    Description = JSONObj["Plot"],
                    IdAPI = JSONObj["imdbID"],
                    UrlImage = JSONObj["Poster"],
                    IdContributeur = "IDapiOMDb",
                    DateAjout = DateTime.Now
                };
                if(JSONObj["Released"] != "N/A")
                {
                    movie.DateCreation = DateTime.ParseExact(JSONObj["Released"], "dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                    
                if (ModelState.IsValid)
                {
                    db.Oeuvres.Add(movie);
                    db.SaveChanges();
                }
                else
                {
                    //
                }
            }

           
            movie = db.Oeuvres.FirstOrDefault(m => m.IdAPI == idapi);
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
                var idartist = ArtistesController.GetArtistId(d);
                if(db.Participes.FirstOrDefault(m=>m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre) == null)
                {

                    Participe participe = new Participe()
                    {
                        IdArtiste = ArtistesController.GetArtistId(d),
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
                    db.SaveChanges();
                }
                else
                {
                    movie.Participes.Add(db.Participes.FirstOrDefault(m => m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre));
                    db.SaveChanges();
                }

            }
            //Writer
            String[] writers = JSONObj["Writer"].Split(',');
            foreach (String w in writers)
            {
                var idartist = ArtistesController.GetArtistId(w);
                if (db.Participes.FirstOrDefault(m => m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre) == null)
                {
                    Participe participe = new Participe()
                    {
                        IdArtiste = ArtistesController.GetArtistId(w),
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
                    db.SaveChanges();
                }
                else
                {
                    movie.Participes.Add(db.Participes.FirstOrDefault(m => m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre));
                    db.SaveChanges();
                }
            }

            //Actors
            String[] actors = JSONObj["Actors"].Split(',');
            foreach (String a in actors)
            {
                var idartist = ArtistesController.GetArtistId(a);
                if (db.Participes.FirstOrDefault(m => m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre) == null)
                {
                    Participe participe = new Participe()
                    {
                        IdArtiste = ArtistesController.GetArtistId(a),
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
                    db.SaveChanges();
                }
                else
                {
                    movie.Participes.Add(db.Participes.FirstOrDefault(m => m.IdArtiste == idartist && m.IdOeuvre == movie.IdOeuvre));
                    db.SaveChanges();
                }
            }

            return movie;
        }

        public static string CleanJson(string json)
        {
            var index1 = json.IndexOf("[");
            var index2 = json.IndexOf("]");
            return json.Remove(index1 - 11, index2 - index1 + 12);
        }
    }
}

