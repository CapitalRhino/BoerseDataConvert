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
            // Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine();
            if (args.Contains("-i") || args.Contains("--input"))
            {
                string zipfile = args[Array.IndexOf(args, "-i") + 1];
            }
            if (args.Contains("-d") || args.Contains("--directory"))
            {
                string inputDir = args[Array.IndexOf(args, "-d" + 1)];
            }
            if (args.Contains("-o") || args.Contains("--output"))
            {
                string outputDir = args[Array.IndexOf(args, "-o" + 1)];
            }
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Help();
                return;
            }
            // Reader reader = new Reader(@"E:\Downloads\TestData-2021_07_02", new string[1] { "subtype910.txt" });
            Reader reader = new Reader(@"D:\Code\ИТ Кариера\Стаж\задача\TestData-2021_07_02", new string[1] { "subtype910.txt" });
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
        static void Help()
        {
            Console.WriteLine("BoerseDataConvert v1.0.0");
            Console.WriteLine("D. Delchev and D. Byalkov, 2021");
            Console.WriteLine("---");
            Console.WriteLine("-i <input zip file> or --input <input zip file>");
            Console.WriteLine("-d <working directory> or --directory <working directory>");
            Console.WriteLine("-o <output directory> or --output <output directory>");
        }
    }
}
