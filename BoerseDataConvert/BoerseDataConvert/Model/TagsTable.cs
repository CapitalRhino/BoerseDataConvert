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
        Tag[] table;//name,isSring,stringLength,
        public TagsTable(string tags)
        {
            reader = new StreamReader(tags);
            table = new Tag[int.Parse(reader.ReadLine()) + 1];
            LoadInfo();
        }
        private void LoadInfo()
        {
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split('|').ToArray();
                    table[int.Parse(line[0])]= new Tag(line.Skip(1).ToArray());
                }
            }
        }
        public bool CheckInvalidTag(int tag)
        {
            return table[tag]==null;
        }
        public string GetTagName(int tag)
        {        
            return table[tag].Name;
        }
        public bool IsString(int tag)
        {
            return table[tag].IsString;
        }
        public bool HaveValueRanges(int tag)
        {
            return table[tag].HaveValueRanges;
        }
        public int GetMaxValueLength(int tag)
        {
            return table[tag].StringLength;
        }
        public bool CheckStringLengthToBig(int tag, int value)
        {
            return table[tag].StringLength<value;
        }
        public bool CheckValidValue(int tag, string value)
        { 
            return table[tag].ValidValue(value);
        }
    }
}
