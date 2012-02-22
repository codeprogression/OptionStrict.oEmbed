using System.Web;
using System.Web.Mvc;
using OptionStrict.oEmbed.Example.Models;

namespace OptionStrict.oEmbed.Example.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        readonly IoEmbedReader _reader;

        public HomeController(IoEmbedReader reader)
        {
            _reader = reader;
        }

        public ActionResult Index(string api, string url)
        {
            return View();
        }
        public ActionResult Oembed(string api, string url)
        {
            if (string.IsNullOrEmpty(api) || string.IsNullOrEmpty(url))
                return View();

            var oembedResponse =
                (_reader.Read(api, url, null, null, oEmbedFormat.Json, null, Request.Headers["User-Agent"]));
            var model = new OembedView
                        {
                            RawResult = oembedResponse.RawResult,
                            Format = oembedResponse.Format.ToString(),
                            AuthorName = oembedResponse.oEmbed.AuthorName,
                            Type = oembedResponse.oEmbed.Type.ToString(),
                            Url = HttpUtility.UrlDecode(oembedResponse.oEmbed.Url),
                            Html = oembedResponse.oEmbed.Html
                        };
            model.PrettyPrint();
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}