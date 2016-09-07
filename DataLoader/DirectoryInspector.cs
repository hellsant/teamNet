using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    public class DirectoryInspector
    {
        public DirectoryInspector()
        {
        }

        public IList<string> GetNameFiles()
        {
            IList<string> result = new List<string>();
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo("Results/");
            System.IO.FileInfo[] fileNames = dirInfo.GetFiles();
            foreach (System.IO.FileInfo file in fileNames)
            {
                result.Add(file.ToString());
            }
            return result;
        }
    }
}
