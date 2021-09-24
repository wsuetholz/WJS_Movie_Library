using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using WJS_Movie_Library.Model;
using CsvHelper.Configuration;

namespace WJS_Movie_Library.Services
{
    public class MovieService 
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private IList<Movie> _movieList;
        private IList<Movie> _newMovies;

        public sealed class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.MovieId).Name("movieId");
                Map(m => m.Title).Name("title");
                Map(m => m.Genres).Name("genres");
            }
        }

        public MovieService()
        {
            _movieList = new List<Movie>();
            _newMovies = new List<Movie>();
        }

        public MovieService(string fname) 
        {
            _movieList = this.LoadFromCSV(fname);
            _newMovies = new List<Movie>();
        }

        public IList<Movie> LoadFromCSV(string fname)
        {
            IList<Movie> movieList = new List<Movie>();

            using (var reader = new StreamReader(fname))
            {
                try
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<MovieMap>();

                        var records = csv.GetRecords<Movie>();
                        movieList = new List<Movie>(records);
                    }
                }
                catch (Exception ex)
                {
                    // Log the Exception
                    log.Error($"Problem Reading Data File {fname}.  EX: {ex}");
                }
            }

            return movieList;
        }

        public IList<Movie> GetMovies()
        {
            return _movieList;
        }

        public IList<Movie> GetRangeOfMovies (int startIndex, int length)
        {
            IList<Movie> movies = new List<Movie>();

            for (int idx = startIndex; idx < (startIndex + length); idx++)
            {
                if (idx >= _movieList.Count)
                    break;
                movies.Add(_movieList[idx]);
            }

            return movies;
        }

        public void AddMovie(UInt64 movieId, string movieTitle, IList<string>genres)
        {
            Movie movie = new Movie(movieId, movieTitle, genres);
            this.AddMovie(movie);
        }

        public void AddMovie(Movie movie)
        {
            _movieList.Add(movie);
            _newMovies.Add(movie);
        }

        public bool NeedSave()
        {
            if (_newMovies.Count > 0)
                return true;
            else
                return false;
        }

        public void SaveMovies(string fname)
        {
            if (NeedSave())
            {
                using (var sw = new StreamWriter(fname, true))
                {
                    foreach (Movie movie in _newMovies)
                    {
                        sw.WriteLine(movie.ToString());
                    }
                }
            }
        }

        public UInt64 GetMaxMovieId()
        {
            UInt64 ret = 0;

            foreach (Movie movie in _movieList)
            {
                if (movie.MovieId > ret) ret = movie.MovieId;
            }

            return ret;
        }

        public bool Exists(string title)
        {
            bool result = false;
            string uComp = title.ToUpper();

            foreach (Movie movie in _movieList)
            {
                string uTitle = movie.Title.ToUpper();

                if (uTitle.Equals(uComp))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}