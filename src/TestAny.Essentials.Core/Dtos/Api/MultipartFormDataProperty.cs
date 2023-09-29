namespace TestAny.Essentials.Core.Dtos.Api
{
    public class MultipartFormDataProperty
    {
        public enum ContentTransmitTypeEnum
        {
            AsString,
            AsFile
        }

        public enum ContentStreamTypeEnum
        {
            Stream,
            Text
        }

        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public ContentTransmitTypeEnum ContentTransmitType { get; set; }
        public ContentStreamTypeEnum ContentStreamType { get; set; }

        public string ContentType { get; set; }
    }
}
