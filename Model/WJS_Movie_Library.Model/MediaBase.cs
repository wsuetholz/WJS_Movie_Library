using System;
using CsvHelper;
using WJS_Movie_Library.Model.Interfaces;

namespace WJS_Movie_Library.Model
{
    public abstract class MediaBase : IMedia
    {
        public UInt64 Id { get; set; }
        public string Title { get; set; }
        public abstract string MediaTypeName { get; }

        public abstract void Display(bool showHeader, bool showType = false);

        public abstract void RegisterCSVMap(CsvContext context);
        public abstract bool EnterCustomField(bool first);

        public MediaBase()
        {

        }
    }
}