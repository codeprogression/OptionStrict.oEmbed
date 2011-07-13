using System;
using System.IO;
using System.Web;

namespace OptionStrict.oEmbed.MVC
{
    public class oEmbedWriter : OptionStrict.oEmbed.oEmbedWriter
    {
        readonly HttpResponseBase _response;

        public oEmbedWriter(HttpResponseBase response)
        {
            _response = response;
        }

        public override Stream WriteResponse(oEmbed oembed, oEmbedFormat format)
        {
            if (format == oEmbedFormat.Jsonp)
                throw new ArgumentException("jsonp format requires a callback", "format");
            return WriteResponse(oembed, format, null);
        }

        public override Stream WriteResponse(oEmbed oembed, oEmbedFormat format, string callback)
        {
            if (oembed == null)
                return FileNotFound();
            string oEmbedString;
            switch (format)
            {
                case oEmbedFormat.Unspecified:
                case oEmbedFormat.Json:
                    oEmbedString = oEmbedSerializer.SerializeJson(oembed);
                    _response.ContentType = "application/json";
                    break;
                case oEmbedFormat.Jsonp:
                    oEmbedString = callback + "(" + oEmbedSerializer.SerializeJson(oembed) + ")";
                    _response.ContentType = "application/javascript";
                    break;
                case oEmbedFormat.Xml:
                    oEmbedString = oEmbedSerializer.SerializeXml(oembed);
                    _response.ContentType = "text/xml";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }

            var resultStream = ToStream(oEmbedString);
            if (_response.Headers["Content-Length"] != null)
                _response.Headers.Remove("Content-Length");
            _response.AddHeader("Content-Length", resultStream.Length.ToString());
            return resultStream;
        }

        public override Stream FileNotFound()
        {
            _response.SuppressContent = true;
            _response.StatusCode = 404;
            return new MemoryStream();
        }

    }
}