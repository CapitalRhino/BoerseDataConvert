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
        private string CheckTagValue(string tag, string value)
        {
            return "";
        }
    }
}
