using System;
using System.Collections;
using System.Collections.Generic;

namespace WJS_Movie_Library.Model
{
    public class Movie 
    {
        public UInt64 MovieId { get; set; }
        public string Title { get; set; }

        private IList<string> _genres = new List<string>();
        public IList<string> Genres 
        {
            get { return _genres; }
            set {
                if (value.Count == 1 && value[0].Contains("|"))
                {
                    _genres = this.SetGenres(value[0]);
                }
                else
                {
                    _genres = value;
                }
            }
        }

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

        public Movie()
        {

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
            if (fields.Length > 2) Genres = this.SetGenres(fields[2], subFieldSep);
        }

        public Movie(UInt64 movieId, string title, string genres)
        {
            string[] genreArr = genres.Split("|");
            Genres = new List<string>(genreArr);
        }

        public IList<string> SetGenres(string genreLine, string fieldSep = "|")
        {
            string [] fields = genreLine.Split(fieldSep);
            IList<string> genres = new List<string>();
            foreach (string genre in fields)
            {
                genres.Add(genre);
            }

            return genres;
        }
    }
}