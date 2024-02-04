using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataAccessLibrary.Manager
{
    public class LogManager
    {

        private static readonly Lazy<LogManager> lazyInstance =
        new Lazy<LogManager>(() => new LogManager());
        public static LogManager Instance => lazyInstance.Value;

        public static void WriteLogs(string logmessage)
        {
            //string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Environment.GetFolderPath
                            (Environment.SpecialFolder.LocalApplicationData);
            string fileName = "DataAccessLibraryLog.txt";
            string filePath = Path.Combine(directoryPath, fileName);
            Console.WriteLine(filePath);

            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Append)))
            {
                sw.WriteLineAsync(DateTime.Now + " : " + logmessage);
            }
        }
    }
}
