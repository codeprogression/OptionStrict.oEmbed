/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System.Xml.Serialization;

namespace OptionStrict.oEmbed
{
    public enum oEmbedType
    {
        [XmlEnum(Name = "unspecified")] Unspecified,
        [XmlEnum(Name = "rich")] Rich,
        [XmlEnum(Name = "photo")] Photo,
        [XmlEnum(Name = "video")] Video,
        [XmlEnum(Name = "link")] Link
    }
}