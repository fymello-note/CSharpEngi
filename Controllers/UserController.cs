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
            foreach ()
            {

            }
            return View(input);
        }
    }
}