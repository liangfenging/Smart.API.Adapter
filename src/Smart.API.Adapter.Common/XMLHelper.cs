using System.Xml;
using System.Xml.Serialization;

namespace Smart.API.Adapter.Common
{
    public class XMLHelper
    {
        public static T FromXMLToObject<T>(string xmlFilePath) where T : class
        {
            var reader = XmlReader.Create(xmlFilePath);
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
    }
}
