using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Models.Film
{
    /// <summary>
    /// Represenrs a film (db record) with its title, release date, director and producer information.
    /// </summary>
    public class FilmRecord
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
    }
}