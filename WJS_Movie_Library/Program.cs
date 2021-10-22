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
            String movieFName = "movies";
            String showFName = "shows";
            String videoFName = "videos";

            string mediaType = "";
            bool firstTime = true;

            log.Info($"Startup using data files {movieFName}, {showFName}, {videoFName}.");

            //CsvMediaService<Movie> movieService = new CsvMediaService<Movie>(movieFName);
            //CsvMediaService<Show> showService = new CsvMediaService<Show>(showFName);
            //CsvMediaService<Video> videoService = new CsvMediaService<Video>(videoFName);
            JsonMediaService<Movie> movieService = new JsonMediaService<Movie>(movieFName);
            JsonMediaService<Show> showService = new JsonMediaService<Show>(showFName);
            JsonMediaService<Video> videoService = new JsonMediaService<Video>(videoFName);

            MenuService menuService = new MenuService();
            MenuService.MainMenuCommandOptions cmdOpt;
            menuService.AddMediaType("Movie");
            menuService.AddMediaType("Show");
            menuService.AddMediaType("Video");
            ListMediaService listMedia = new ListMediaService(20);
            CreateMediaRecordService createMediaRecord = new CreateMediaRecordService();

            log.Info("Initialization Complete");
            do
            {
                cmdOpt = menuService.Prompt(firstTime);
                firstTime = false;

                log.Info($"Menu Choice {cmdOpt}.");
                switch (cmdOpt)
                {
                    case MenuService.MainMenuCommandOptions.Add:
                        mediaType = menuService.PromtMediaType();

                        if (mediaType.Equals("Movie"))
                        {
                            createMediaRecord.CreateRecord<Movie>(movieService);
                        }
                        else if (mediaType.Equals("Show"))
                        {
                            createMediaRecord.CreateRecord<Show>(showService);

                        }
                        else if (mediaType.Equals("Video"))
                        {
                            createMediaRecord.CreateRecord<Video>(videoService);
                        }
                        break;
                    case MenuService.MainMenuCommandOptions.List:
                        mediaType = menuService.PromtMediaType();

                        IMediaService mediaService = movieService;
                        if (mediaType.Equals("Show"))
                        {
                            mediaService = showService;
                        }
                        else if (mediaType.Equals("Video"))
                        {
                            mediaService = videoService;
                        }
                        listMedia.ShowMedia(mediaService);
                        break;
                    case MenuService.MainMenuCommandOptions.Save:
                        movieService.Save();
                        showService.Save();
                        videoService.Save();
                        break;
                    case MenuService.MainMenuCommandOptions.Quit:
                        if (movieService.NeedSave() || showService.NeedSave() || videoService.NeedSave())
                        {
                            if (!menuService.PromptOkToCloseWithoutSave())
                            {
                                cmdOpt = MenuService.MainMenuCommandOptions.Unset;
                            }
                        }
                        break;
                    default:
                        break;
                }
            } while (cmdOpt != MenuService.MainMenuCommandOptions.Quit);

            log.Info("Application Complete.");
        }

    }
}
