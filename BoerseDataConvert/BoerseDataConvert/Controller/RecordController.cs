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
                    xmlRecord += $"";
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        private string ConvertTag(string tag)
        {
            switch (tag)
            {
                default:
                    break;
            }
        }
        private void CheckValues(string tag, string value)
        {
            
        }
    }
}
