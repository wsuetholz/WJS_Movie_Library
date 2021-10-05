using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJS_Movie_Library.Services
{
    public class MenuService
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private string _mainTitle = "My Media Library";
        private string _mainCommandOptions = "Please Select Command (A)dd, (L)ist, (S)ave, (Q)uit";
        private string _savePrompt = "Media Data Has Changed\n\nDo you really want to quit?  (Y)es or (N)o:";

        public enum MainMenuCommandOptions { Unset, Add, List, Save, Quit };

        public IList<string> MediaTypes { get; set; }

        public MenuService()
        {
            MediaTypes = new List<string>();
        }

        public void AddMediaType(string newType)
        {
            IList<string> mediaTypes = MediaTypes;
            mediaTypes.Add(newType);
            MediaTypes = mediaTypes;
        }

        public string PromtMediaType()
        {
            string sel = "";

            int maxIdx = 0;
            
            foreach (string mediaType in MediaTypes)
            {
                Console.WriteLine($"{++maxIdx}. {mediaType}");
            }

            if (maxIdx > 0)
            {
                string sSel = "";
                int selIdx = 0;
                while (selIdx < 1)
                {
                    Console.WriteLine("Select Media Type: ");
                    sSel = Console.ReadLine();
                    if (sSel.Length > 0)
                    {
                        try
                        {
                            selIdx = Int32.Parse(sSel);
                            if (selIdx > maxIdx)
                            {
                                selIdx = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            selIdx = 0;
                        }
                        if (selIdx < 1)
                        {
                            Console.WriteLine($"Entry {sSel} not within 1 .. {maxIdx}");
                        }
                    }
                }

                if (selIdx > 0)
                {
                    --selIdx;
                    sel = MediaTypes[selIdx];
                }
            }

            return sel;
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

        public bool PromptOkToCloseWithoutSave()
        {
            bool ret = false;
            bool answered = false;

            while (!answered)
            {
                Console.WriteLine(_savePrompt);
                string resp = Console.ReadLine();
                if (resp.Length > 0)
                {
                    string respCh = resp.Substring(0, 1).ToUpper();
                    if (respCh.Equals("Y"))
                    {
                        answered = true;
                        ret = true;
                    }
                    else if (respCh.Equals("N"))
                    {
                        answered = true;
                        ret = false;
                    }
                }
            }

            return ret;
        }
    }
}
