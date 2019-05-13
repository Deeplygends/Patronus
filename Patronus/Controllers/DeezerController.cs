
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Patronus.Models;

namespace Patronus.Controllers
{
    public class DeezerController : Controller
    {
        private  static PatronusDBEntities db = new PatronusDBEntities();
        public static JsonDeezer GetDeezerResult(string title)
        {
            string parameters = "";
            
            /*if (!String.IsNullOrEmpty(title))
            {
                parameters += "track:\"" + title + "\"";
            }*/
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://api.deezer.com/search?q=" + title));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            JsonDeezer json = JsonConvert.DeserializeObject<JsonDeezer>(jsonString);
            foreach (var element in json.data)
            {
                var type = db.TypeOeuvres.FirstOrDefault(x => x.LabelType.Equals("Track"));
                if (type is null)
                {
                    type = new TypeOeuvre()
                    {
                        LabelType = "Track"
                    };
                    db.TypeOeuvres.Add(type);
                    db.SaveChanges();
                }

                Oeuvre o = db.Oeuvres.FirstOrDefault(x => x.IdAPI.Equals(element.id.ToString() +"D")) ?? new Oeuvre()
                {

                    DateAjout = DateTime.Now,
                    IdAPI = element.id.ToString() + "D",
                    TypeOeuvre = type,
                    UrlImage = element.link,
                    IdContributeur = "IDapiDeezer",
                    Label = element.title
                };
                if (db.Oeuvres.FirstOrDefault(x => x.IdAPI.Equals(element.id.ToString() + "D")) == null)
                {
                    db.Oeuvres.Add(o);
                    db.SaveChanges();
                }

                Artiste artiste = db.Artistes.FirstOrDefault(x => x.Nom.Equals(element.artist.name)) ?? new Artiste()
                {
                    IdArtiste = Guid.NewGuid().ToString(),
                    Nom = element.artist.name,
                    Prenom = ""
                };
                if (db.Artistes.FirstOrDefault(x => x.Nom.Equals(element.artist.name)) == null)
                {
                    db.Artistes.Add(artiste);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    }
                }

                type = db.TypeOeuvres.FirstOrDefault(x => x.LabelType.Equals("Album"));
                    if (type is null)
                    {
                        type = new TypeOeuvre()
                        {
                            LabelType = "Album"
                        };
                        db.TypeOeuvres.Add(type);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    }
                }
                 
                var alb = db.Oeuvres.FirstOrDefault(x => x.IdAPI.Equals(element.album.id.ToString() + "D")) ?? new Oeuvre()
                {
                    Label = element.album.title,
                    DateAjout = DateTime.Now,
                    IdAPI = element.album.id.ToString() + "D",
                    TypeOeuvre = type,
                    IdContributeur = "IDapiDeezer",
                    UrlImage = element.album.cover_xl
                };
                alb.Enfants.Add(o);

                if (db.Oeuvres.FirstOrDefault(x => x.IdAPI.Equals(element.album.id.ToString() + "D")) == null)
                {
                    db.Oeuvres.Add(alb);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    }
                }

                

            }

            return json;
        }


    }
}