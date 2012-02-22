using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace OptionStrict.oEmbed
{
    public class oEmbedJavaScriptConverter : JavaScriptConverter
    {
        static readonly Type[] _supportedTypes = new[]
                                                     {
                                                         typeof (oEmbed)
                                                     };

        public override IEnumerable<Type> SupportedTypes
        {
            get { return _supportedTypes; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary,
                                           Type type,
                                           JavaScriptSerializer serializer)
        {
            if (type == typeof (oEmbed))
            {
                var obj = new oEmbed();

                if (dictionary.ContainsKey("version"))
                    obj.Version = serializer.ConvertToType<string>(dictionary["version"]);
                if (dictionary.ContainsKey("title"))
                    obj.Title = serializer.ConvertToType<string>(dictionary["title"]);
                if (dictionary.ContainsKey("type"))
                    obj.Type = serializer.ConvertToType<oEmbedType>(dictionary["type"]);
                if (dictionary.ContainsKey("author_name"))
                    obj.AuthorName = serializer.ConvertToType<string>(dictionary["author_name"]);
                if (dictionary.ContainsKey("author_url"))
                    obj.AuthorUrl = serializer.ConvertToType<string>(dictionary["author_url"]);
                if (dictionary.ContainsKey("provider_name"))
                    obj.ProviderName = serializer.ConvertToType<string>(dictionary["provider_name"]);
                if (dictionary.ContainsKey("provider_url"))
                    obj.ProviderUrl = serializer.ConvertToType<string>(dictionary["provider_url"]);
                if (dictionary.ContainsKey("cache_age"))
                    obj.CacheAge = serializer.ConvertToType<int>(dictionary["cache_age"]);
                if (dictionary.ContainsKey("thumbnail_url"))
                    obj.ThumbnailUrl = serializer.ConvertToType<string>(dictionary["thumbnail_url"]);
                if (dictionary.ContainsKey("thumbnail_width"))
                    obj.ThumbnailWidth = serializer.ConvertToType<int?>(dictionary["thumbnail_width"]);
                if (dictionary.ContainsKey("thumbnail_height"))
                    obj.ThumbnailHeight = serializer.ConvertToType<int?>(dictionary["thumbnail_height"]);
                if (dictionary.ContainsKey("url"))
                    obj.Url = serializer.ConvertToType<string>(dictionary["url"]);
                if (dictionary.ContainsKey("width") && dictionary["width"] != null)
                    obj.Width = serializer.ConvertToType<int>(dictionary["width"]);
                if (dictionary.ContainsKey("height") && dictionary["height"] != null)
                    obj.Height = serializer.ConvertToType<int>(dictionary["height"]);
                if (dictionary.ContainsKey("html"))
                    obj.Html = serializer.ConvertToType<string>(dictionary["html"]);


                return obj;
            }

            return null;
        }

        public override IDictionary<string, object> Serialize(
            object obj,
            JavaScriptSerializer serializer)
        {
            var dataObj = serializer.ConvertToType<oEmbed>(obj);
            if (dataObj != null)
            {
                return new Dictionary<string, object>
                           {
                               {"type", dataObj.Type.ToString().ToLower()},
                               {"version", dataObj.Version},
                               {"title", dataObj.Title},
                               {"author_name", dataObj.AuthorName},
                               {"author_url", dataObj.AuthorUrl},
                               {"provider_name", dataObj.ProviderName},
                               {"provider_url", dataObj.ProviderUrl},
                               {"cache_age", dataObj.CacheAge},
                               {"thumbnail_url", dataObj.ThumbnailUrl},
                               {"thumbnail_width", dataObj.ThumbnailWidth},
                               {"thumbnail_height", dataObj.ThumbnailHeight},
                               {"url", dataObj.Url},
                               {"width", dataObj.Width},
                               {"height", dataObj.Height},
                               {"html", dataObj.Html}
                           };
            }
            return new Dictionary<string, object>();
        }
    }
}