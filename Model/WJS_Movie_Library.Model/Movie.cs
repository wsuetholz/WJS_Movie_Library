using System;
using System.Collections;
using System.Collections.Generic;

namespace WJS_Movie_Library.Model
{
    public class Movie 
    {
        public UInt64 MovieId { get; set; }
        public string Title { get; set; }

        public IList<string> Genres { get; set; }

        public static string HeaderLine() 
        {
            return "movieid,title,genres";
        }

        public override string ToString()
        {
            string retStr = "";
            string title = Title;
            string csv=",";
            string genres = string.Join("|", Genres);

            if (title.Contains(csv))
            {
                title = "\"" + title + "\"";
            }

            retStr = $"{MovieId}{csv}{title}{csv}{genres}";

            return retStr;
        }

        public Movie(UInt64 movieId, string title, IList<string>genres)
        {
            this.MovieId = movieId;
            this.Title = title;
            this.Genres = genres;
        }

        public Movie(UInt64 movieId, string title, string genres)
        {
            string[] genreArr = genres.Split("|");
            Genres = new List<string>(genreArr);
        }
    }
}