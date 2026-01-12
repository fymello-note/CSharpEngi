using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Models.Film
{
    public class FilmDataGridItemViewModel
    {
        public int FilmId { get; set; }
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        public string Producer { get; set; }

        public List<string> Cast { get; set; } = new List<string>();
    }
}