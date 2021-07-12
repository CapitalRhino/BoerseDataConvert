using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public static class WarningStat
    {
        private static Dictionary<int, int> tableWarnings = tableWarnings = new Dictionary<int, int>();
        private static string curFile;
        public static void Refresh(string fileName)
        {
            curFile = fileName;
            tableWarnings = new Dictionary<int, int>();
        }
        /*
         type "inv" - Ivalid tag
         type "long" - too long string value
         type "notnum" - is not in a valid format for number
         type "range" - Value not in range 
        */
        public static void Add(int tag)//
        {
            if (tableWarnings.ContainsKey(tag))
            {
                tableWarnings[tag]++;
            }
            else
            {
                tableWarnings.Add(tag, 1);
            }
        }
        public static void PrintWarnigs()
        {
            if (tableWarnings.Count == 0)
            {
                Console.WriteLine($"There is no warnings in file {curFile}");
            }
            else
            {
                Console.WriteLine($"Warnings in file {curFile}");
                foreach (var warning in tableWarnings)
                {
                    int tag = warning.Key;
                    int count = warning.Value;
                    Console.WriteLine($"Invalid tag \"{tag}\": {count} times.");
                }
            }
        }
    }
}

