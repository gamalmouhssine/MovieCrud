using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCrud.Models;
using MovieCrud.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCrud.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<List<Genre>> loaddropdown()
        {
            return await _context.Genres.OrderBy(m => m.GName).ToListAsync();
        }

        private ValueTask<Movie> Findmovie(int? id)
        {
            return _context.Movies.FindAsync(id);
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Movies.ToListAsync());
        }



        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieModel
            {
                Genres = await loaddropdown()
            };

            return View("MovieForm",ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieModel model)
        {
            var file = Request.Form.Files;
            var poster = file.FirstOrDefault();
            var stream = new MemoryStream();
            var allowedExtention = new List<string> { ".jpg", ".png" };
            if (!ModelState.IsValid && !file.Any() && !allowedExtention.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await loaddropdown();
                ModelState.AddModelError("Poster", "Please select Movie image!!!");
                ModelState.AddModelError("Poster", "Only Png or Jpg images!!!");
                return View("MovieForm", model);

            }
            await poster.CopyToAsync(stream);
            var movie = new Movie
            {
                Titel = model.Titel,
                GenreId = model.GenreId,
                Year = model.Year,
                Store = model.Store,
                Rate=model.Rate,
                Poster = stream.ToArray()

            };
            _context.Movies.Add(movie);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }



        public async Task<IActionResult>Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movie = await Findmovie(id);
            if (movie==null)
            {
                return NotFound();
            }
            var ViewModel = new MovieModel
            {
                Id = movie.Id,
                Titel = movie.Titel,
                GenreId = movie.GenreId,
                Rate = movie.Rate,
                Year = movie.Year,
                Store = movie.Store,
                Poster = movie.Poster,
                Genres = await loaddropdown()
            };
            return View("MovieForm",ViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieModel model)
        {
            var file = Request.Form.Files;
            var poster = file.FirstOrDefault();
            var stream = new MemoryStream();
            var movie = await Findmovie(model.Id);
            var allowedExtention = new List<string> { ".jpg", ".png" };
            if (!ModelState.IsValid && !file.Any() && !allowedExtention.Contains(Path.GetExtension(poster.FileName).ToLower())&& movie == null)
            {
                model.Genres = await loaddropdown();
                ModelState.AddModelError("Poster", "Please select Movie image!!!");
                ModelState.AddModelError("Poster", "Only Png or Jpg images!!!");
                return View("MovieForm", model);

            }
            else
            {
                movie.Poster = stream.ToArray();
            }
           
            await poster.CopyToAsync(stream);

            movie.Titel = model.Titel;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.Store = model.Store;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



    }
}
