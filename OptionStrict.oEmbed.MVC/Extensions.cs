using System;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace OptionStrict.oEmbed.MVC
{
    public static class Extensions
    {
        public static oEmbed oEmbed(this  HtmlHelper helper, string api, string url)
        {
            return (ServiceLocator.Current.GetInstance<IoEmbedReader>()).Read(api, url).oEmbed;
        }

        public static oEmbed oEmbed(this  HtmlHelper helper, Uri api, Uri url)
        {
            return oEmbed(helper,
                          api != null ? api.AbsoluteUri : null,
                          url != null ? url.AbsoluteUri : null);
        }
    }
}