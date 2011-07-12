using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.ServiceModel.Web;

namespace OptionStrict.oEmbed.WCF
{
    public class oEmbedWriter : OptionStrict.oEmbed.oEmbedWriter
    {
        readonly OutgoingWebResponseContext _response;

        public oEmbedWriter(OutgoingWebResponseContext response)
        {
            _response = response;
        }

        public override Stream WriteResponse(oEmbed oembed, oEmbedFormat format)
        {
            if (format == oEmbedFormat.Jsonp)
                throw new ArgumentException("jsonp format requires a callback", "format");
            return WriteResponse(oembed, format, null);
        }

        public override Stream WriteResponse(oEmbed oembed, oEmbedFormat format,
                                             string callback)
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
            _response.ContentLength = resultStream.Length;
            if (oembed.CacheAge > 0)
            {
                _response.Headers.Add(HttpResponseHeader.CacheControl, "max-age=" + oembed.CacheAge + ", public");
                _response.Headers.Add(HttpResponseHeader.Expires,
                                      DateTime.UtcNow.AddSeconds(oembed.CacheAge).ToString("R",
                                                                                           CultureInfo.InvariantCulture));
            }
            return resultStream;
        }

        public override Stream FileNotFound()
        {
            _response.SuppressEntityBody = true;
            _response.StatusCode = HttpStatusCode.NotFound;
            return new MemoryStream();
        }
    }
}
