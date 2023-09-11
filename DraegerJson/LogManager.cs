using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DraegerJson
{
    public class LogManager
    {
        private static string logFileName = "log.txt";
        public static void SetUp(string logfilename) 
        {
            logFileName = logfilename;
        }
        public static void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(logFileName, FileMode.Append)))
            {
                writer.WriteLine($"[{DateTime.Now}]: {message}");
            }
            Console.WriteLine(message);
        }
    }
}
