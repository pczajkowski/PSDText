using System;
using System.IO;
using Xunit;

namespace PSDTextTests
{
    public class PSDTextTests
    {
        private const string TestFile = "./testFiles/test.psd";
        private const string NoTextPSD = "./testFiles/noText.psd";

        [Fact]
        public void ReadXMLFromPSD()
        {
            var test = new PSDText.PSDText(TestFile);
            Assert.NotEmpty(test.TextData);
        }

        [Fact]
        public void SaveAsXML()
        {
            var test = new PSDText.PSDText(TestFile);

            var destination = "./test.xml";
            test.SaveAsXML(destination);
            Assert.True(File.Exists(destination));
            File.Delete(destination);
        }

        [Fact]
        public void SaveAsXMLWrongPath()
        {
            var test = new PSDText.PSDText(TestFile);
            Assert.Throws<ArgumentNullException>(() => test.SaveAsXML(null));
        }

        [Fact]
        public void NoText()
        {
            Assert.Throws<Exception>(() => new PSDText.PSDText(NoTextPSD));
        }

        [Fact]
        public void SaveAsJSON()
        {
            var test = new PSDText.PSDText(TestFile);

            var destination = "./test.json";
            test.SaveAsJSON(destination);
            Assert.True(File.Exists(destination));
            File.Delete(destination);
        }

        [Fact]
        public void SaveAsJSONWrongPath()
        {
            var test = new PSDText.PSDText(TestFile);
            Assert.Throws<ArgumentNullException>(() => test.SaveAsJSON(null));
        }
    }
}
