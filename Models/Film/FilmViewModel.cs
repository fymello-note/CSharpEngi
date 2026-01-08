using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Models.Film
{
    public class FilmViewModel
    {
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "Campo obbligatorio")]
        [MinLength(2, ErrorMessage = "Min. {1} caratteri")]
        public string Director { get; set; }
        [MinLength(2, ErrorMessage = "Min. {1} caratteri")]
        public string Producer { get; set; }
        public List<string> Cast { get; set; } = new List<string>();
    }
}