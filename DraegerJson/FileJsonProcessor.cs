using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draft_Draeger
{
    //class inherits from JsonProcessor -method
    public class FileJsonProcessor : IJsonProcessor 
    {
        private string dataPath;
        public FileJsonProcessor(string dataPath) 
        {
            this.dataPath = dataPath;
            CheckJsonDirectory();
        }

        public void ProcessJson(string obj)
        {
            using (StreamWriter writer = new StreamWriter(dataPath))
            {
                writer.WriteLine(obj);
            }
        }

        private void CheckJsonDirectory()
        {
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath)!);
            }
        }
    }
}
