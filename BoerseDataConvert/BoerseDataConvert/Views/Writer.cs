using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public class Writer
    {
        private static string curFilesName;
        private static StreamWriter writer;
        private static string address;

        public Writer(string _address, string filesName)
        {
            address = _address;
            curFilesName = filesName;
            string[] file = curFilesName.Split('.').ToArray();
            writer = new StreamWriter($@"{address}/{file[0]}.xml");
            writer.WriteLine($"<table name=\"{file[0]}\">");
            
        }
        public void WriteRecord(string record)
        {
            writer.WriteLine(record);
        }
        public static void NextFile(string fileName)
        {
            EndFile();
            curFilesName = fileName;
            string[] file = curFilesName.Split('.').ToArray();
            writer = new StreamWriter($@"{address}/{file[0]}.xml");
            writer.WriteLine($"<table name=\"{file[0]}\">");
        }
        public static void EndFile()
        {
            writer.WriteLine("</table>");
            writer.Close();
        }
    }
}
