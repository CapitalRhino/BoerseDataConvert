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

            FileStream ostrm;
            StreamWriter writer1;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("./log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer1 = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer1);

            // input handling
            string zipFile = "", inputDir = "", outputDir = "";
            Console.WriteLine();
            if (args.Contains("-i") || args.Contains("--input"))
            {
                zipFile = args[Array.IndexOf(args, "-i") + 1];
            }
            if (args.Contains("-d") || args.Contains("--directory"))
            {
                inputDir = args[Array.IndexOf(args, "-d") + 1];
            }
            if (args.Contains("-o") || args.Contains("--output"))
            {
                outputDir = args[Array.IndexOf(args, "-o") + 1];
            }
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Help();
                return;
            }
            zipFile = @"D:\Code\ИТ Кариера\Стаж\задача\testdata.zip";
            inputDir = @"D:\Code\ИТ Кариера\Стаж\задача\inputdir";
            outputDir = @"D:\Code\ИТ Кариера\Стаж\задача\outputdir";
            if (zipFile == "" || inputDir == "" || outputDir == "")
            {
                throw new ArgumentException("Fields cannot be empty");
            }

            // TODO: clear matching files from inputDir
            // TODO: check free disk space before file ops

            ZipFile.ExtractToDirectory(zipFile, inputDir); // zip extract

            // read files
            string[] fileNames = Directory.GetFiles(inputDir).Select(x => x.Split('\\', '/').Last()).ToArray();

            Reader reader = new Reader(inputDir, fileNames);
            Writer writer = new Writer(outputDir, fileNames[0]);
            RecordController a = new RecordController(fileNames[0]);

            while (true)
            {
                try
                {
                    Record record = reader.ReadLineRecord();
                    string s = a.ConvertToXml(record);
                    writer.WriteRecord(s);
                }
                catch (IndexOutOfRangeException e)
                {
                    Reader.EndFile();
                    Writer.EndFile();
                    break;
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
                Console.WriteLine("-h or --help - Prints this message");
            }
        }
    }
}