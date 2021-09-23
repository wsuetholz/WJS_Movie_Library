using System;
using CsvHelper;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Services;

namespace WJS_Movie_Library
{
    class Program
    {
        static void Main(string[] args)
        {
            String fName = "Data/movies.csv";

            MovieService movieService = new MovieService(fName);
            foreach (Movie movie in movieService.GetMovies())
            {
                Console.WriteLine(movie);
            }
        }
    }
}
