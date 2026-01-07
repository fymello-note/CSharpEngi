using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Lo Username è obbligatorio.")]
        public string Username { get; set; }

        [DisplayName("La tua password")]
        [Required(ErrorMessage = "{0} è obbligatoria.")]
        public string Password { get; set; }

    }
}