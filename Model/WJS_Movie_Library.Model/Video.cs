using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using WJS_Movie_Library.Shared;

namespace WJS_Movie_Library.Model
{
    public class Video : MediaBase
    {
        public sealed class MovieMap : ClassMap<Video>
        {
            public MovieMap()
            {
                Map(m => m.Id).Index(0).Name("videoId");
                Map(m => m.Title).Index(1).Name("title");
                Map(m => m.Format).Index(2).Name("format");
                Map(m => m.Length).Index(3).Name("length");
                Map(m => m.Regions).Index(4).Name("regions").TypeConverter<IntArrayCsvConverter>();
            }
        }

        private string _mediaTypeName = "Video";
        public override string MediaTypeName { get => _mediaTypeName; }

        private string _pageHeader = "Video List:\nVideo Id   Title    Format    Length    Regions";
        private string _formatEntry = "Video Format: ";
        private string _lengthEntry = "Video Length: ";
        private string _regionTitle = "Enter the Regions for New Video.  Press Enter on a Blank line to finish.";
        private string _regionEntry = "Region Id: ";

        private enum CustomFields { FieldFormat, FieldLength, FieldRegion };
        private CustomFields _currField = CustomFields.FieldFormat;

        public string Format { get; set; }
        public int Length { get; set; }

        private IList<int> _regions = new List<int>();
        public IList<int> Regions
        {
            get { return _regions; }
            set
            {
                _regions = value;
            }
        }

        public Video()
        {
        }

        public Video(UInt64 videoId, string title, string format, int length, IList<int> regions)
        {
            this.Id = videoId;
            this.Title = title;
            this.Format = format;
            this.Length = length;
            this.Regions = regions;
        }

        public Video(string csvLine, string fieldSep = ",", string subFieldSep = "|")
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
                Regions = this.SetRegions(fields[idx], subFieldSep);
                idx++;
            }
        }

        public IList<int> SetRegions(string line, string fieldSep = "|")
        {
            string[] fields = line.Split(fieldSep);

            IList<int> regions = new List<int>();
            foreach (string field in fields)
            {
                regions.Add(Int32.Parse(field));
            }

            return regions;
        }

        public override string ToString()
        {
            string retStr = "";
            string title = Title;
            string csv = ",";
            string format = Format;
            int length = Length;
            string regions = string.Join("|", Regions);

            if (title.Contains(csv))
            {
                title = "\"" + title + "\"";
            }

            retStr = $"{Id}{csv}{title}{csv}{format}{csv}{length}{csv}{regions}";

            return retStr;
        }

        public override void Display(bool showHeader)
        {
            if (showHeader)
                Console.WriteLine(this._pageHeader);

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
                _currField = CustomFields.FieldFormat;
            }

            switch (_currField)
            {
                case CustomFields.FieldFormat:
                    string format = "";
                    while (format.Length < 1)
                    {
                        Console.WriteLine(_formatEntry);
                        format = Console.ReadLine();
                    }
                    Format = format;
                    _currField++;
                    break;
                case CustomFields.FieldLength:
                    string sLen = "";
                    int length = 0;
                    while (length < 1)
                    {
                        Console.WriteLine(_lengthEntry);
                        sLen = Console.ReadLine();
                        if (sLen.Length > 0)
                        {
                            try
                            {
                                length = Int32.Parse(sLen);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Entry {sLen} not a valid number!");
                            }
                        }
                        if (length < 1)
                        {
                            Console.WriteLine("Length entry is required to be > 0.  Please try again.");
                        }
                    }
                    Length = length;
                    _currField++;
                    break;
                case CustomFields.FieldRegion:
                default:
                    string region = "";
                    IList<int> regions = new List<int>();
                    Console.WriteLine(_regionTitle);
                    while (region.Length < 1)
                    {
                        Console.WriteLine(_regionEntry);
                        region = Console.ReadLine();
                        if (region.Length > 0)
                        {
                            try
                            {
                                int iRegion = Int32.Parse(region);
                                regions.Add(iRegion);
                                
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine($"Entry {region} not a valid number!");
                            }
                            region = "";
                        }
                        else
                        {
                            region = "Q";
                        }

                        Console.WriteLine();
                    }

                    Regions = regions;
                    done = true;
                    break;
            }

            return done;
        }
    }
}