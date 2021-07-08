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
            foreach (var tagValue in record.TagsValues)
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
        private string CheckTagValue(string tag, string value)
        {
            string[] tagLine;
            try
            {
                tagLine = tagsTable.GetTagValue(tag);
            }
            catch (KeyNotFoundException)
            {

                throw new ArgumentException($"WARN: Invalid tag \"{tag}\", {cur_fileName} line {count + 1}");
            }
            string tagname = tagLine[0];
            if (value != "NULL" && tagLine.Length == 2)
            {
                string[] valueType = tagLine[1].Split('-').ToArray();
                if (valueType.Length == 2)
                {
                    if (value.Length > int.Parse(valueType[1]))
                    {
                        throw new ArgumentException($"WARN: Too long value \"{tag}\", \"{value}\", max allowed \"{valueType[1]}\", {cur_fileName} line {count + 1}");
                    }
                }
                else
                {
                    if (value == "") return tagname;
                    try
                    {
                        double.Parse(value);
                    }
                    catch (FormatException)
                    {
                        throw new ArgumentException($"WARN: Value is not in a valid format for number \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");
                    }

                }
            }
            else
            {
                string[] valueRange = tagLine[2].Split('#').ToArray();
                bool countain = false;
                if (value == "") return tagname;
                for (int i = 0; i < valueRange.Length; i++)
                {
                    if (valueRange[i] == value)
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

