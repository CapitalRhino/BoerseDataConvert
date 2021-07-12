using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert.Model
{
    public static  class WarningStat
    {
        private class Warning
        {
            string type;
            int count;

            public Warning(string type)
            {
                this.type = type;
                count = 1;
            }
            public void Add()
            {
                count++;
            }
        }

        private static Dictionary<int, Warning> tableWarnings = tableWarnings = new Dictionary<int, Warning>();
        private static string curFile;
        public static void Refresh(string fileName)
        {
            curFile = fileName;
            tableWarnings = new Dictionary<int,Warning>();
        }
        /*
         type "inv" - Ivalid tag
         type "long" - too long string value
         type "notnum" - is not in a valid format for number
         type "range" - Value not in range 
        */
        public static void Add(int tag,string type)//
        {
            if (tableWarnings.ContainsKey(tag))
            {
                tableWarnings[tag].Add();
            }
            else 
            {
                Warning warning = new Warning(type);
                tableWarnings.Add(tag, warning);
            }
        }
    }
}
