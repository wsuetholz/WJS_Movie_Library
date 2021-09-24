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
        private IList<Movie> movieList;

        public sealed class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.MovieId).Name("movieId");
                Map(m => m.Title).Name("title");
                Map(m => m.Genres).Name("genres");
            }
        }

        public MovieService(string fname) 
        {
            using (var reader = new StreamReader(fname))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<MovieMap>();

                    var records = csv.GetRecords<Movie>();
                    movieList = new List<Movie>(records);
                }
            
            }
        }

        public IList<Movie> GetMovies()
        {
            return movieList;
        }

        public IList<Movie> GetRangeOfMovies (int startIndex, int length)
        {
            IList<Movie> movies = new List<Movie>();

            for (int idx = startIndex; idx < (startIndex + length); idx++)
            {
                if (idx >= movieList.Count)
                    break;
                movies.Add(movieList[idx]);
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
            movieList.Add(movie);
        }

        public void SaveMovies(string fname)
        {
            using (var sw = new StreamWriter(fname))
            {
                sw.WriteLine(Movie.HeaderLine());

                foreach (Movie movie in movieList)
                {
                    sw.WriteLine(movie.ToString());
                }
            }
        }

        public UInt64 GetMaxMovieId()
        {
            UInt64 ret = 0;

            foreach (Movie movie in movieList)
            {
                if (movie.MovieId > ret) ret = movie.MovieId;
            }

            return ret;
        }

        public bool Exists(string title)
        {
            bool result = false;
            string uComp = title.ToUpper();

            foreach (Movie movie in movieList)
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