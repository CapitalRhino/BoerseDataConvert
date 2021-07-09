using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public class RecordController
    {
        private static int count;
        private static string cur_fileName;
        private TagsTable tagsTable;
        public RecordController(string fileName, string tags)
        {
            count = 1;
            cur_fileName = fileName;
            tagsTable = new TagsTable(tags);
        }
        public static void NextFile(string fileName)
        {
            count = 1;
            cur_fileName = fileName;
        }
        public string ConvertToXml(Record record)
        {
            StringBuilder xmlRecord = new StringBuilder();
            xmlRecord.Append($"	<record id=\"{count}\">\n");
            foreach (var tagValue in record)
            {
                try
                {
                    string tag = CheckTagValue(tagValue.Key, tagValue.Value);
                    xmlRecord.Append($"		<{tag}>{tagValue.Value}</{tag}>\n");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            xmlRecord.Append($"	</record>");
            count++;
            return xmlRecord.ToString();
        }
        private string CheckTagValue(int tag, string value)
        {
            
            if(tagsTable.CheckInvalidTag(tag)) throw new ArgumentException($"WARN: Invalid tag \"{tag}\", {cur_fileName} line {count + 1}");
            string tagname = tagsTable.GetTagName(tag);
            if (value != "NULL" && tagsTable.HaveValueRanges(tag))//Checks if the tag have not a value ranges
            {
                if (tagsTable.IsString(tag) && tagsTable.CheckStringLength(tag,value.Length))//Checks if value type is string
                {                   
                    throw new ArgumentException($"WARN: Too long value \"{tag}\", \"{value}\", max allowed \"{tagsTable.GetTagName(tag)}\", {cur_fileName} line {count + 1}");
                }
                else//Checks if value type is decimal
                {
                    if (value == "") return tagname;
                    try//Checks if the value is in the correct format
                    {
                        double.Parse(value, new System.Globalization.CultureInfo("de-DE"));
                    }
                    catch (FormatException)
                    {
                         throw new ArgumentException($"WARN: Value is not in a valid format for number \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");   
                    }
                }
            }
            else//Checks if the tag have  a value ranges
            {
                string[] valueRange = tagLine[2].Split('#').ToArray();
                bool countain = false;
                if (value == "") return tagname;
                for (int i = 0; i < valueRange.Length; i++)
                {
                    if (valueRange[i] == value)//Checks if the value is in value ranges
                    {
                        countain = true;
                        break;
                    }
                }
                if (!countain)
                {
                    throw new ArgumentException($"WARN: Value not in range \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");
                }
            }
            return tagname;
        }

    }
}

