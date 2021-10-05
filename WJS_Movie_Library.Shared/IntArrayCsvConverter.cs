using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJS_Movie_Library.Shared
{
    public class IntArrayCsvConverter : TypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] allElements = text.Split("|");
            int[] allElementsAsInt = allElements.Select(s => int.Parse(s)).ToArray();
            return new List<int>(allElementsAsInt);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return string.Join("|", ((List<int>)value).ToArray());
        }

    }
}
