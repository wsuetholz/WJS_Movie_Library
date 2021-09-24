using System;
using System.Collections.Generic;
using CsvHelper;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Services;

namespace WJS_Movie_Library
{
    class Program
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            String fName = "../../../../Data/movies.csv";
            bool firstTime = true;

            log.Info($"Startup using data file {fName}.");

            MovieService movieService = new MovieService(fName);
            MainMenuService mainMenuService = new MainMenuService();
            MainMenuService.MainMenuCommandOptions cmdOpt;
            ListMoviesService listMovies = new ListMoviesService(20);
            CreateMovieService createMovie = new CreateMovieService();

            log.Info("Initialization Complete");
            do
            {
                cmdOpt = mainMenuService.Prompt(firstTime);
                firstTime = false;

                log.Info($"Menu Choice {cmdOpt}.");
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
                    case MainMenuService.MainMenuCommandOptions.Quit:
                        if (movieService.NeedSave())
                        {
                            if (!mainMenuService.PromptOkToCloseWithoutSave())
                            {
                                cmdOpt = MainMenuService.MainMenuCommandOptions.Unset;
                            }
                        }
                        break;
                    default:
                        break;
                }
            } while (cmdOpt != MainMenuService.MainMenuCommandOptions.Quit);

            log.Info("Application Complete.");
        }

    }
}
