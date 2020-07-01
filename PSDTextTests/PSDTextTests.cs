using System.IO;
using Xunit;

namespace PSDTextTests
{
    public class PSDTextTests
    {
        private const string TestFile = "./testFiles/test.psd";

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
            Assert.NotEmpty(test.TextData);

            var destination = "./test.xml";
            test.SaveAsXML(destination);
            Assert.True(File.Exists(destination));
            File.Delete(destination);
        }

        [Fact]
        public void SaveAsJSON()
        {
            var test = new PSDText.PSDText(TestFile);
            Assert.NotEmpty(test.TextData);

            var destination = "./test.json";
            test.SaveAsJSON(destination);
            Assert.True(File.Exists(destination));
            File.Delete(destination);
        }
    }
}
