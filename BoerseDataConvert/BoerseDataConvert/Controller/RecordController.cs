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
        private static WarningStat warning;
        public static  int Count
        {
            get { return count - 1; }
        }
        /// <summary>
        /// It create XML writer,xml file, writes the first line in it
        /// </summary>
        /// <param name="adr">output directoty</param>
        /// <param name="fileName">currnt file name</param>
        /// <param name="tags">Locataion of tags,txt</param>
        public RecordController(string adr, string fileName, string tags)
        {
            count = 1;
            address = adr;
            cur_fileName = fileName.Split('.').First();
            Directory.CreateDirectory(address);
            tagsTable = new TagsTable(tags);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.Indent = true;
            writer = XmlWriter.Create($@"{address}/{cur_fileName}.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", null, cur_fileName);
            warning = new WarningStat(fileName);
        }
        /// <summary>
        /// It writes the last line, closes the flie, and XMLwriter
        /// and It creates new XML writer,new xml file, writes the first line in it
        /// </summary>
        /// <param name="fileName">name of the next file</param>
        public static void NextFile(string fileName)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            warning.PrintWarnigs();
            warning = new WarningStat(fileName);
            cur_fileName = fileName.Split('.').First();
            count = 1;
            Console.WriteLine($"INFO: Converting {cur_fileName}");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CheckCharacters = false;
            writer = XmlWriter.Create($@"{address}/{cur_fileName}.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("table");
            writer.WriteAttributeString("name", null, cur_fileName);
        }
        /// <summary>
        /// It write the information in the record into the XML file
        /// </summary>
        /// <param name="record">Record that stores information about one record from the files</param>
        /// <param name="fastCheck">if true it makes a fastChech</param>
        public void WriteXmlRecord(Record record,bool fastCheck)
        {
            writer.WriteStartElement("record");
            writer.WriteAttributeString("id", null, count.ToString());
            foreach (var tagValue in record)
            {
                string tag = "";
                if (fastCheck)
                {
                    tag = FastChechTag(tagValue.Key);
                }
                else 
                {  
                    tag = FullCheckTagValue(tagValue.Key, tagValue.Value);
                }
                
                if (tag != null)
                {
                    writer.WriteStartElement(tag);
                    writer.WriteValue(tagValue.Value);
                    writer.WriteEndElement();
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
            warning.PrintWarnigs();
        }
        /// <summary>
        /// It checks only if the tag is valid
        /// </summary>
        /// <param name="tag">tag in the numeric form</param>
        /// <returns>return name of the tag</returns>
        private string FastChechTag(int tag)
        {
            if (tagsTable.CheckInvalidTag(tag))
            {
                warning.Add(tag);
                return null;
            }
            else
            {
                return tagsTable.GetTagName(tag);
            }
        }
        /// <summary>
        /// It make a full check on the value
        /// </summary>
        /// <param name="tag">tag in the numeric form</param>
        /// <param name="value"></param>
        /// <returns>return name of the tag</returns>
        private string FullCheckTagValue(int tag, string value)
        {
            if (tagsTable.CheckInvalidTag(tag))
            {
                warning.Add(tag);
                return null;
            }
            string tagname = tagsTable.GetTagName(tag);
            if (value == "") return tagname;
            if (value != "NULL" && !tagsTable.HaveValueRanges(tag)) //Checks if the tag have not a value ranges
            {
                if (tagsTable.IsString(tag))//Checks if value type is string
                {
                    if (tagsTable.CheckStringLengthToBig(tag, value.Length))
                    {
                        Console.Error.WriteLine($"WARN: Too long value \"{tag}\", \"{value}\", max allowed \"{tagsTable.GetTagName(tag)}\", {cur_fileName} line {count + 1}");
                        return null;
                    }
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
                        Console.Error.WriteLine($"WARN: Value is not in a valid format for number \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");
                        return null;
                    }
                }
            }
            else//Checks if the tag have  a value ranges
            {
                if (!tagsTable.CheckValidValue(tag, value))
                {
                    Console.Error.WriteLine($"WARN: Value not in range \"{tag}\", \"{value}\", {cur_fileName} line {count + 1}");
                    return null;
                }
            }
            return tagname;
        }

    }
}

