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

        public Writer(string filesName)
        {
            curFilesName = filesName;
            string[] file = curFilesName.Split('.').ToArray();
            writer = new StreamWriter($"{file[0]}.xml");
            writer.WriteLine($"<table name=”{file[0]}”>");
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
            writer = new StreamWriter($"{file[0]}.xml");
            writer.WriteLine($"<table name=”{file[0]}”>");
        }
        public static void EndFile()
        {
            writer.WriteLine("</table>");
            writer.Close();
        }
    }
}
