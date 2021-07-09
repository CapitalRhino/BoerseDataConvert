using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public class Tag
    {
        private string name;
        private bool isString;
        private int stringLength;
        private bool haveValueRanges;
        private string[] valueRanges;

        public Tag(string[] input)
        {
            name = input[0];
            string[] type = input[1].Split('-').ToArray();
            if (type.Length == 2)
            {
                isString = true;
                stringLength = int.Parse(type[1]);
            }
            else isString = false;
            if (input.Length == 3)
            {
                valueRanges = input[2].Split('#').ToArray();
            }
            else haveValueRanges = false;
        }
        public string Name
        {
            get { return name; }
        }
        public bool IsString
        {
            get { return isString; }
        }
        public bool HaveValueRanges
        {
            get { return haveValueRanges; }
        }
        public int StringLength
        {
            get { return stringLength; }
        }
        public bool ValidValue(string value)
        {
            foreach (string validValue  in valueRanges)
            {
                if (validValue == value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
