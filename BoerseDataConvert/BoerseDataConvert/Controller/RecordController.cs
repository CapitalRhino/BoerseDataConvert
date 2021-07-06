using System;
using System.Collections.Generic;
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
            string xlmRecord = $"<record id=”{count}”>";
            foreach (var tagValue in record.TagsValues)
            {
                try
                {
                    string tag = ConvertTag(tagValue.Key);
                    CheckValues(tagValue.Key, tagValue.Value);
                    xlmRecord += $"";
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return xlmRecord;
        }
        private string ConvertTag(string tag)
        {
            switch (tag)
            {
                default:
                    break;
            }
        }
        static void CheckValues(string tag, string value)
        {
            switch (tag)
            {
                //case:
            }
        }
        static void ChechValueRange(string rangeType, string value)
        {
            List<string> posibleValues = new List<string>();
            switch (rangeType)
            {
                case "A01":
                    posibleValues = new List<string>() { "J", "N" };                
                    break;
                case "001":
                    posibleValues = new List<string>() { "WAR", "KO", "EXO","AZE","AKA","BSK","IND","Warrant", "Exotic products" };
                    break;
                case "002":
                    posibleValues = new List<string>() { "BND", "BSK", "COM", "CUR", "FI", "FON" , "FUT", "IND", "KOB", "MUL" , "STO", "DER" , "ETC", "PRC" };
                    break;
                case "001":
                    posibleValues = new List<string>() { "J", "N" };
                    break;

            }
            if (!posibleValues.Contains(value)) throw new ArgumentException("Invalid value!");
        }
    }
}
