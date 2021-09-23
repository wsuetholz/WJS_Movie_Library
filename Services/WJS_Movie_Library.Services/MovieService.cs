using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using WJS_Movie_Library.Model;

namespace WJS_Movie_Library.Services
{
    public class MovieService 
    {
        private IList<Movie> movieList;

        public MovieService(string fname) 
        {
            using (var reader = new StreamReader(fname))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Movie>();
                    movieList = new List<Movie>(records);
                }
            
            }
        }

        public IList<Movie> GetMovies()
        {
            return movieList;
        }

        public void AddMovie(Movie movie)
        {

        }

        public bool Exists(string title)
        {
            bool result = false;

            return result;
        }
    }
}