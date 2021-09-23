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

        public Movie(string csvLine, string fieldSep = ",", string subFieldSep = "|")
        {
            string[] fields = csvLine.Split(fieldSep);
            if (fields.Length > 0)
            {
                try 
                { 
                    MovieId = UInt64.Parse(fields[0]); 
                    }
                catch (Exception ex) 
                { // Log Exception 
                }
            }
            if (fields.Length > 1) Title = fields[1];
            if (fields.Length > 2) this.SetGenres(fields[2], subFieldSep);
        }

        public Movie(UInt64 movieId, string title, string genres)
        {
            string[] genreArr = genres.Split("|");
            Genres = new List<string>(genreArr);
        }

        public void SetGenres(string genreLine, string fieldSep = "|")
        {
            string [] fields = genreLine.Split(fieldSep);
            IList<string> genres = new List<string>();
            foreach (string genre in fields)
            {
                genres.Add(genre);
            }

            Genres = genres;
        }
    }
}