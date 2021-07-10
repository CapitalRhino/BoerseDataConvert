using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoerseDataConvert
{
    public class RecordController
    {
        private static int count;
        private static string cur_fileName;
        private TagsTable tagsTable;
        private static string address;
        private static XmlWriter writer;
        public RecordController(string adr,string fileName, string tags)
        {
            count = 1;
            address = adr;
            cur_fileName = fileName.Split('.').First();
            Directory.CreateDirectory(address);
            tagsTable = new TagsTable(tags);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create($@"{address}/{cur_fileName}.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", null, "file");
        }
        public static void NextFile(string fileName)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            cur_fileName = fileName.Split('.').First();
            count = 1;
            cur_fileName = fileName;
            writer.WriteStartDocument();
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", null, "file");
        }
        public void WriteXmlRecord (Record record)
        {
            writer.WriteStartElement("record");
            writer.WriteAttributeString("id", null, count.ToString());
            foreach (var tagValue in record)
            {
                try
                {
                    string tag = CheckTagValue(tagValue.Key, tagValue.Value);
                    writer.WriteStartElement(tag);
                    writer.WriteValue(tagValue.Value);
                    writer.WriteEndElement();
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            writer.WriteEndElement();
            count++;
        }
        public static void EndFile()
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
        private string CheckTagValue(int tag, string value)
        {
            
            if(tagsTable.CheckInvalidTag(tag)) throw new ArgumentException($"WARN: Invalid tag \"{tag}\", {cur_fileName} line {count + 1}");
            string tagname = tagsTable.GetTagName(tag);
            if (value != "NULL" && tagsTable.HaveValueRanges(tag))//Checks if the tag have not a value ranges
            {
                if (tagsTable.IsString(tag) && tagsTable.CheckStringLengthToBig(tag,value.Length))//Checks if value type is string
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
               if(!tagsTable.CheckValidValue(tag,value))throw new ArgumentException($"WARN: Value not in range \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");
            }
            return tagname;
        }

    }
}

