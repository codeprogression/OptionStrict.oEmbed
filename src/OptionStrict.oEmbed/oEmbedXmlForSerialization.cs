/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System;
using System.Xml;
using System.Xml.Serialization;

namespace OptionStrict.oEmbed
{
    [XmlRoot("oembed")]
    public class oEmbedXmlForSerialization : oEmbed
    {
        public oEmbedXmlForSerialization()
        {
        }

        public oEmbedXmlForSerialization(oEmbed oEmbed)
        {
            Version = oEmbed.Version;
            Type = oEmbed.Type;
            Title = oEmbed.Title;
            AuthorName = oEmbed.AuthorName;
            AuthorUrl = oEmbed.AuthorUrl;
            ProviderName = oEmbed.ProviderName;
            ProviderUrl = oEmbed.ProviderUrl;
            CacheAge = oEmbed.CacheAge;
            ThumbnailUrl = oEmbed.ThumbnailUrl;
            ThumbnailWidth = oEmbed.ThumbnailWidth;
            ThumbnailHeight = oEmbed.ThumbnailHeight;
            Url = oEmbed.Url;
            Width = oEmbed.Width;
            Height = oEmbed.Height;
            Html = oEmbed.Html;
        }

        [XmlElement("html")]
        [XmlText]
        public XmlNode[] PCDATA
        {
            get
            {
                var dummy = new XmlDocument();
                XmlNode node = dummy.CreateNode(XmlNodeType.Text, "html", "");
                node.Value = Html;
                return new[] {node};
            }

            set
            {
                if (value == null)
                {
                    Html = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }

                XmlNode node0 = value[0];

                if (node0 == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid node type {0}", node0.NodeType));
                }

                Html = node0.Value ?? node0.OuterXml;
            }
        }
    }
}