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
        private static int count = 1;
        public string ConvertToXml(Record record)
        {
            StringBuilder xlmRecord = new StringBuilder();
            xlmRecord.Append($"<record id=”{count}”>\n");
            foreach (var tagValue in record.TagsValues)
            {
                try
                {
                    string tag =CheckTagValue(tagValue.Key, tagValue.Value);
                    xlmRecord.Append($"<{tag}>{tagValue.Value}</{tag}>\n");
                }
                catch (ArgumentException e)
                {

                    throw new ArgumentException(e.Message);
                }
            }
            xlmRecord.Append($"</record>");
            count++;
            return xlmRecord.ToString();
        }
        private string CheckTagValue(string tag, string value)
        {
            string tagname="";
            StreamReader reader = new StreamReader(@"..\..\..\..\tags.txt");
            using (reader)
            {
                string[] tagLine= null ;
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split('|').ToArray();
                    if (line[0] == tag) tagLine = line;
                }
                if (tagLine == null) throw new ArgumentException("Invalid tag!");
                tagname = tagLine[1];
                if (value != "NULL" && tagLine.Length == 3)
                {
                    string[] valueType = tagLine[2].Split('-').ToArray();
                    if (valueType.Length == 2)
                    {
                        if (value.Length > int.Parse(valueType[1])) throw new ArgumentException("Value is too long!");
                    }
                    else
                    {
                        try
                        {
                            double.Parse(value);
                        }
                        catch (FormatException)
                        {
                            throw new ArgumentException("Value does not represent a number in a valid format!");
                        }

                    }
                }
                else 
                {
                    string[] valueRange = tagLine[3].Split('#').ToArray();
                    bool countain = false;
                    for (int i = 0; i < valueRange.Length; i++)
                    {
                        if (valueRange[i] == value)
                        {
                            countain = true;
                            break;
                        }
                    }
                    if(!countain) throw new ArgumentException("Value is not in value range!");
                }      
            }
            return tagname;
        }
    }
}
