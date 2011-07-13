using System.Web;
using System.Web.Mvc;

namespace OptionStrict.oEmbed.Example.Models
{
    public static class Extensions
    {
        public static MvcHtmlString WrappedHtml(this oEmbed oEmbedResult)
        {
            string result;
            switch (oEmbedResult.Type)
            {
                case oEmbedType.Photo:
                    result = "<img src='" + oEmbedResult.Url + "' height='" + oEmbedResult.Height + "px' width='" +
                             oEmbedResult.Width + "px' alt='" + oEmbedResult.Title + "' />";
                    break;
                case oEmbedType.Link:
                    result = "<a href='" + oEmbedResult.Url + "'>" + oEmbedResult.Title ?? oEmbedResult.Url + "</a>";
                    break;
                default:
                    result = oEmbedResult.Html;
                    break;
            }
            return new MvcHtmlString(result);
        }
    }
}