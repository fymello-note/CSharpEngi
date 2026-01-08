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
        private static readonly char[] specialChar = new char[] { '$', '-', '_', '=' };

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

            string errorMessage = CheckPasswordError(input.Password);
            if (!string.IsNullOrEmpty(errorMessage))
                ModelState.AddModelError("Password", "La password deve contenere almeno" + errorMessage);

            return View(input);
        }

        private string CheckPasswordError(string password)
        {
            bool[] passwordCheckResults = CheckPassword(password);

            bool upper = passwordCheckResults[0];
            bool lower = passwordCheckResults[1];
            bool number = passwordCheckResults[2];
            bool special = passwordCheckResults[3];

            string errorMessage = string.Empty;

            if (!upper)
            {
                errorMessage += " una maiuscola <br>";
            }
            if (!lower)
            {
                errorMessage += "La password deve contenere almeno una minuscola \n";
            }
            if (!number)
            {
                errorMessage += "La password deve contenere almeno un numero \n";
            }
            if (!special)
            {
                errorMessage += "La password deve contenere almeno carattere tra ( "+ string.Join("", specialChar) +" )";
            }

            

            return errorMessage;

        }

        private bool[] CheckPassword(string password) {

            bool[] bools = new bool[4] { false, false, false, false };

            bool upper = false;
            bool lower = false;
            bool number = false;
            bool special = false;

            foreach (var c in password) {
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

            bools[0] = upper;
            bools[1] = lower;
            bools[2] = number;
            bools[3] = special;

            return bools;
        }
    }
}