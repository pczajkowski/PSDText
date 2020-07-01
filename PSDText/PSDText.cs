using System;
using System.IO;
using System.Text;

namespace PSDText
{
    public class PSDText
    {
        private readonly string _xmlData;
        private string Readxmpmeta(string path)
        {
            var sb = new StringBuilder(1000);
            using (var sr = new StreamReader(path))
            {
                sr.ReadLine(); //skip first line
                    
                var line = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return string.Empty;

                var read = line.StartsWith("<x:xmpmeta");
                while (read)
                {
                    if (line.StartsWith("</x:xmpmeta>"))
                        read = false;

                    sb.Append(line);

                    line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        return sb.ToString();
                }
            }

            return sb.ToString();
        }

        public PSDText(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"File {path} doesn't exist!");

            _xmlData = Readxmpmeta(path);
        }
    }
}
