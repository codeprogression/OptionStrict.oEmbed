using System;
using System.IO;
using System.Text;

namespace OptionStrict.oEmbed
{
    public interface IoEmbedWriter
    {
        string Write(oEmbed oembed, oEmbedFormat format);
        string Write(oEmbed oembed, oEmbedFormat format, string callback);
        Stream WriteResponse(oEmbed oembed, oEmbedFormat format);
        Stream WriteResponse(oEmbed oembed, oEmbedFormat format, string callback);}

    public abstract class oEmbedWriter : IoEmbedWriter
    {
        public virtual string Write(oEmbed oembed, oEmbedFormat format)
        {
            if (format == oEmbedFormat.Jsonp)
                throw new ArgumentException("jsonp format requires a callback", "format");
            return Write(oembed, format, null);
        }

        public virtual string Write(oEmbed oembed, oEmbedFormat format, string callback)
        {
            switch (format)
            {
                case oEmbedFormat.Unspecified:
                case oEmbedFormat.Json:
                    return oEmbedSerializer.SerializeJson(oembed);
                case oEmbedFormat.Jsonp:
                    return callback + "(" + oEmbedSerializer.SerializeJson(oembed) + ")";
                case oEmbedFormat.Xml:
                    return oEmbedSerializer.SerializeXml(oembed);
                default:
                    throw new ArgumentOutOfRangeException("format");
            }
        }

        public abstract Stream WriteResponse(oEmbed oembed, oEmbedFormat format);
        public abstract Stream WriteResponse(oEmbed oembed, oEmbedFormat format, string callback);
        public abstract Stream FileNotFound();

        protected Stream ToStream(string oEmbedString)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(oEmbedString));
        }
    }
}