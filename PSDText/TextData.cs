using System.Xml.Serialization;

namespace PSDText
{
    public class TextData
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string Text { get; set; }
    }
}
