using System.IO;

// git clone <something> <new dir name>
namespace WJS_Movie_Library.Shared
{
    public class BuildDataFileName
    {
        public static string BuildDataFilePath(string fName, string myExtension)
        {
            string fPath = "";
            string dataDir = "Data";
            string baseName = Path.GetFileNameWithoutExtension(fName);
            var currDir = Directory.GetParent(Directory.GetCurrentDirectory());
            while (!Directory.Exists(Path.Combine(currDir.FullName, dataDir)))
            {
                currDir = currDir.Parent;
            }

            fPath = Path.Combine(currDir.FullName, dataDir, baseName) + myExtension;

            return fPath;
        }
    }
}
