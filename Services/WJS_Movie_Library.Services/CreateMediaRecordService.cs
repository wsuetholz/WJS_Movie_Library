using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJS_Movie_Library.Model;
using WJS_Movie_Library.Model.Interfaces;

namespace WJS_Movie_Library.Services
{
    public class CreateMediaRecordService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private string _pageTitle = "Enter New Data";
        private string _movieTitle = "Title: ";
        private string _duplicateTitle = "Duplicate Title.  Please Try again.";
        private string _validateMedia = "Press A to add the following information";

        public CreateMediaRecordService()
        {

        }

        public UInt64 CreateRecord<T>( CsvMediaService<T> mediaService ) where T: MediaBase, new()
        {
            T newRecord = new();

            Console.WriteLine();
            Console.WriteLine($"{_pageTitle} for {newRecord.MediaTypeName}.");

            string mediaTitle = "";
            while (mediaTitle.Length < 1)
            {
                Console.WriteLine($"{newRecord.MediaTypeName} {_movieTitle}");
                mediaTitle = Console.ReadLine();
                if (mediaTitle.Length > 0)
                {
                    if (mediaService.Exists(mediaTitle))
                    {
                        Console.WriteLine(_duplicateTitle);
                        mediaTitle = "";
                    }
                }
                Console.WriteLine();
            }

            newRecord.Title = mediaTitle;

            bool firstOne = true;
            while ( !newRecord.EnterCustomField(firstOne) )
            {
                firstOne = false;
                Console.WriteLine();
            }

            Console.WriteLine(_validateMedia);
            newRecord.Display(false);
            string resp = Console.ReadLine();
            if (resp.Length > 0)
            {
                string respCh = resp.Substring(0, 1).ToUpper();
                if (respCh.Equals("A"))
                {
                    newRecord.Id = mediaService.GetMaxId() + 1;
                    mediaService.Add(newRecord);
                }
                else
                {
                    Console.WriteLine("Skipped!");
                }
            }

            return newRecord.Id;
        }
    }
}
