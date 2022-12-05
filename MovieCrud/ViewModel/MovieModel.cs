using MovieCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCrud.ViewModel
{
    public class MovieModel
    {
        public int Id { get; set; }

        [Required,StringLength(50)]
        public string Titel { get; set; }

        [Required]
        public int Year { get; set; }

        [Required, Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2000)]
        public string Store { get; set; }

        [Display(Name ="Select Poster!!")]
        public byte[] Poster { get; set; }

        [Display(Name ="Genre")]
        public byte GenreId { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
    }
}
