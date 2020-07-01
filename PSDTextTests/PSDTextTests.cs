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
    }
}
