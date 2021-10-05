using System;
using System.Collections.Generic;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Services;
using Xunit;

namespace WJS_Movie_Library.Tests
{
    public class MovieService_UnitTests
    {
        [Fact]
        public void TitleExists()
        {
            CsvMediaService<Movie> movieService = new CsvMediaService<Movie>();
            IList<string> genres = new List<string>();
            genres.Add("Genre1");
            genres.Add("Genre2");

            movieService.Add(new Movie((UInt64)1, "Title 1(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 2(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 3(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 4(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 5(1900)", genres));

            bool result = movieService.Exists("Title 3(1900)");

            Assert.True(result);
        }

        [Fact]
        public void TitleDoesntExist()
        {
            CsvMediaService<Movie> movieService = new CsvMediaService<Movie>();
            IList<string> genres = new List<string>();
            genres.Add("Genre1");
            genres.Add("Genre2");

            movieService.Add(new Movie((UInt64)1, "Title 1(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 2(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 3(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 4(1900)", genres));
            movieService.Add(new Movie((UInt64)1, "Title 5(1900)", genres));

            bool result = movieService.Exists("Title 0(1900)");

            Assert.False(result);
        }
    }
}
