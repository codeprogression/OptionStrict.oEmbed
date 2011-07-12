/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace OptionStrict.oEmbed
{
    public interface IoEmbedWriter
    {
        string Write(oEmbed oembed, oEmbedFormat format);
        string Write(oEmbed oembed, oEmbedFormat format, string callback);
        Stream WriteWebResponse(HttpResponseBase response, oEmbed oembed, oEmbedFormat format);
        Stream WriteWebResponse(HttpResponseBase response, oEmbed oembed, oEmbedFormat format, string callback);
        Stream WriteWcfResponse(OutgoingWebResponseContext response, oEmbed oembed, oEmbedFormat format);
        Stream WriteWcfResponse(OutgoingWebResponseContext response, oEmbed oembed, oEmbedFormat format, string callback);
    }

    public class oEmbedWriter : IoEmbedWriter
    {
        // direct to request with proper content type set and httpstatus for errors

        #region IoEmbedWriter Members

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

        public virtual Stream WriteWebResponse(HttpResponseBase response, oEmbed oembed, oEmbedFormat format)
        {
            if (format== oEmbedFormat.Jsonp)
                throw new ArgumentException("jsonp format requires a callback", "format");
            return WriteWebResponse(response, oembed, format, null);
        }

        public virtual Stream WriteWebResponse(HttpResponseBase response, oEmbed oembed, oEmbedFormat format,
                                               string callback)
        {
            if (oembed == null)
                return FileNotFound(response);
            string oEmbedString;
            switch (format)
            {
                case oEmbedFormat.Unspecified:
                case oEmbedFormat.Json:
                    oEmbedString = oEmbedSerializer.SerializeJson(oembed);
                    response.ContentType = "application/json";
                    break;
                case oEmbedFormat.Jsonp:
                    oEmbedString = callback + "(" + oEmbedSerializer.SerializeJson(oembed) + ")";
                    response.ContentType = "application/javascript";
                    break;
                case oEmbedFormat.Xml:
                    oEmbedString = oEmbedSerializer.SerializeXml(oembed);
                    response.ContentType = "text/xml";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }

            var resultStream = ToStream(oEmbedString);
            if (response.Headers["Content-Length"]!=null)
                response.Headers.Remove("Content-Length");
            response.AddHeader("Content-Length", resultStream.Length.ToString());
            return resultStream;
        }

        public virtual Stream WriteWcfResponse(OutgoingWebResponseContext response, oEmbed oembed, oEmbedFormat format)
        {
            if (format == oEmbedFormat.Jsonp)
                throw new ArgumentException("jsonp format requires a callback", "format");
            return WriteWcfResponse(response, oembed, format, null);
        }

        public virtual Stream WriteWcfResponse(OutgoingWebResponseContext response, oEmbed oembed, oEmbedFormat format,
                                               string callback)
        {
            if (oembed == null)
                return FileNotFound(response);
            string oEmbedString;
            switch (format)
            {
                case oEmbedFormat.Unspecified:
                case oEmbedFormat.Json:
                    oEmbedString = oEmbedSerializer.SerializeJson(oembed);
                    response.ContentType = "application/json";
                    break;
                case oEmbedFormat.Jsonp:
                    oEmbedString = callback + "(" + oEmbedSerializer.SerializeJson(oembed) + ")";
                    response.ContentType = "application/javascript";
                    break;
                case oEmbedFormat.Xml:
                    oEmbedString = oEmbedSerializer.SerializeXml(oembed);
                    response.ContentType = "text/xml";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format");
            }
            var resultStream = ToStream(oEmbedString);
            response.ContentLength = resultStream.Length;
            if (oembed.CacheAge > 0)
            {
                response.Headers.Add(HttpResponseHeader.CacheControl, "max-age=" + oembed.CacheAge + ", public");
                response.Headers.Add(HttpResponseHeader.Expires,
                                     DateTime.UtcNow.AddSeconds(oembed.CacheAge).ToString("R",
                                                                                          CultureInfo.InvariantCulture));
            }
            return resultStream;
        }

        #endregion

        private Stream FileNotFound(HttpResponseBase response)
        {
            response.SuppressContent = true;
            response.StatusCode = 404;
            return new MemoryStream();
        }

        private Stream FileNotFound(OutgoingWebResponseContext response)
        {
            response.SuppressEntityBody = true;
            response.StatusCode = HttpStatusCode.NotFound;
            return new MemoryStream();
        }

        private Stream ToStream(string oEmbedString)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(oEmbedString));
        }
    }
}