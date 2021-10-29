using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using WJS_Movie_Library.Shared;

namespace WJS_Movie_Library.Model
{
    public class Movie : MediaBase
    {
        public sealed class MovieMap : ClassMap<Movie>
        {
            public MovieMap()
            {
                Map(m => m.Id).Index(0).Name("movieId");
                Map(m => m.Title).Index(1).Name("title");
                Map(m => m.Genres).Index(2).Name("genres").TypeConverter<StringArrayCsvConverter>();
            }
        }

        private string _mediaTypeName = "Movie";
        public override string MediaTypeName { get => _mediaTypeName; }

        private string _pageHeader = "Movie List:\nMovie Id   Title       Genres";
        private string _pageHeaderType = "Movie List:\nMedia Type     Movie Id   Title       Genres";
        private string _genreTitle = "Enter a Genre for New Movie.  Press Enter on a Blank line to finish.";
        private string _movieGenre = "Movie Genre: ";

        private enum CustomFields { FieldGenre };
        private CustomFields _currField = CustomFields.FieldGenre;

        private IList<string> _genres = new List<string>();
        public IList<string> Genres
        {
            get { return _genres; }
            set
            {
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

        public Movie()
        {
        }

        public Movie(UInt64 movieId, string title, IList<string> genres)
        {
            this.Id = movieId;
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
                    Id = UInt64.Parse(fields[0]);
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
            string[] fields = genreLine.Split(fieldSep);
            IList<string> genres = new List<string>();
            foreach (string genre in fields)
            {
                genres.Add(genre);
            }

            return genres;
        }

        public override string ToString()
        {
            string retStr = "";
            string title = Title;
            string csv = ",";
            string genres = string.Join("|", Genres);

            if (title.Contains(csv))
            {
                title = "\"" + title + "\"";
            }

            retStr = $"{Id}{csv}{title}{csv}{genres}";

            return retStr;
        }

        public override void Display(bool showHeader, bool showType = false)
        {
            if (showHeader)
                if (showType)
                    Console.WriteLine(this._pageHeaderType);
                else
                    Console.WriteLine(this._pageHeader);

            if (showType)
                Console.WriteLine($"{_mediaTypeName}\t{this.ToString()}");
            else
                Console.WriteLine(this.ToString());
        }

        public override void RegisterCSVMap(CsvContext context)
        {
            context.RegisterClassMap<MovieMap>();
        }

        public override bool EnterCustomField(bool first)
        {
            bool done = false;

            _currField = CustomFields.FieldGenre;

            string genre = "";
            IList<string> genres = new List<string>();
            Console.WriteLine(_genreTitle);
            while (genre.Length < 1)
            {
                Console.WriteLine(_movieGenre);
                genre = Console.ReadLine();
                if (genre.Length > 0)
                {
                    genres.Add(genre);
                    genre = "";
                }
                else
                {
                    genre = "Q";
                }

                Console.WriteLine();
            }

            Genres = genres;
            done = true;

            return done;
        }
    }
}