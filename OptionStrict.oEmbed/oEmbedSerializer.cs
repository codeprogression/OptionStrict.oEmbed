/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OptionStrict.oEmbed
{
    public static class oEmbedSerializer
    {
        public static oEmbed Deserialize(oEmbedResponse response)
        {
            return Deserialize(response.RawResult, response.Format);
        }

        public static oEmbed Deserialize(string response, oEmbedFormat format)
        {
            oEmbed result = null;
            switch (format)
            {
                case oEmbedFormat.Unspecified:
                    break;
                case oEmbedFormat.Json:
                    result = DeserializeJson(response);
                    break;
                case oEmbedFormat.Xml:
                    result = DeserializeXml(response);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        public static oEmbed DeserializeJson(string response)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] {new oEmbedJavaScriptConverter()});
                return serializer.Deserialize<oEmbed>(response);
            }
            catch
            {
                return null;
            }
        }

        public static oEmbed DeserializeXml(string response)
        {
            var serializer = new XmlSerializer(typeof (oEmbedXmlForSerialization));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response));
            try
            {
                return serializer.Deserialize(memoryStream) as oEmbed;
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeJson(oEmbed response)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] {new oEmbedJavaScriptConverter()});
                return serializer.Serialize(response);
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeXml(oEmbed response)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var serializer = new XmlSerializer(typeof (oEmbedXmlForSerialization));
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                serializer.Serialize(xmlTextWriter, new oEmbedXmlForSerialization(response));
                memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch
            {
                return null;
            }
        }
    }
}