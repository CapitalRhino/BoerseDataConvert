using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    /// <summary>
    /// Class Recorrd stores information about the tags and the values of every record 
    /// </summary>
    public class Record : IEnumerable
    {
        //The tags and the values are stored in list from KeyValuePair where the tags are the keys
        private List<KeyValuePair<int, string>> TagsValues;

        public Record()
        {
            TagsValues = new List<KeyValuePair<int, string>>();
        }
        /// <summary>
        /// This method adds a new TagValuePair in the TagsValues
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public void Add(int tag, string value)
        {
            TagsValues.Add(new KeyValuePair<int, string>(tag, value));
        }
        /// <summary>
        /// The iterator for class Record
        /// </summary>
        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            foreach (var item in TagsValues)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

