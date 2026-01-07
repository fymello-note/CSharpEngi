using ProgettoProva.web.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoProva.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About() {
            var model = new AboutViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult About(AboutViewModel input) {
            if(input.Username != "hello")
            {
                ModelState.AddModelError("Username", "Username non corretto");
            }

            if (input.Password != "password")
            {
                ModelState.AddModelError("Password", "Password non corretta");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            /*if (input.Username == "hello" && input.Password == "password") {

            } else {
                //input.ErrorMessage = "Credenziali non valide";
            }*/


                return View(input);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}