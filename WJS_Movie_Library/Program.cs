using System;
using System.Collections.Generic;
using CsvHelper;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Services;

namespace WJS_Movie_Library
{
    class Program
    {
        static void Main(string[] args)
        {
            String fName = "../../../../Data/movies.csv";
            bool firstTime = true;

            MovieService movieService = new MovieService(fName);
            MainMenuService mainMenuService = new MainMenuService();
            MainMenuService.MainMenuCommandOptions cmdOpt;
            ListMoviesService listMovies = new ListMoviesService(20);
            CreateMovieService createMovie = new CreateMovieService();

            do
            {
                cmdOpt = mainMenuService.Prompt(firstTime);
                firstTime = false;

                switch (cmdOpt)
                {
                    case MainMenuService.MainMenuCommandOptions.Add:
                        createMovie.CreateMovie(movieService);
                        break;
                    case MainMenuService.MainMenuCommandOptions.List:
                        listMovies.ShowMovies(movieService);
                        break;
                    case MainMenuService.MainMenuCommandOptions.Save:
                        movieService.SaveMovies(fName);
                        break;
                    default:
                        break;
                }
            } while (cmdOpt != MainMenuService.MainMenuCommandOptions.Quit);
        }
    }
}
