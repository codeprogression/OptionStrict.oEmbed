using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace OptionStrict.oEmbed.Example.Models
{
    public class OembedView
    {
        public string RawResult { get; set; }
        public string Format { get; set; }
        public string AuthorName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }

        public void PrettyPrint()
        {
            PrettyPrintXml(Html);
            if (Format.ToLower() == "json")
                PrettyPrintJson();
            else if (Format.ToLower() == "xml")
                PrettyPrintXml(RawResult);
        }

        void PrettyPrintXml(string xml)
        {
            var document = new XmlDocument();

            try
            {
                document.LoadXml(xml);
                using (var stream = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(stream, Encoding.Unicode))
                    {

                        writer.Formatting = System.Xml.Formatting.Indented;
                        document.WriteContentTo(writer);
                        writer.Flush();
                        stream.Flush();
                        stream.Position = 0;

                        var reader = new StreamReader(stream);
                        xml = reader.ReadToEnd();
                    }
                }
            }
            catch (XmlException)
            {
            }
        }

        void PrettyPrintJson()
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(RawResult);
            RawResult = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}