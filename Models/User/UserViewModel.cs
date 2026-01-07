using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Models.User
{
    public class UserViewModel {

        [Required(ErrorMessage = "il nome è obbligatorio.")]
        [RegularExpression("^([A-Za-z\\- ]+)$", ErrorMessage = "Il nome può contenere lettere, spazi e trattini.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "il nome è obbligatorio.")]
        [RegularExpression("^([A-Za-z\\- ]+)$", ErrorMessage = "Il cognome può contenere lettere, spazi e trattini.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "il nome è obbligatorio.")]
        [MinLength(2, ErrorMessage = "L'username deve avere almeno 2 lettere.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "il nome è obbligatorio.")]
        [MinLength(8, ErrorMessage = "La password deve avere almeno 8 lettere.")]
        public string Password { get; set; }
    }
}