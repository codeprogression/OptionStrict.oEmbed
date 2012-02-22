using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace OptionStrict.oEmbed
{
    public interface IoEmbedReader
    {
        oEmbedResponse Read(oEmbedRequest request);
        oEmbedResponse Read(string api, string url);
        oEmbedResponse Read(string api, string url, oEmbedFormat format);
        oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format);
        oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format, NameValueCollection queryParameters);

        // Create compiler directives to exclude all but this version in 4.0 version
        oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format, NameValueCollection queryParameters, string userAgent);
    }

    public class oEmbedReader : IoEmbedReader
    {
        public oEmbedResponse Read(string api, string url)
        {
            return Read(api, url, oEmbedFormat.Unspecified);
        }

        public oEmbedResponse Read(string api, string url, oEmbedFormat format)
        {
            return Read(api, url, null, null, format);
        }

        public oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format)
        {
            return Read(api, url, maxwidth, maxheight, format, null);
        }

        public oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format,
                                   NameValueCollection queryParameters)
        {
            return Read(api, url, maxwidth, maxheight, format, queryParameters, null);
        }

        public oEmbedResponse Read(string api, string url, int? maxwidth, int? maxheight, oEmbedFormat format,
                                   NameValueCollection queryParameters, string userAgent)
        {
            return
                Read(new oEmbedRequest(api, url, maxwidth, maxheight, format,
                                       queryParameters ?? new NameValueCollection(), userAgent));
        }

        // Create compiler directives to exclude all but this in NET4 version
//        public oEmbedResponse Read(string api, string url, int? maxwidth=null, int? maxheight=null, oEmbedFormat format=oEmbedFormat.Unspecified,
//                                   NameValueCollection queryParameters=null, string userAgent=null)
//        {
//            return
//                Read(new oEmbedRequest(api, url, maxwidth, maxheight, format,
//                                       queryParameters ?? new NameValueCollection(), userAgent));
//        }
        public oEmbedResponse Read(oEmbedRequest request)
        {
            var response = new oEmbedResponse();
            if (!string.IsNullOrEmpty(request.Api) &&
                !string.IsNullOrEmpty(request.Url))
            {
                response = GetQueryResult(request);
                if (response.oEmbed != null)
                {
                    response.oEmbed.Url = response.oEmbed.Url ?? request.Url;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusDescription = "Resource Api or Url is null or empty";
            }
            return response;
        }

        private oEmbedResponse GetQueryResult(oEmbedRequest request)
        {
            if (request.MaxWidth.HasValue && request.QueryParameters.AllKeys.All(x => x != "maxwidth"))
                request.QueryParameters.Add("maxwidth", request.MaxWidth.Value.ToString(CultureInfo.InvariantCulture));
            if (request.MaxHeight.HasValue && request.QueryParameters.AllKeys.All(x => x != "maxheight"))
                request.QueryParameters.Add("maxheight", request.MaxHeight.Value.ToString(CultureInfo.InvariantCulture));
            if (request.Format != oEmbedFormat.Unspecified && request.QueryParameters.AllKeys.All(x => x != "format"))
            if (request.Format != oEmbedFormat.Unspecified)
                request.QueryParameters.Add("format", request.Format.ToString().ToLower());
            var query = new StringBuilder(request.Api + "?url=" + HttpUtility.UrlEncode(request.Url));
            for (var i = 0; i < request.QueryParameters.Count; i++)
            {
                query.Append("&" + request.QueryParameters.GetKey(i) + "=" + request.QueryParameters.Get(i));
            }
            return GetOEmbedResponseFromWeb(request, query.ToString());
        }

        private oEmbedResponse GetOEmbedResponseFromWeb(oEmbedRequest request, string query)
        {

            var readResponse = new oEmbedResponse();
            var webrequest = (HttpWebRequest) WebRequest.Create(query);
            if (request.UserAgent != null)
                webrequest.UserAgent = request.UserAgent;
            if (request.Format == oEmbedFormat.Json) webrequest.Accept = "application/json";
            if (request.Format == oEmbedFormat.Xml) webrequest.Accept = "text/xml";
            using (var response = webrequest.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    readResponse.StatusCode = response.StatusCode;
                    readResponse.StatusDescription = response.StatusDescription;
                    readResponse.RawResult = ReadResponse(response.GetResponseStream());

                    if (response.ContentType.ToLower().Contains("xml"))
                        readResponse.Format = oEmbedFormat.Xml;
                    if (response.ContentType.ToLower().Contains("json"))
                        readResponse.Format = oEmbedFormat.Json;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        readResponse.oEmbed = oEmbedSerializer.Deserialize(readResponse);
                        ValidateResponse(readResponse, request.MaxWidth ?? int.MaxValue,
                                         request.MaxHeight ?? int.MaxValue, request.Format);
                    }
                    response.Close();
                }
            }

            return readResponse;
        }

        private static string ReadResponse(Stream responseStream)
        {
            using (var reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd().Trim();
            }
        }

        private void ValidateResponse(oEmbedResponse response, int maxwidth, int maxheight, oEmbedFormat requestFormat)
        {
            response.BrokenRules.AddRange(ValidateBasic(response));
            if (!response.IsValid) return;
            response.BrokenRules.AddRange(ValidateThumbnail(response, maxheight, maxwidth));
            response.BrokenRules.AddRange(ValidateFormat(response, requestFormat));
            response.BrokenRules.AddRange(ValidateType(response, maxheight, maxwidth));
            response.BrokenRules.AddRange(ValidateSize(response, maxheight, maxwidth));
        }

        private IList<ValidationRule> ValidateType(oEmbedResponse response, int maxheight, int maxwidth)
        {
            var brokenRules = new List<ValidationRule>();
            switch (response.oEmbed.Type)
            {
                case oEmbedType.Photo:
                    brokenRules.AddRange(ValidateValue(response.oEmbed.Url, response));
                    break;
                case oEmbedType.Rich:
                case oEmbedType.Video:
                    brokenRules.AddRange(ValidateValue(response.oEmbed.Html, response));
                    break;
                case oEmbedType.Link:
                    break;
                case oEmbedType.Unspecified:
                    brokenRules.Add(new ValidationRule("Type", "Response Type must be Photo, Rich, Video, or Link"));
                    break;
            }
            return brokenRules;
            }

        private static IEnumerable<ValidationRule> ValidateValue(string requiredValue, oEmbedResponse response)
        {
            var brokenRules = new List<ValidationRule>();
            if (string.IsNullOrEmpty(requiredValue))
                brokenRules.Add(new ValidationRule("Required Value",
                                                   string.Format(
                                                       "Response is missing required value for {0} for type {1}",
                                                       requiredValue, response.oEmbed.Type)));
            return brokenRules;
        }

        private static IEnumerable<ValidationRule> ValidateSize(oEmbedResponse response, int maxheight, int maxwidth)
        {
            var brokenRules = new List<ValidationRule>();
            if (response.oEmbed.Height <= 0 || response.oEmbed.Height > maxheight)
                brokenRules.Add(new ValidationRule("Size",
                                                   string.Format(
                                                       "Height of {0} is greater than requested maxheight of {1}",
                                                       response.oEmbed.Height, maxheight)));
            if (response.oEmbed.Width <= 0 || response.oEmbed.Width > maxwidth)
                brokenRules.Add(new ValidationRule("Size",
                                                   string.Format(
                                                       "Width of {0} is greater than requested maxwidth of {1}",
                                                       response.oEmbed.Width, maxwidth)));
            return brokenRules;
        }

        private static IEnumerable<ValidationRule> ValidateFormat(oEmbedResponse response, oEmbedFormat requestFormat)
        {
            var brokenRules = new List<ValidationRule>();
            if (requestFormat != oEmbedFormat.Unspecified &&
                requestFormat != response.Format)
                brokenRules.Add(new ValidationRule("Format", "Requested format (" + requestFormat.ToString().ToLower() +
                                                             ") does not match response (" +
                                                             response.Format.ToString().ToLower() + ")"));
            return brokenRules;
        }

        private static IEnumerable<ValidationRule> ValidateThumbnail(oEmbedResponse response, int maxheight, int maxwidth)
        {
            var brokenRules = new List<ValidationRule>();
            if (!string.IsNullOrEmpty(response.oEmbed.ThumbnailUrl)
                && (response.oEmbed.ThumbnailWidth == 0 ||
                    response.oEmbed.ThumbnailHeight == 0))
                brokenRules.Add(new ValidationRule("Thumbnail",
                                                   "ThumbnailUrl requires ThumbnailWidth and ThumbnailHeight"));
            if (response.oEmbed.ThumbnailWidth > 0
                && (string.IsNullOrEmpty(response.oEmbed.ThumbnailUrl) ||
                    response.oEmbed.ThumbnailHeight == 0))
                brokenRules.Add(new ValidationRule("Thumbnail",
                                                   "ThumbnailWidth requires ThumbnailUrl and ThumbnailHeight"));
            if (response.oEmbed.ThumbnailHeight > 0
                && (string.IsNullOrEmpty(response.oEmbed.ThumbnailUrl) ||
                    response.oEmbed.ThumbnailWidth == 0))
                brokenRules.Add(new ValidationRule("Thumbnail",
                                                   "ThumbnailHeight requires ThumbnailUrl and ThumbnailWidth"));
            if (response.oEmbed.ThumbnailHeight > maxheight)
                brokenRules.Add(new ValidationRule("Thumbnail",
                                                   string.Format(
                                                       "ThumbnailHeight of {0} greater than requested maxheight of {1}",
                                                       response.oEmbed.ThumbnailHeight,
                                                       maxheight)));
            if (response.oEmbed.ThumbnailWidth > maxwidth)
                brokenRules.Add(new ValidationRule("Thumbnail",
                                                   string.Format(
                                                       "ThumbnailWidth of {0} is greater than requested maxwidth of {1}",
                                                       response.oEmbed.ThumbnailWidth,
                                                       maxwidth)));

            return brokenRules;
        }

        private static IEnumerable<ValidationRule> ValidateBasic(oEmbedResponse response)
        {
            var brokenRules = new List<ValidationRule>();
            if (response.oEmbed == null)
            {
                brokenRules.Add(new ValidationRule("Response Content Body", "Could not parse response"));
                return brokenRules;
            }
            if (response.oEmbed.Version != "1.0")
                brokenRules.Add(new ValidationRule("Version",
                                                   string.Format("Version number must be 1.0, but instead was {0}",
                                                                 response.oEmbed.Version)));
            return brokenRules;
        }
    }
}