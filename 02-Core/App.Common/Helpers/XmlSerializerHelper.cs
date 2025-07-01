using System.IO;
using System.Xml.Serialization;

namespace App.Common.Helpers
{
    public static class XmlSerializerHelper
    {
        public static string GetXmlString<TModel>(TModel model)
            where TModel : class
        {
            var xmlSerializer = new XmlSerializer(typeof(TModel));

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, model);

                return stringWriter.ToString();
            }
        }
    }
}
