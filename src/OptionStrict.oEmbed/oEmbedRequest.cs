using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OptionStrict.oEmbed
{
    public class oEmbedRequest
    {
        public string Api { get; set; }
        public string Url { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public oEmbedFormat Format { get; set; }
        public NameValueCollection QueryParameters { get; set; }
        public string UserAgent { get; set; }

        public oEmbedRequest(string api, string url, int? maxWidth, int? maxHeight, oEmbedFormat format, NameValueCollection queryParameters, string userAgent)
        {
            Api = api;
            Url = url;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            Format = format;
            QueryParameters = queryParameters??new NameValueCollection();
            UserAgent = userAgent;
        }

        public oEmbedRequest(string api, string url)
        {
            Api = api;
            Url = url;
            QueryParameters = new NameValueCollection();
            Format = oEmbedFormat.Json;
        }

        private oEmbedRequest()
        {
        }

        public override string ToString()
        {
            var url = new StringBuilder(string.Format("{0}?url={1}",Api, Url));
            if (MaxWidth.HasValue) url.AppendFormat("&maxwidth={0}", MaxWidth);
            if (MaxHeight.HasValue) url.AppendFormat("&maxheight={0}", MaxHeight);
            if (Format != oEmbedFormat.Unspecified) url.AppendFormat("&format={0}", Format);
            if (QueryParameters.HasKeys())
            {
                foreach (var key in QueryParameters.AllKeys)
                {
                    url.AppendFormat("&{0}={1}", key,QueryParameters[key]);    
                }
            }
            if (!string.IsNullOrEmpty(UserAgent.Trim())) url.AppendFormat("&useragent={0}", UserAgent);
            return url.ToString();
        }

        public void AddQueryParameters(NameValueCollection queryString)
        {
            foreach (var key in queryString.AllKeys)
            {
                if (key.ToLower().In("api","url")) continue;
                var index = key;
                var propertyInfos = GetType().GetProperties();
                var property = propertyInfos
                    .SingleOrDefault(x => x.Name.ToLower() == index.ToLower());
                

                if (property != null)
                {
                    property.SetValue(this, ParseValue(property, queryString[index]), null);
                }
                else QueryParameters.Add(index, queryString[index]);
            }
        }

        private object ParseValue(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(Nullable<Int32>))
                return value == null ? int.MaxValue : int.Parse(value);
            if (property.PropertyType==typeof(oEmbedFormat))
            {
                return Enum.Parse(typeof (oEmbedFormat), value??"unspecified", true);
            }
            return value;
        }

    }
}