using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BoerseDataConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            // Args format
            // -i *.zip or --input *.zip
            // -d directory or --dir directory
            // -o directory or --output direcory
            // -h - help
            Console.WriteLine();
            Reader reader = new Reader(@"E:\Downloads\TestData-2021_07_02", new string[1] { "subtype910.txt" });
            RecordController a = new RecordController("");
            while (true)
            {
                try
                {
                    Record record = reader.ReadLineRecord();
                    Console.WriteLine(a.ConvertToXml(record));
                }
                catch (IndexOutOfRangeException e)
                {
                    break;
                }
                
            }
        }
    }
}
