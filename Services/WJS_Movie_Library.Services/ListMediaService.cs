using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJS_Movie_Library.Model;

namespace WJS_Movie_Library.Services
{
    public class ListMediaService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private string _pageTitle = "Movie List:\nMovie Id   Title       Genres";
        private string _pagePromptMore = "Press Enter to Continue.  Press U to Page Up or press Q to Quit";
        private string _pagePromptLast = "Press Enter to Finish.";

        public int StartPos { get; set; }
        public int PageHeight { get; set; }

        public ListMediaService(int pageHeight)
        {
            PageHeight = pageHeight;
            StartPos = 0;
        }

        private int ShowMediaPage ( IMediaService mediaService, bool showCount )
        {
            int lastIndex = 0;
            bool showHeader = true;
            IList<MediaBase> mediaList = mediaService.GetRangeOfMedia(StartPos, PageHeight);
            int totalRecords = 0;
            bool showType = false;

            if (showCount)
            {
                totalRecords = mediaService.GetMediaList().Count;
                Console.WriteLine($"Total Records: {totalRecords}");
                Console.WriteLine();
                showType = true;
            }

            if (mediaList.Count <= 0)
                return lastIndex;

            lastIndex = mediaList.Count + StartPos;
            foreach (MediaBase record in mediaList)
            {
                record.Display(showHeader, showType);
                showHeader = false;
            }
            
            return lastIndex;
        }

        public void ShowMedia ( IMediaService mediaService, bool showCount = false )
        {
            bool quit = false;
            int lastIndex = -1;

            StartPos = 0;

            do
            {
                lastIndex = ShowMediaPage(mediaService, showCount);
                if (lastIndex < PageHeight)
                {
                    Console.WriteLine(_pagePromptLast);
                    Console.ReadLine();
                }
                else
                {
                    int increment = PageHeight;

                    Console.WriteLine(_pagePromptMore);
                    string resp = Console.ReadLine();
                    if (resp.Length > 0)
                    {
                        string respCh = resp.Substring(0, 1).ToUpper();
                        if (respCh.Equals("Q"))
                        {
                            quit = true;
                        }
                        else if (respCh.Equals("U"))
                        {
                            increment = 0 - increment;
                        }
                    }
                    StartPos = StartPos + increment;
                    if (StartPos < 0) StartPos = 0;
                }
            } while (!quit && lastIndex >= PageHeight);            
        }
    }
}
