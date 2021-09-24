using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJS_Movie_Library.Services
{
    public class MainMenuService
    {
        private string _mainTitle = "My Movie Library";
        private string _mainCommandOptions = "Please Select Command (A)dd, (L)ist, (S)ave, (Q)uit";

        public enum MainMenuCommandOptions { Add, List, Save, Quit };

        public MainMenuService()
        {

        }

        public MainMenuCommandOptions Prompt(bool showTitle = false)
        {
            MainMenuCommandOptions ret = MainMenuCommandOptions.Quit;
            bool commandSet = false;

            if (showTitle)
            {
                Console.WriteLine(_mainTitle);
                Console.WriteLine();
            }

            while (!commandSet)
            {
                Console.WriteLine(_mainCommandOptions);
                string resp = Console.ReadLine().ToUpper();
                if (resp.Length > 0)
                {
                    string respCh = resp.Substring(0, 1);
                    if (respCh.Equals("A"))
                    {
                        ret = MainMenuCommandOptions.Add;
                        commandSet = true;
                    }
                    else if (respCh.Equals("L"))
                    {
                        ret = MainMenuCommandOptions.List;
                        commandSet = true;
                    }
                    else if (respCh.Equals("S"))
                    {
                        ret = MainMenuCommandOptions.Save;
                        commandSet = true;
                    }
                    else if (respCh.Equals("Q"))
                    {
                        ret = MainMenuCommandOptions.Quit;
                        commandSet = true;
                    }
                }
            }

            return ret;
        }
    }
}
