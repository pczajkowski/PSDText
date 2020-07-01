using System.Xml.Serialization;

namespace PSDText
{
    public class TextData
    {
        [XmlAttribute]
        public string Name;

        public string Text;
    }
}
