using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    /// <summary>
    /// This class stores the name,the type and the value ranges for one tag 
    /// </summary>
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
                haveValueRanges = true;
            }
            else haveValueRanges = false;
        }
        /// <summary>
        /// Returns the name of the tag 
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Returns true if the type of the tag is string 
        /// </summary>
        public bool IsString
        {
            get { return isString; }
        }
        /// <summary>
        /// Returns true if  the tag has a value ranges
        /// </summary>
        public bool HaveValueRanges
        {
            get { return haveValueRanges; }
        }
        /// <summary>
        /// Returns max length of the string value
        /// </summary>
        public int StringLength
        {
            get { return stringLength; }
        }
        /// <summary>
        /// Returns true if the value is in the value ranges
        /// </summary>
        public bool ValidValue(string value)
        {
            foreach (string validValue in valueRanges)
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
