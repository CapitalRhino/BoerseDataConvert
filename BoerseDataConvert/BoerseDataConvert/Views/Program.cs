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
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer1);
            

            // input handling
            string[] input = InputValidate(args);
            string zipFile = input[0], inputDir = input[1], outputDir = input[2], tags = input[3];

            CheckFreeDisk(outputDir);
            ZipExtract(zipFile, inputDir);

            // only read filenames
            string[] fileNames = Directory.GetFiles(inputDir).Select(x => x.Split('\\', '/').Last()).ToArray();

            Reader reader = new Reader(inputDir, fileNames);
            Writer writer = new Writer(outputDir, fileNames[0]);
            RecordController a = new RecordController(fileNames[0], tags);

            while (true)
            {
                try
                {
                    Record record = reader.ReadLineRecord();
                    string s = a.ConvertToXml(record);
                    writer.WriteRecord(s);
                }
                catch (IndexOutOfRangeException)
                {
                    Reader.EndFile();
                    Writer.EndFile();
                    break;
                }
            }
            Console.WriteLine("INFO: Success, exiting");
            Environment.Exit(0);
        }
        static void Help()
        {
            Console.WriteLine("BoerseDataConvert v1.0.0");
            Console.WriteLine("D. Delchev and D. Byalkov, 2021");
            Console.WriteLine("---");
            Console.WriteLine("-i <input zip file> or --input <input zip file>");
            Console.WriteLine("-d <working directory> or --directory <working directory>");
            Console.WriteLine("-o <output directory> or --output <output directory>");
            Console.WriteLine("-t <tags file> or --tags <tags file>");
            Console.WriteLine("-h or --help - Prints this message");
            Environment.Exit(0);
        }
        static void CheckFreeDisk(string outputDir)
        {
            double gibibyte = 1073741824;
            DriveInfo driveInfo = new DriveInfo(Directory.GetDirectoryRoot(outputDir));
            double availableSpace = driveInfo.AvailableFreeSpace;
            Console.WriteLine($"INFO: {availableSpace / gibibyte}GiB available");
            if (availableSpace < 2147483648)
            {
                Environment.ExitCode = 3;
                throw new IOException($"ERROR: Insufficient disk space, {availableSpace / gibibyte}GiB less than 2GiB");
            }
        }
        static string[] InputValidate(string[] args)
        {
            string[] output = new string[4];
            // check input zipfile parameter
            if (args.Contains("-i"))
            {
                output[0] = args[Array.IndexOf(args, "-i") + 1];
            }
            if (args.Contains("--input"))
            {
                output[0] = args[Array.IndexOf(args, "--input") + 1];
            }
            // check input directory parameter
            if (args.Contains("-d"))
            {
                output[1] = args[Array.IndexOf(args, "-d") + 1];
            }
            if (args.Contains("--directory"))
            {
                output[1] = args[Array.IndexOf(args, "--directory") + 1];
            }
            // check output directory parameter
            if (args.Contains("-o"))
            {
                output[2] = args[Array.IndexOf(args, "-o") + 1];
            }
            if (args.Contains("--output"))
            {
                output[2] = args[Array.IndexOf(args, "-output") + 1];
            }
            if (args.Contains("-t"))
            {
                output[3] = args[Array.IndexOf(args, "-t") + 1];
            }
            if (args.Contains("--tags"))
            {
                output[3] = args[Array.IndexOf(args, "--tags") + 1];
            }
            if (!args.Contains("-t") || !args.Contains("--tags"))
            {
                output[3] = "tags.txt";
            }
            // check help parameter
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Help();
            }
            // Checks if somehow the flags get in the output and if
            // the output string are null. Prevents invalid paths.
            if (output.Contains("-i") || output.Contains("--input") ||
                output.Contains("-d") || output.Contains("--directory") ||
                output.Contains("-o") || output.Contains("--output") ||
                output.Contains("-h") || output.Contains("--help") || 
                output[0] == null || output[1] == null || output[2] == null)
            {
                throw new ArgumentException("ERROR: Parameters cannot be empty");
            }
            // returns the strings
            return output;
        }
        static void ZipExtract(string zipFile, string inputDir)
        {
            Directory.Delete(inputDir, true);
            Directory.CreateDirectory(inputDir);
            ZipFile.ExtractToDirectory(zipFile, inputDir, true); // zip extract
            Console.WriteLine("INFO: Successful ZIP extraction");
        }
    }
}
