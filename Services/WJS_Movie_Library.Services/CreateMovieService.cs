using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJS_Movie_Library.Services
{
    public class CreateMovieService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private string _pageTitle = "Enter New Movie Data";
        private string _genreTitle = "Enter a Genre for New Movie.  Press Enter on a Blank line to finish.";
        private string _movieTitle = "Movie Title: ";
        private string _duplicateTitle = "Duplicate Movie Title.  Please Try again.";
        private string _movieGenre = "Movie Genre: ";
        private string _validateMovie = "Press A to add the following information";

        public CreateMovieService()
        {

        }

        public UInt64 CreateMovie( MovieService movieService )
        {
            UInt64 newMovieId = 0;

            Console.WriteLine();
            Console.WriteLine(_pageTitle);

            string movieTitle = "";
            while (movieTitle.Length < 1)
            {
                Console.WriteLine(_movieTitle);
                movieTitle = Console.ReadLine();
                if (movieTitle.Length > 0)
                {
                    if (movieService.Exists(movieTitle))
                    {
                        Console.WriteLine(_duplicateTitle);
                        movieTitle = "";
                    }
                }
                Console.WriteLine();
            }

            string genre = "";
            IList<string> genres = new List<string>();
            Console.WriteLine(_genreTitle);
            while (genre.Length < 1)
            {
                Console.WriteLine(_movieGenre);
                genre = Console.ReadLine();
                if (genre.Length > 0)
                {
                    genres.Add(genre);
                    genre = "";
                }
                else
                {
                    genre = "Q";
                }

                Console.WriteLine();
            }

            Console.WriteLine(_validateMovie);
            string genreStr = string.Join("|", genres);
            Console.WriteLine($"{movieTitle},{genreStr}");
            string resp = Console.ReadLine();
            if (resp.Length > 0)
            {
                string respCh = resp.Substring(0, 1).ToUpper();
                if (respCh.Equals("A"))
                {
                    newMovieId = movieService.GetMaxMovieId() + 1;
                    movieService.AddMovie(newMovieId, movieTitle, genres);
                }
                else
                {
                    Console.WriteLine("Skipped!");
                }
            }

            return newMovieId;
        }
    }
}
