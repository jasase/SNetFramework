using System.Collections.Generic;
using System.IO;

namespace Framework.Contracts.Services.XmlToObject
{
    public interface IXmlToObject
    {
        T ReadXml<T>(string path);
        T ReadXml<T>(string path, IEnumerable<string> pathToSchema);        
        T ReadXml<T>(Stream stream);
        T ReadXmlFromString<T>(string xml);
    }
}
