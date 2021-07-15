using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    /// <summary>
    /// This class strores information about a invalid tags in a file
    /// </summary>
    public  class WarningStat
    {
        private Dictionary<int, int> tableWarnings;
        private  string curFile;
        public WarningStat(string fileName)
        {
            curFile = fileName;
            tableWarnings = new Dictionary<int, int>();
        }
        /// <summary>
        /// It increases count of encounters if the invalid tag or it sets it to 1 if the tag is new
        /// </summary>
        /// <param name="tag">Invalid tag in numerical form</param>
        public void Add(int tag)
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
        /// <summary>
        /// It prints the count of encounters of every invalid tag in the file
        /// </summary>
        public  void PrintWarnigs()
        {
            if (tableWarnings.Count == 0)
            {
                Console.WriteLine($"INFO: No warnings in {curFile}");
            }
            else
            {
                Console.Error.WriteLine($"WARN: Warnings in {curFile}");
                foreach (var warning in tableWarnings)
                {
                    int tag = warning.Key;
                    int count = warning.Value;
                    Console.Error.WriteLine($"WARN: Invalid tag \"{tag}\": {count} times.");
                }
            }
        }
    }
}

