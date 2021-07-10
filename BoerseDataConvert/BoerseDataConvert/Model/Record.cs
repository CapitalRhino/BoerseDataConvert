using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{

    public class Record : IEnumerable
    {
        private List<KeyValuePair<int, string>> TagsValues;

        public Record()
        {
            TagsValues = new List<KeyValuePair<int, string>>();
        }
        public void Add(int tag, string value)
        {
            TagsValues.Add(new KeyValuePair<int, string>(tag, value));
        }
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

