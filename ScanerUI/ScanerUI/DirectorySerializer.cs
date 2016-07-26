using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScanerUI
{
    public class DirectorySerializer
    {
        private const string FilePath = @".\data.xml";

        private XmlSerializer serializer = new XmlSerializer(typeof(Directory));

        public void WriteScores(Directory directory)
        {
            using (var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                serializer.Serialize(stream, directory);
                stream.Close();
            }
        }
    }
}
