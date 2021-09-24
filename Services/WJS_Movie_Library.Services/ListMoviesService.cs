using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJS_Movie_Library.Model;

namespace WJS_Movie_Library.Services
{
    public class ListMoviesService
    {
        private string _pageTitle = "Movie List:\nMovie Id   Title       Genres";
        private string _pagePromptMore = "Press Enter to Continue.  Press U to Page Up or press Q to Quit";
        private string _pagePromptLast = "Press Enter to Finish.";

        public int StartPos { get; set; }
        public int PageHeight { get; set; }

        public ListMoviesService(int pageHeight)
        {
            PageHeight = pageHeight;
            StartPos = 0;
        }

        private int ShowMoviePage ( MovieService movieService )
        {
            int lastIndex = 0;
            IList<Movie> movies = movieService.GetRangeOfMovies(StartPos, PageHeight);

            if (movies.Count <= 0)
                return lastIndex;

            lastIndex = movies.Count + StartPos;
            foreach (Movie movie in movies)
            {
                Console.WriteLine(movie.ToString());
            }
            
            return lastIndex;
        }

        public void ShowMovies ( MovieService movieService )
        {
            bool quit = false;
            int lastIndex = -1;

            StartPos = 0;

            do
            {
                lastIndex = ShowMoviePage(movieService);
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
