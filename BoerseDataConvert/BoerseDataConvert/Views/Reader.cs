using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BoerseDataConvert
{
    public class Reader
    {
        static Stopwatch stopwatch = new Stopwatch();
        private static string[] filesNames;
        private static StreamReader reader;
        private static int fileInd;
        private string adr;
        public Reader(string adr, string[] _filesNames)
        {
            fileInd = 0;
            reader = new StreamReader($@"{adr}/{_filesNames[fileInd]}", CodePagesEncodingProvider.Instance.GetEncoding(1252));
            this.adr = adr;
            filesNames = _filesNames;
            string date = reader.ReadLine();
            stopwatch.Start();
            CheckFirstLine(date);
        }
        public Record ReadLineRecord()
        {
            string s = reader.ReadLine();
            if (reader.EndOfStream)
            {
                CheckFinalLine(s);
                EndFile();
                fileInd++;
                stopwatch.Start();
                reader = new StreamReader($@"{adr}/{filesNames[fileInd]}", CodePagesEncodingProvider.Instance.GetEncoding(1252));
                RecordController.NextFile(filesNames[fileInd]);
                string date = reader.ReadLine();
                CheckFirstLine(date);
                s = reader.ReadLine();
            }
            string[] sr = s.Split("|").ToArray();
            Record record = new Record();
            foreach (var item in sr)
            {
                string[] d = item.Split('#', 2).ToArray();
                record.Add(int.Parse(d[0]), d[1]);
            }
            return record;
        }
        private void CheckFirstLine(string date)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime time = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", provider);
            }
            catch (Exception)
            {

                Console.WriteLine($"WARN: Invalid date in file {filesNames[fileInd]}");
            }
        }
        private void CheckFinalLine(string s)
        {
            if ("Datensaetze: " != s.Substring(0, 13))
            {
                Console.WriteLine($"wARN: Final line on file {filesNames[fileInd]} is not in th correct format!");
            }
            else
            {
                int count = 0;
                try
                {
                    string countStr = s.Split(' ', 2).Last();
                    count = int.Parse(countStr);
                    if (count != RecordController.Count)
                    {
                        Console.Error.WriteLine($"WARN: Count on file {filesNames[fileInd]} is not correct! Real count is {RecordController.Count}");
                    }
                }
                catch (Exception)
                {
                    Console.Error.WriteLine($"WARN: Count on file {filesNames[fileInd]} is not in the correct format");
                }

            }
        }
        internal static void EndFile()
        {
            reader.Close();
            stopwatch.Stop();
            Console.WriteLine($"INFO: { filesNames[fileInd] } was converted successfully in {stopwatch.Elapsed:c}");
            stopwatch.Reset();
        }
    }
}
