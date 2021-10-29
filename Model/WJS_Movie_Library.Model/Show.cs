using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using WJS_Movie_Library.Shared;

namespace WJS_Movie_Library.Model
{
    public class Show : MediaBase
    {
        public sealed class MovieMap : ClassMap<Show>
        {
            public MovieMap()
            {
                Map(m => m.Id).Index(0).Name("showId");
                Map(m => m.Title).Index(1).Name("title");
                Map(m => m.Season).Index(2).Name("season");
                Map(m => m.Episode).Index(3).Name("episode");
                Map(m => m.Writers).Index(4).Name("writers").TypeConverter<StringArrayCsvConverter>();
            }
        }

        private string _mediaTypeName = "Show";
        public override string MediaTypeName { get => _mediaTypeName; }

        private string _pageHeader = "Show List:\nShow Id   Title   Season   Episode    Writers";
        private string _pageHeaderType = "Show List:\nMedia Type     Show Id   Title   Season   Episode    Writers";
        private string _seasonEntry = "Season Number: ";
        private string _episodeEntry = "Episode Number: ";
        private string _writerTitle = "Enter a Writers for New Show.  Press Enter on a Blank line to finish.";
        private string _writerEntry = "Writer Name: ";

        private enum CustomFields { FieldSeason, FieldEpisode, FieldWriters };
        private CustomFields _currField = CustomFields.FieldSeason;

        public int Season { get; set; }
        public int Episode { get; set; }

        private IList<string> _writers = new List<string>();
        public IList<string> Writers
        {
            get { return _writers; }
            set
            {
                if (value.Count == 1 && value[0].Contains("|"))
                {
                    _writers = this.SetWriters(value[0]);
                }
                else
                {
                    _writers = value;
                }
            }
        }

        public Show()
        {
        }

        public Show(UInt64 movieId, string title, int season, int episode, IList<string> writers)
        {
            this.Id = movieId;
            this.Title = title;
            this.Writers = writers;
        }

        public Show(string csvLine, string fieldSep = ",", string subFieldSep = "|")
        {
            int idx = 0;
            string[] fields = csvLine.Split(fieldSep);
            if (fields.Length > idx)
            {
                try
                {
                    Id = UInt64.Parse(fields[idx]);
                }
                catch (Exception ex)
                { // Log Exception 
                }
                idx++;
            }
            if (fields.Length > idx)
            {
                Title = fields[idx];
                idx++;
            }
            if (fields.Length > idx)
            {
                Writers = this.SetWriters(fields[idx], subFieldSep);
                idx++;
            }
        }

        public IList<string> SetWriters(string line, string fieldSep = "|")
        {
            string[] fields = line.Split(fieldSep);
            IList<string> writers = new List<string>();
            foreach (string writer in fields)
            {
                writers.Add(writer);
            }

            return writers;
        }

        public override string ToString()
        {
            string retStr = "";
            string title = Title;
            string csv = ",";
            int season = Season;
            int episode = Episode;
            string writers = string.Join("|", Writers);

            if (title.Contains(csv))
            {
                title = "\"" + title + "\"";
            }

            retStr = $"{Id}{csv}{title}{csv}{season}{csv}{episode}{csv}{writers}";

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

            if (first)
            {
                _currField = CustomFields.FieldSeason;
            }

            switch (_currField)
            {
                case CustomFields.FieldSeason:
                    string sSeason = "";
                    int season = 0;

                    while (season < 1)
                    {
                        Console.WriteLine(_seasonEntry);
                        sSeason = Console.ReadLine();
                        if (sSeason.Length > 0)
                        {
                            try
                            {
                                season = Int32.Parse(sSeason);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Entry {sSeason} not a valid number!");
                            }
                        }
                        if (season < 1)
                        {
                            Console.WriteLine("Season entry is required to be > 0.  Please try again.");
                        }
                    }
                    Season = season;
                    _currField++;
                    break;
                case CustomFields.FieldEpisode:
                    string sEpisode = "";
                    int episode = 0;
                    while (episode < 1)
                    {
                        Console.WriteLine(_episodeEntry);
                        sEpisode = Console.ReadLine();
                        if (sEpisode.Length > 0)
                        {
                            try
                            {
                                episode = Int32.Parse(sEpisode);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Entry {sEpisode} not a valid number!");
                            }
                        }
                        if (episode < 1)
                        {
                            Console.WriteLine("Episode entry is required to be > 0.  Please try again.");
                        }
                    }
                    Episode = episode;
                    _currField++;
                    break;
                case CustomFields.FieldWriters:
                default:
                    string writer = "";
                    IList<string> writers = new List<string>();
                    Console.WriteLine(_writerTitle);
                    while (writer.Length < 1)
                    {
                        Console.WriteLine(_writerEntry);
                        writer = Console.ReadLine();
                        if (writer.Length > 0)
                        {
                            writers.Add(writer);
                            writer = "";
                        }
                        else
                        {
                            writer = "Q";
                        }

                        Console.WriteLine();
                    }

                    Writers = writers;
                    done = true;
                    break;
            }

            return done;
        }
    }
}
