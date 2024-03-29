﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace PSDText
{
    public class PSDText
    {
        private readonly string _xmlData;
        private readonly XmlNamespaceManager _ns = new(new NameTable());
        public List<TextData> ExtractedStrings;

        private static string Readxmpmeta(string path)
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
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var text = textNode.SelectSingleNode("./photoshop:LayerText", _ns)?.InnerText;
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                data.Add(new TextData { Name = name, Text = text });
            }

            return data;
        }

        public PSDText(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File {path} doesn't exist!");

            _xmlData = Readxmpmeta(path);
            if (string.IsNullOrWhiteSpace(_xmlData))
                throw new Exception("No data was read!");

            AddXMLNamespaces();
            ExtractedStrings = GetTextData();
            if (!ExtractedStrings.Any())
                throw new Exception("Nothing was read from XML!");
        }

        /// <summary>
        /// Serializes text layers as XML.
        /// </summary>
        /// <param name="path">Output XML path.</param>
        public void SaveAsXML(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var xmlSerializer = new XmlSerializer(typeof(List<TextData>));
            using var fs = File.OpenWrite(path);
            xmlSerializer.Serialize(fs, ExtractedStrings);
        }

        /// <summary>
        /// Serializes text layers as JSON.
        /// </summary>
        /// <param name="path">Output JSON path.</param>
        public void SaveAsJSON(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            using var fs = File.OpenWrite(path);
            JsonSerializer.Serialize(fs, ExtractedStrings, typeof(List<TextData>));
        }
    }
}
