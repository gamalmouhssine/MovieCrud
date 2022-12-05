using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCrud.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string Store { get; set; }
        public byte[] Poster { get; set; }
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
