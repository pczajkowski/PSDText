using System.IO;

namespace PSDText
{
    public interface ISerializer
    {
        void Serialize(TextWriter textWriter, object o);
    }
}
