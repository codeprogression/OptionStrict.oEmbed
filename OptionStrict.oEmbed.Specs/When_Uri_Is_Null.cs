using System.Net;

namespace OptionStrict.oEmbed.Specs
{
    [Subject("Url")]
    public class When_Uri_Is_Null : oEmbedReaderTestBase
    {
        private Because of = () =>
        {
            var reader = new oEmbedReader();
            Response = reader.Read(null, null);
        };

        private It should_have_status_code_BadRequest = () => Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
    }
}