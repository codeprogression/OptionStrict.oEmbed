using System.Linq;

namespace OptionStrict.oEmbed
{
    public static class Extensions
    {
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

        public static bool In<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
