using Newtonsoft.Json;
using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Selenium.Essentials.Utilities.Helpers
{
    public class SerializationHelper
    {
        public static void SerializeToXml<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }
        public static T DeSerializeFromXml<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            string attributeXml = string.Empty;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (T)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }

            return objectOut;
        }

        public static string SerializeToJson(dynamic obj) => JsonConvert.SerializeObject(obj);
        public static void SerializeToJson<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(serializableObject, Newtonsoft.Json.Formatting.Indented));
        }
        public static T DeSerializeFromJsonFile<T>(string fileName)
        {
            if (fileName.IsEmpty()) { return default(T); }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
        }
        public static T DeSerializeFromJsonContent<T>(string content)
        {
            if (string.IsNullOrEmpty(content)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
