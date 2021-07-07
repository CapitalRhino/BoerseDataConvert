using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoerseDataConvert
{
    public class Reader
    {
        private string[] filesNames;
        private StreamReader reader;
        private int fileInd;
        private string adr;
        public Reader(string adr, string[] filesNames)
        {
            fileInd = 0;
            reader = new StreamReader($@"{adr}/{filesNames[fileInd]}", CodePagesEncodingProvider.Instance.GetEncoding(1252));
            this.adr = adr;
            this.filesNames = filesNames;
            reader.ReadLine();
        }
        public Record ReadLineRecord()
        {
            string s = reader.ReadLine();
            if (s.Substring(0, 11) == "Datensaetze")
            {
                fileInd++;
                reader.Close();
                reader = new StreamReader($@"{adr}/{filesNames[fileInd]}", CodePagesEncodingProvider.Instance.GetEncoding(1252));
                RecordController.NextFile(filesNames[fileInd]);
                Writer.NextFile(filesNames[fileInd]);
                s = reader.ReadLine();
                s = reader.ReadLine();
            }
             string[] sr = s.Split("|").ToArray();
            Dictionary<string, string> a = new Dictionary<string, string>();
            foreach (var item in sr)
            {
                string[] d = item.Split('#').ToArray();
                a.Add(d[0], d[1]);
            }
            Record record = new Record();
            record.TagsValues = a;
            return record;
        }
    }
}
