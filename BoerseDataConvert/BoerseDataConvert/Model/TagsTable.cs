using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public class TagsTable
    {
        StreamReader reader ;
        Dictionary<string, string[]> table;
        public TagsTable()
        {
            reader = new StreamReader(@"..\..\..\..\tags.txt");
            table = new Dictionary<string, string[]>();
            LoadInfo();
        }
        private void LoadInfo()
        {
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split('|').ToArray();
                    table.Add(line[0], line.Skip(1).ToArray());
                }
            }
        }
        public string[] GetTagValue(string tag)
        {
            return table[tag];
        }
    }
}
