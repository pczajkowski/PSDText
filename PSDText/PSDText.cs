using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PSDText
{
    public class PSDText
    {
        private readonly string _xmlData;
        private readonly XmlNamespaceManager _ns = new XmlNamespaceManager(new NameTable());
        public List<TextData> TextData;

        private string Readxmpmeta(string path)
        {
            var sb = new StringBuilder(1000);
            using (var sr = new StreamReader(path))
            {
                var read = false;
                while (true)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        return sb.ToString();

                    if (line.StartsWith("<x:xmpmeta"))
                        read = true;

                    if (read) sb.Append(line);

                    if (line.StartsWith("</x:xmpmeta>"))
                        break;
                }
            }

            return sb.ToString();
        }

        private void AddXMLNamespaces()
        {
            _ns.AddNamespace("x", "adobe:ns:meta/");
            _ns.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            _ns.AddNamespace("photoshop", "http://ns.adobe.com/photoshop/1.0/");
        }

        private XmlDocument ReadXML()
        {
            var xml = new XmlDocument();

            using (var sr = new StringReader(_xmlData))
            using (var reader = XmlReader.Create(sr))
            {
                xml.Load(reader);
            }

            return xml;
        }

        private List<TextData> GetTextData()
        {
            var xml = ReadXML();
            var data = new List<TextData>();

            var textNodes =
                xml.SelectNodes("/x:xmpmeta/rdf:RDF/rdf:Description/photoshop:TextLayers/rdf:Bag/rdf:li", _ns);
            foreach (XmlNode textNode in textNodes)
            {
                var name = textNode.SelectSingleNode("./photoshop:LayerName", _ns)?.InnerText;
                var text = textNode.SelectSingleNode("./photoshop:LayerText", _ns)?.InnerText;

                data.Add(new TextData { Name = name, Text = text });
            }

            return data;
        }

        public PSDText(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"File {path} doesn't exist!");

            _xmlData = Readxmpmeta(path);
            AddXMLNamespaces();
            TextData = GetTextData();
        }

        /// <summary>
        /// Serializes text layers as XML.
        /// </summary>
        /// <param name="path">Output XML path.</param>
        /// <returns>True on successful serialization,
        /// false if there's nothing to write.</returns>
        public bool SaveAsXML(string path)
        {
            if (!TextData.Any())
                return false;

            var serializer = new XmlSerializer(typeof(List<TextData>));
            using (var sr = new StreamWriter(path))
            {
                serializer.Serialize(sr, TextData);
            }

            return true;
        }

        /// <summary>
        /// Serializes text layers as JSON.
        /// </summary>
        /// <param name="path">Output JSON path.</param>
        /// <returns>True on successful serialization,
        /// false if there's nothing to write.</returns>
        public bool SaveAsJSON(string path)
        {
            if (!TextData.Any())
                return false;

            var serializer = new JsonSerializer();
            using (var sr = new StreamWriter(path))
            {
                serializer.Serialize(sr, TextData);
            }

            return true;
        }
    }
}
