using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ReleaseYear { get; set; }
        public string Genre { get; set; }
        public int? Duration { get; set; }
        public decimal? Rating { get; set; }
        public string Description { get; set; }   
    }
}
