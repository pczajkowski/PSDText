using System;
using System.IO;
using Newtonsoft.Json;

namespace PSDText
{
    public class MyJsonSerializer : ISerializer
    {
        private readonly JsonSerializer serializer;

        public MyJsonSerializer(JsonSerializer jsonSerializer)
        {
            if (jsonSerializer == null)
                throw new ArgumentNullException("jsonSerializer");

            serializer = jsonSerializer;
        }

        public void Serialize(TextWriter textWriter, object o)
        {
            serializer.Serialize(textWriter, o);
        }
    }
}
