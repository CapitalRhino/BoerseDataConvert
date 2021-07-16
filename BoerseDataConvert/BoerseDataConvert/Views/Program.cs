using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Mono.Options;
using System.Diagnostics;

namespace BoerseDataConvert
{
    class Program
    {
        static string zipFile;
        static string inputDirectory;
        static string outputDirectory;
        static string tagsFile;
        static bool helpMessage;
        static bool fastCheck;
        static void Main(string[] args)
        {
            /* 
            FileStream ostrm;
            StreamWriter writer1;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream($"./{DateTime.Now.ToString("dd-HH-mm-ss")}.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer1 = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer1);
            */

            // input handling
            var p = new OptionSet() {
                "BoerseDataConvert Version 1.0.0",
                "---",
                "Usage: BoerseDataConvert [OPTIONS]+",
                "Tool which converts from Boerse Stuttart format to XML",
                "",
                "Flags:",
                { "?|h|help", "prints help message", x => helpMessage = true },
                { "i|input=", "specify input zip file", x => zipFile = x },
                { "d|directory=", "specify working directory", x => inputDirectory = x },
                "The working directory is cleared recursively if it isn't empty!",
                { "o|output=", "specify output directory", x => outputDirectory = x },
                { "t|tags=", "specify tag file", x =>  tagsFile = x },
                { "f|fast|fastcheck", "fastcheck mode", x =>  fastCheck = true },
                { "<>", v => throw new ArgumentException("ERROR: Invalid arguments") }, // default
                "",
                "Created by D. Delchev and D. Byalkov, 2021"
            };

            if (tagsFile == null)
            {
                tagsFile = "tags.txt";
            }

            try
            {
                p.Parse(args);
                if (helpMessage)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    Environment.Exit(0);
                }
                CheckFreeDisk(zipFile, outputDirectory);
                ZipExtract(zipFile, inputDirectory);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(3);
            }

            // only read filenames
            string[] fileNames = Directory.GetFiles(inputDirectory).Select(x => x.Split('\\', '/').Last()).ToArray();

            Reader reader = new Reader(inputDirectory, fileNames);
            RecordController a = new RecordController(outputDirectory, fileNames[0], tagsFile);
            Stopwatch totalRunTime = new Stopwatch();
            totalRunTime.Start();
            while (true)
            {
                try
                {
                    Record record = reader.ReadLineRecord();
                    a.WriteXmlRecord(record,fastCheck);
                }
                catch (IndexOutOfRangeException)
                {
                    RecordController.EndFile();
                    break;
                }
            }
            totalRunTime.Stop();
            Console.WriteLine($"INFO: Successful conversion in {totalRunTime.Elapsed:c}, exiting");
            Environment.Exit(0);
        }
        static void CheckFreeDisk(string zipFile, string outputDirectory)
        {
            double gibibyte = 1073741824;
            DriveInfo driveInfo = new DriveInfo(Directory.GetDirectoryRoot(outputDirectory));
            double availableSpace = driveInfo.AvailableFreeSpace;
            FileInfo fi = new FileInfo(zipFile);
            double checkedSpace = fi.Length * 100; // lazy but given the file sizes it's
                                                   // better to overestimate than to under-.
            if (availableSpace < checkedSpace)
            {
                Environment.ExitCode = 3;
                throw new IOException($"ERROR: Estimated required space: {checkedSpace / gibibyte:f2}GiB. Available: {availableSpace / gibibyte:f2}GiB");
            }
            Console.WriteLine($"INFO: {availableSpace / gibibyte:f2}GiB available");
        }
        static void ZipExtract(string zipFile, string inputDirectory)
        {
            if (Directory.Exists(inputDirectory))
            {
                Directory.Delete(inputDirectory, true);
            }
            Directory.CreateDirectory(inputDirectory);
            ZipFile.ExtractToDirectory(zipFile, inputDirectory, true); // zip extract
            Console.WriteLine("INFO: Successful ZIP extraction");
        }
    }
}
