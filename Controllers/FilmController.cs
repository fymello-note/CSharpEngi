using ProgettoProva.web.Models.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoProva.web.Controllers
{
    public class FilmController : Controller
    {

        private static List<FilmViewModel> films = new List<FilmViewModel>();
        // GET: Film
        public ActionResult Index()
        {
            return View(films);
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
                films.Add(film);

                return Request.Form["AddAnother"] == "1" ? RedirectToAction("RegisterFilm") : RedirectToAction("Index");
            }
            return View(film);
        }
    }
}