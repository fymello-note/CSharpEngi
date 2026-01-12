using ProgettoProva.web.Models.Film;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using ProgettoProva.web.Extensions.Database;
using System.Reflection;

namespace ProgettoProva.web.Controllers
{
    public class FilmController : Controller
    {
        private const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=testDatabase;";

        // private static List<FilmViewModel> films = new List<FilmViewModel>();

        // GET: Film
        public ActionResult Index()
        {
            return View(GetFilms());
        }

        [HttpGet]
        public ActionResult RegisterFilm()
        {
            ViewData["GenreListItems"] = GetGenreListItem();
            var film = new FilmViewModel()
            {
                Title = "Mad Max",
                ReleaseDate = new DateTime(2015, 01, 01),
                Director = "George Miller",
                Cast = new List<string>() { "Tom Hardy" }
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
            ViewData["GenreListItems"] = GetGenreListItem();
            return View(film);
        }

        private void AddFilm(FilmViewModel film)
        {

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO film (Title, ReleaseDate, Director, Producer, GenreId) VALUES (@Title, @ReleaseDate, @Director, @Producer, @GenreId);";

                    // command.Parameters.AddWithValue("Title", film.Title);
                    // command.Parameters.AddWithValue("ReleaseDate", film.ReleaseDate);
                    // command.Parameters.AddWithValue("Director", film.Director);
                    //command.Parameters.AddWithValue("Producer", film.Producer == null ? DBNull.Value : (object)film.Producer);
                    // command.Parameters.AddWithValueOrDBNull("Producer", film.Producer);

                    /*
                    command.Parameters.BindParameters(new Dictionary<string, object>()
                    {
                        //{ "Director", film.Director }, <- altro modo per scrivere
                        ["Title"] = film.Title,
                        ["ReleaseDate"] = film.ReleaseDate,
                        ["Director"] = film.Director,
                        ["Producer"] = film.Producer
                    });
                    */

                    var record = new FilmRecord()
                    {
                        Title = film.Title,
                        Director = film.Director,
                        Producer = film.Producer,
                        ReleaseDate = film.ReleaseDate,
                    };

                    command.Parameters.BindParameters(film, new string[] { nameof(film.FilmId), "Cast", nameof(film.Genre)});

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
                    command.CommandText = @"SELECT film.FilmId, film.Title, film.ReleaseDate, film.Director, film.Producer, genre.[Name] as Genre 
                                            From dbo.Film as film inner join dbo.Genre as genre 
                                            on Film.GenreId = Genre.GenreId;";

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int filmIdPos = reader.GetOrdinal("FilmId");
                            int titlePos = reader.GetOrdinal("Title");
                            int releaseDatePos = reader.GetOrdinal("ReleaseDate");
                            int directorPos = reader.GetOrdinal("Director");
                            int producerPos = reader.GetOrdinal("Producer");
                            int genrePos = reader.GetOrdinal("Genre");
                            
                            do
                            {
                                films.Add(
                                    new FilmViewModel
                                    {
                                        FilmId = reader.GetInt32(filmIdPos),
                                        Title = reader.GetString(titlePos),
                                        ReleaseDate = reader.GetDateTime(releaseDatePos),
                                        Director = reader.GetString(directorPos),
                                        Producer = !reader.IsDBNull(producerPos) ? reader.GetString(producerPos) : null,
                                        Genre = reader.GetString(genrePos),
                                        /*Cast = new List<string>() { "mike" }*/
                                    });
                            } while (reader.Read());
                        }
                    }
                }
            }

            return films;
        }

        [HttpGet]
        public ActionResult EditFilm(int id)
        {
            ViewData["GenreListItems"] = GetGenreListItem();
            return View(GetFilmById(id));
        }

        [HttpPost]
        public ActionResult EditFilm(int id, FilmViewModel film)
        {
            if (ModelState.IsValid)
            {
                //string idString = Request.Form["FilmToMod"];

                //film.FilmId = int.Parse(idString);

                film.FilmId = id;
                
                UpdateFilm(film);

                return RedirectToAction("Index");
            }

            ViewData["GenreListItems"] = GetGenreListItem();
            return View(film);
        }

        public void UpdateFilm(FilmViewModel film)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Film SET Title = @Title, ReleaseDate = @ReleaseDate, Director = @Director, Producer = @Producer WHERE FilmId = @FilmId;";

                    command.Parameters.BindParameters(film, new string[] { "Cast", nameof(film.Genre)});

                    command.ExecuteNonQuery();
                }
            }
        }

        public FilmViewModel GetFilmById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT FilmId, Title, ReleaseDate, Director, Producer, GenreId FROM Film WHERE FilmId = @FilmId";

                    command.Parameters.AddWithValue("FilmId", id);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            int titlePos = reader.GetOrdinal("Title");
                            int releaseDatePos = reader.GetOrdinal("ReleaseDate");
                            int directorPos = reader.GetOrdinal("Director");
                            int producerPos = reader.GetOrdinal("Producer");
                            int filmIdPos = reader.GetOrdinal("FilmId");
                            int genreIdPos = reader.GetOrdinal("GenreId");

                            FilmViewModel filmToReturn = new FilmViewModel()
                            {
                                Title = reader.GetString(titlePos),
                                Director = reader.GetString(directorPos),
                                Producer = reader.GetString(producerPos) ?? null,
                                FilmId = reader.GetInt32(filmIdPos),
                                ReleaseDate = reader.GetDateTime(releaseDatePos),
                                Cast = new List<string>(),
                                GenreId = reader.GetInt16(genreIdPos)
                            };

                            return filmToReturn;
                        }

                        throw new Exception("Film by FilmId not found");
                    }
                }
            }
        }

        private List<SelectListItem> GetGenreListItem()
        {
            List<SelectListItem> items = new List<SelectListItem>() { };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT GenreId, [Name] FROM dbo.Genre";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            int genreIdPos = reader.GetOrdinal("GenreId");
                            int namePos = reader.GetOrdinal("Name");

                            do
                            {
                                SelectListItem item = new SelectListItem()
                                {
                                    Value = reader.GetInt16(genreIdPos).ToString(),
                                    Text = reader.GetString(namePos)

                                };

                                items.Add(item);
                            } while (reader.Read());
                        }
                    }

                }
            }
            return items;
        }
    }
}