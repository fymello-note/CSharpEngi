using ProgettoProva.web.Models.Film;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using ProgettoProva.web.Extensions.Database;
using ProgettoProva.web.Exceptions;
using System.Reflection;
using System.Data;

namespace ProgettoProva.web.Controllers
{
    public class FilmController : Controller
    {

        private const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=testDatabase;";

        // private static List<FilmViewModel> films = new List<FilmViewModel>();

        // Action
        [HttpGet]
        public ActionResult Index()
        {
            return View(GetFilms());
        }

        // Action
        [HttpGet]
        public ActionResult RegisterFilm()
        {
            ViewData["GenreListItems"] = GetGenreListItem();
            return View();
        }

        // Action
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

        // Action
        [HttpGet]
        public ActionResult EditFilm(int id)
        {
            ViewData["GenreListItems"] = GetGenreListItem();
            try
            {
                FilmViewModel film = GetFilmById(id);
                return View(film);
            }catch (ResourceNotFoundException) {
                return View("NotFound");
            }
        }

        // Action
        [HttpPost]
        public ActionResult EditFilm(int id, FilmViewModel film)
        {
            if (ModelState.IsValid)
            {
                //string idString = Request.Form["FilmToMod"];

                //film.FilmId = int.Parse(idString);

                UpdateFilm(id, film);

                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreListItems"] = GetGenreListItem();
            return View(film);
        }


    }
}