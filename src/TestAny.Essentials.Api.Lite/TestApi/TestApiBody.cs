using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pj.Library;

namespace TestAny.Essentials.Api
{
    /// <summary>
    /// Api Response body representation
    /// </summary>
    public class TestApiBody
    {
        public TestApiBody(string content)
        {
            ContentString = content;
        }

        private string _contentString;

        /// <summary>
        /// Returns the response message content as string
        /// </summary>
        public string ContentString
        {
            get
            {
                return _contentString;
            }

            private set
            {
                _contentString = value;
            }
        }

        /// <summary>
        /// Returns the response message as Json (which is of type dynamic)
        /// </summary>
        public dynamic ContentJson
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(ContentString);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert the Json to a type
        /// </summary>
        /// <typeparam name="T">Type to which the json needs to be converted</typeparam>
        /// <returns>T: Type to which json is converted</returns>
        public T ToType<T>()
        {
            try
            {
                return SerializationHelper.DeSerializeJsonFromString<T>(JsonConvert.SerializeObject(ContentJson));
            }
            catch (Exception ex)
            {
                PjUtility.Runtime.Logger.Log($"Conversion failed to type: {typeof(T).Name}. Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Convert the Json to a type
        /// </summary>
        /// <typeparam name="T">Type to which the json needs to be converted</typeparam>
        /// <param name="content">json in form of dynamic content</param>
        /// <returns>T: Type to which json is converted</returns>
        public T ToType<T>(dynamic content)
        {
            try
            {
                return SerializationHelper.DeSerializeJsonFromString<T>(JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                PjUtility.Runtime.Logger.Log($"Conversion failed to type: {typeof(T).Name}. Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Filter the json using json path expression
        /// </summary>
        /// <param name="filterText">json path expression</param>
        /// <returns>returns the IEnumerable<string> which matched the json path expression against the content</returns>
        public IEnumerable<string> FilterJsonContent(string filterText) =>
            ContentString
            .ApplyJsonPathExpression(filterText);
    }
}
