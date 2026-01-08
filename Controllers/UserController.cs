using ProgettoProva.web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoProva.web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index() {
            var Users = new List<UserViewModel>()
            {
                new UserViewModel
                {
                    FirstName = "pippo", LastName ="cocaina", UserName = "pippo.cocaina", Password = "gg"
                },
                new UserViewModel
                {
                    FirstName = "pippo", LastName ="cocaina", UserName = "pippo.cocaina", Password = "gg"
                }
            };
            
            return View();
        }

        [HttpGet]
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel input) {

            char[] specialChar = new char[] { '$', '-', '_', '=' };

            bool upper = false;
            bool lower = false;
            bool number = false;
            bool special = false;

            foreach (var c in input.Password) {
                if (char.IsUpper(c)) {
                    upper = true;
                }
                else if (char.IsLower(c))
                {
                    lower = true;
                }
                else if (char.IsDigit(c))
                {
                    number = true;
                }
                else if (specialChar.Any(sc => c == sc)) {
                    special = true;
                }

                if (upper && lower && number && special) 
                    break;
                
            }

            if (!upper)
            {
                ModelState.AddModelError("Password", "La password deve contenere almeno una maiuscola");
            }
            if (!lower)
            {
                ModelState.AddModelError("Password", "La password deve contenere almeno una minuscola");
            }
            if (!number)
            {
                ModelState.AddModelError("Password", "La password deve contenere almeno un numero");
            }
            if (!special)
            {
                ModelState.AddModelError("Password", "La password deve contenere almeno carattere tra ( "+ string.Join("", specialChar) +" )");
            }


            return View(input);
        }
    }
}