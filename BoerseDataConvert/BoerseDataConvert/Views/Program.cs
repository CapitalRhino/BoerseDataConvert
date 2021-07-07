using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace BoerseDataConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            // Args format
            // -i *.zip or --input *.zip
            // -d directory or --dir directory
            // -o directory or --output direcory
            // -h - help
            string s = "900#01|204#J|008#HSH Nordbank AG|205#150215|206#111138|460#HSH Nordbank|207#HSH Nordbank AG#Gerhart-Hauptmann-Platz 50#20095 Hamburg#Deutschland|208#info@hsh-nordbank.com|209#040 33330|210#https://www.hsh-nordbank.de|451#TUKDD90GPC79G1KOE162";
            string[] sr = s.Split("|").ToArray();
            Dictionary<string, string> a = new Dictionary<string, string>();
            foreach (var item in sr)
            {
                string[] d = item.Split('#').ToArray();
                a.Add(d[0], d[1]);
            }
            Record record = new Record();
            record.TagsValues = a;
            RecordController con = new RecordController("oo");
            Console.WriteLine(con.ConvertToXml(record));
            Console.WriteLine("Hello World!");
        }
    }
}
