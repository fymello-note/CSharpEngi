using ProgettoProva.web.Models.Film;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace ProgettoProva.web.Controllers
{
    public class FilmController : Controller
    {
        private const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=testDatabase;";

        private static List<FilmViewModel> films = new List<FilmViewModel>();
        // GET: Film
        public ActionResult Index()
        {
            return View(GetFilms());
        }

        [HttpGet]
        public ActionResult RegisterFilm()
        {
            var film = new FilmViewModel()
            {
                //Title = "Mad Max",
                //ReleaseDate = new DateTime(2015, 01, 01),
                //Director = "George Miller",
                //Cast = new List<string>() { "Tom Hardy" }
            };
            return View(film);
        }

        [HttpPost]
        public ActionResult RegisterFilm(FilmViewModel film)
        {
            if (ModelState.IsValid)
            {
                AddFilm(film);

                return Request.Form["AddAnother"] == "1" ? RedirectToAction("RegisterFilm") : RedirectToAction("Index");
            }
            return View(film);
        }

        private void AddFilm(FilmViewModel film)
        {

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO film (Title, ReleaseDate, Director, Producer) VALUES (@Title, @ReleaseDate, @Director, @Producer);";

                    command.Parameters.AddWithValue("Title", film.Title);
                    command.Parameters.AddWithValue("ReleaseDate", film.ReleaseDate);
                    command.Parameters.AddWithValue("Director", film.Director);
                    command.Parameters.AddWithValue("Producer", film.Producer == null ? DBNull.Value : (object) film.Producer);

                    command.ExecuteNonQuery();
                }
            }
        }

        private List<FilmViewModel> GetFilms()
        {
            List<FilmViewModel> films = new List<FilmViewModel>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Title, ReleaseDate, Director, Producer FROM Film;";

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int titlePos = reader.GetOrdinal("Title");
                            int releaseDatePos = reader.GetOrdinal("ReleaseDate");
                            int directorPos = reader.GetOrdinal("Director");
                            int producerPos = reader.GetOrdinal("Producer");
                            
                            do
                            {
                                films.Add(
                                    new FilmViewModel
                                    {
                                        //FilmId = reader.GetInt32(filmId)
                                        Title = reader.GetString(titlePos),
                                        ReleaseDate = reader.GetDateTime(releaseDatePos),
                                        Director = reader.GetString(directorPos),
                                        Producer = !reader.IsDBNull(producerPos) ? reader.GetString(producerPos) : null,
                                        Genre = "Azione",
                                        Cast = new List<string>() { "mike" }
                                    });
                            } while (reader.Read());
                        }
                    }
                }
            }

            return films;

        }
    }
}