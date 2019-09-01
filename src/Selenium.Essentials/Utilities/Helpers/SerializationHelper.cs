using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Selenium.Essentials
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
        public static T DeSerializeJsonFromString<T>(string content)
        {
            if (string.IsNullOrEmpty(content)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(content);
        }

        public static Dictionary<string, string> JsonToDictionary(string content) => _JsonReadToDict(content);

        private static string _CalculateJsonKeyPropertyName(string parent, string currentPropertyName)
        {
            return parent.IsEmpty() ? currentPropertyName : $"{parent} - {currentPropertyName}";
        }

        private static Dictionary<string, string> _JsonInnerCollectionPropertyNameDuplicateNumberingLogic(
            Dictionary<string, string> primaryDictionary,
            Dictionary<string, string> innerCollectionDictionary,
            JProperty currentProperty,
            string parentPropertyName
            )
        {
            if (innerCollectionDictionary.Any())
            {
                innerCollectionDictionary.Iter(d =>
                {
                    var calculatedKey = _CalculateJsonKeyPropertyName(parentPropertyName, d.Key);
                    if (primaryDictionary.ContainsKey(calculatedKey))
                    {
                        calculatedKey = $"{calculatedKey}_{Guid.NewGuid()}";
                    }
                    primaryDictionary.Add(calculatedKey, d.Value);
                });
            }
            else
            {
                if (currentProperty != null)
                {
                    var calculatedKey = _CalculateJsonKeyPropertyName(parentPropertyName, currentProperty.Name);
                    if (primaryDictionary.ContainsKey(calculatedKey))
                    {
                        calculatedKey = $"{calculatedKey}_{Guid.NewGuid()}";
                    }
                    primaryDictionary.Add(calculatedKey, currentProperty?.Value.ToString());
                }
            }
            return primaryDictionary;
        }

        private static Dictionary<string, string> _JsonReadToDict(string content, string parent = "")
        {
            var dicCollector = new Dictionary<string, string>();
            try
            {
                if (content.Trim().StartsWith("[") && content.Trim().EndsWith("]"))
                {
                    var jarr = JArray.Parse(content);
                    jarr.Children().OfType<JObject>().Iter(c =>
                    {
                        var inner = _JsonReadToDict(c.ToString(), parent);
                        dicCollector = _JsonInnerCollectionPropertyNameDuplicateNumberingLogic(dicCollector, inner, null, parent);
                    });
                }
                else
                {
                    var jobj = JObject.Parse(content);
                    jobj.Children()
                        .OfType<JProperty>()
                        .Iter(jProp =>
                        {
                            if (jProp.Value.ToString().Contains("{"))
                            {
                                //Read all inner properties
                                var innerProps = _JsonReadToDict(jProp.Value.ToString(), jProp.Name);
                                dicCollector = _JsonInnerCollectionPropertyNameDuplicateNumberingLogic(dicCollector, innerProps, jProp, parent);
                            }
                            else if (jProp.Value.ToString().Trim().StartsWith("[") && jProp.Value.ToString().Trim().EndsWith("]"))
                            {
                                dicCollector.Add(_CalculateJsonKeyPropertyName(parent, jProp.Name),
                                            string.Join(",", jProp
                                                .Value
                                                .ToString()
                                                .SplitAndTrim(",")
                                                .Select(s => s.ReplaceMultiple(string.Empty,
                                                                    Environment.NewLine,
                                                                    "[",
                                                                    "]",
                                                                    "\"").Trim())));
                            }
                            else
                            {
                                dicCollector.Add(_CalculateJsonKeyPropertyName(parent, jProp.Name), jProp.Value.ToString());
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dicCollector;
        }
    }
}
