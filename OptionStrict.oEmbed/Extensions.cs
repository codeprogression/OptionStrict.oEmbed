/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System;
using System.Text;

namespace OptionStrict.oEmbed
{
    public static class Extensions
    {
        public static oEmbed oEmbed(this  HtmlHelper helper, string api, string url)
        {
            return (Container.GetInstance<IoEmbedReader>()).Read(api, url).oEmbed;
        }

        public static oEmbed oEmbed(this  HtmlHelper helper, Uri api, Uri url)
        {
            return oEmbed(helper,
                api != null ? api.AbsoluteUri : null,
                url != null ? url.AbsoluteUri : null);
        }

        public static string WrappedHtml(this oEmbed oEmbed)
        {
            string result;
            switch (oEmbed.Type)
            {
                case oEmbedType.Photo:
                    var imgpreview = "<div class='{0}' style='background: #000000 url({1}) no-repeat center center; width:{2}px; height:{3}px;'></div>";
                    result = string.Format(imgpreview, "Video", oEmbed.Url, oEmbed.Width, oEmbed.Height);
                    break;
                case oEmbedType.Link:
                    result = "<a href='" + oEmbed.Url + "'>" + oEmbed.Title ?? oEmbed.Url + "</a>";
                    break;
                case oEmbedType.Video:
                    var playerName = "uvp" + oEmbed.ProviderName;
                    var randomId = Guid.NewGuid();
                    var preview = new StringBuilder("<div id='{0}_container' class='{1}'");
                    preview.Append(
                        "style='background: #000000 url({5}) no-repeat scroll 0px center; width: {3}px; height: {4}px;'>");
                    preview.Append("<a id='{0}' rel='{2}' style='display:{7}; height: {4}px; ' ");
                    preview.Append("href='#'>&nbsp;</a><div id='{0}_embed' style='display:{8};'>{6}</div></div>");
                    result = string.Format(preview.ToString(), new object[]
                                                                   {
                                                                       randomId,
                                                                       oEmbed.Type,
                                                                       playerName,
                                                                       oEmbed.Width,
                                                                       oEmbed.Height,
                                                                       oEmbed.ThumbnailUrl,
                                                                       oEmbed.Html,
                                                                       oEmbed.ProviderName!="Twistage"?"none":"block",
                                                                       oEmbed.ProviderName!="Twistage"?"block":"none"
                                                                   });
                    break;
                default:
                    result = oEmbed.Html;
                    break;
            }
            return result;
        }

        

        public static oEmbedFormat GetOEmbedFormat(this string format)
        {
            switch (format.ToLower())
            {
                case "xml":
                    return oEmbedFormat.Xml;
                case "json":
                    return oEmbedFormat.Json;
                case "jsonp":
                    return oEmbedFormat.Jsonp;
                default:
                    return oEmbedFormat.Unspecified;
            }
        }
    }
}
