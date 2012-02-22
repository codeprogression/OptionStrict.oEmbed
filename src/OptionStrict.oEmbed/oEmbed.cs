using System.Xml.Serialization;

namespace OptionStrict.oEmbed
{
    [XmlRoot("oembed")]
    public class oEmbed
    {
        public static readonly oEmbed Null = new oEmbed();

        public oEmbed()
        {
            Type = oEmbedType.Rich;
            Version = "1.0";
        }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("type")]
        public oEmbedType Type { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("author_name")]
        public string AuthorName { get; set; }

        [XmlElement("author_url")]
        public string AuthorUrl { get; set; }

        [XmlElement("provider_name")]
        public string ProviderName { get; set; }

        [XmlElement("provider_url")]
        public string ProviderUrl { get; set; }

        [XmlElement("cache_age")]
        public int CacheAge { get; set; }

        [XmlElement("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [XmlElement("thumbnail_width")]
        public int? ThumbnailWidth { get; set; }

        [XmlElement("thumbnail_height")]
        public int? ThumbnailHeight { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("width")]
        public int? Width { get; set; }

        [XmlElement("height")]
        public int? Height { get; set; }

        [XmlIgnore]
        public string Html { get; set; }
    }
}