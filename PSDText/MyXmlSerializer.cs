using System;
using System.IO;
using System.Xml.Serialization;

namespace PSDText
{
    public class MyXmlSerializer : ISerializer
    {
        private readonly XmlSerializer serializer;

        public MyXmlSerializer(XmlSerializer xmlSerializer)
        {
            if (xmlSerializer == null)
                throw new ArgumentNullException("xmlSerializer");

            serializer = xmlSerializer;
        }

        public void Serialize(TextWriter textWriter, object o)
        {
            serializer.Serialize(textWriter, o);
        }
    }
}
