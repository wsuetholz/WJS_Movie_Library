using CsvHelper;

namespace WJS_Movie_Library.Model.Interfaces
{
    public interface IMedia
    {
        ulong Id { get; set; }
        string Title { get; set; }
        string MediaTypeName { get; }

        void RegisterCSVMap(CsvContext context);
        void Display(bool showHeader, bool showType = false);
        bool EnterCustomField(bool first);
    }
}