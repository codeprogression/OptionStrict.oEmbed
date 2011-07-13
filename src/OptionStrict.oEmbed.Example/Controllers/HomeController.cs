using System.Web.Mvc;

namespace OptionStrict.oEmbed.Example.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        readonly IoEmbedWriter _writer;
        readonly IoEmbedReader _reader;

        public HomeController(IoEmbedWriter writer, IoEmbedReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public ActionResult Index()
        {
            return View(new oEmbedResponse { oEmbed = new oEmbed() });
        }

        public ActionResult About()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string api, string url)
        {
            var oembedResponse =
                (_reader.Read(api, url, null, null, oEmbedFormat.Json, null, Request.Headers["User-Agent"]));
            _writer.WriteResponse(oembedResponse.oEmbed, oEmbedFormat.Xml);
            return View(oembedResponse);
        }
    }
}