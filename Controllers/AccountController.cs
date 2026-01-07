using ProgettoProva.web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoProva.web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index(){
            return View();
        }

        [HttpGet]
        public ActionResult Login() {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel input) {
            if(input.Username != "hello")
            {
                ModelState.AddModelError("Username", "Username non corretto");
            }

            if (input.Password != "password")
            {
                ModelState.AddModelError("Password", "Password non corretta");
            }

            if (ModelState.IsValid) {
                return RedirectToAction("Index", "home");
            }
            return View(input);
        }
    }
}