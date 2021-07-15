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
        /// <summary>
        /// it create a reader for first file, start the StopWatch and Checks the first line from the current file
        /// </summary>
        /// <param name="adr">input directoty</param>
        /// <param name="_filesNames">array with the names of the every TXT file</param>
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
        /// <summary>
        /// It reads a line from the current file and return a Record with this information
        /// and it goes to next file if it the current file ends
        /// </summary>
        /// <returns>Record with information from 1 line from current file</returns>
        public Record ReadLineRecord()
        {
            string s = reader.ReadLine();
            if (reader.EndOfStream)
            {
                s = NextFile(s);
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
        /// <summary>
        /// it checks the last line from the current file, closes the reader, create a new reader for he file
        /// also it check first line from the next file and synchronizes a current file in RecordController with its current file
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string NextFile(string s)
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
            return s;
        }
        /// <summary>
        /// it checks if string date is in the correct format
        /// </summary>
        /// <param name="date">first line from a file</param>
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
        /// <summary>
        /// Checks if string s is in the correct format
        /// </summary>
        /// <param name="s">last line from a file</param>
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
        /// <summary>
        /// Closes the reader, stops the stopwatch,print the its time, resets it and start it again
        /// </summary>
        internal static void EndFile()
        {
            reader.Close();
            stopwatch.Stop();
            Console.WriteLine($"INFO: { filesNames[fileInd] } was converted successfully in {stopwatch.Elapsed:c}");
            stopwatch.Reset();
        }
    }
}
