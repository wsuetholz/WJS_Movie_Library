using System;
using System.IO;

namespace WJS_Movie_Library.Shared
{
    public class BuildDataFileName
    {
        public static string BuildDataFilePath(string fName, string myExtension)
        {
            string fPath = "";
            string appBaseDirectory = "WJS_Movie_Library";  // Project Base Directory

            //string dataPath = $"{AppContext.BaseDirectory}..\\..\\..\\Data\\";
            string baseName = Path.GetFileNameWithoutExtension(fName);

            foreach (string part in AppContext.BaseDirectory.Split("\\"))
            {
                fPath = fPath + part + Path.DirectorySeparatorChar;
                if (part.Equals(appBaseDirectory))
                    break;
            }

            fPath = fPath + "Data" + Path.DirectorySeparatorChar + baseName + myExtension;

            return fPath;
        }
    }
}
