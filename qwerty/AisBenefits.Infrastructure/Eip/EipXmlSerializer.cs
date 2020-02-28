using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AisBenefits.Infrastructure.Eip
{
    public static class EipXmlTextSerializer
    {
        public static string Serialize<T>(T obj)
        {
            var ser = new XmlSerializer(typeof(T));

            var ms = new MemoryStream();

            var sw = new StreamWriter(ms, Encoding.UTF8);

            using (XmlWriter writer = XmlWriter.Create(sw))
            {
                ser.Serialize(writer, obj);
            }

            ms.Position = 0;
            var sr = new StreamReader(ms);
            var str = sr.ReadToEnd();

            return str;
        }

        public static T Deserialize<T>(string str)
        {
            var ser = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(str))
            {
                var result = ser.Deserialize(reader);

                return (T)result;
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            var ser = new XmlSerializer(typeof(T));

            using (TextReader reader = new StreamReader(stream))
            {
                var result = ser.Deserialize(reader);

                return (T)result;
            }
        }
    }
}
