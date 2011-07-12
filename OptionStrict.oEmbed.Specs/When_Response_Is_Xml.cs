using System.Xml;
using Machine.Specifications;

namespace OptionStrict.oEmbed.Specs
{
    [Subject("Response")]
    public class When_Response_Is_Xml : oEmbedReaderTestBase
    {
        protected static oEmbed oEmbed;

        private Because of = () =>
        {
            FakeOEmbed = GetFakeOEmbed();
            XmlResponse = oEmbedSerializer.SerializeXml(FakeOEmbed);
            oEmbed = oEmbedSerializer.DeserializeXml(XmlResponse);
        };

        private It should_be_rich_type = () => oEmbed.Type.ShouldEqual(oEmbedType.Rich);
        private It should_have_a_cache_age = () => oEmbed.CacheAge.ShouldEqual(FakeOEmbed.CacheAge);
        private It should_have_a_height = () => oEmbed.Height.ShouldEqual(FakeOEmbed.Height);
        private It should_have_a_provider_name = () => oEmbed.ProviderName.ShouldEqual(FakeOEmbed.ProviderName);
        private It should_have_a_provider_url = () => oEmbed.ProviderUrl.ShouldEqual(FakeOEmbed.ProviderUrl);
        private It should_have_a_thumbnail_height = () => oEmbed.ThumbnailHeight.ShouldEqual(FakeOEmbed.ThumbnailHeight);
        private It should_have_a_thumbnail_url = () => oEmbed.ThumbnailUrl.ShouldEqual(FakeOEmbed.ThumbnailUrl);
        private It should_have_a_thumbnail_width = () => oEmbed.ThumbnailWidth.ShouldEqual(FakeOEmbed.ThumbnailWidth);
        private It should_have_a_title = () => oEmbed.Title.ShouldEqual(FakeOEmbed.Title);
        private It should_have_a_url = () => oEmbed.Url.ShouldEqual(FakeOEmbed.Url);
        private It should_have_a_width = () => oEmbed.Width.ShouldEqual(FakeOEmbed.Width);
        private It should_have_an_author_name = () => oEmbed.AuthorName.ShouldEqual(FakeOEmbed.AuthorName);
        private It should_have_an_author_url = () => oEmbed.AuthorUrl.ShouldEqual(FakeOEmbed.AuthorUrl);
        private It should_have_be_version_1_0 = () => oEmbed.Version.ShouldEqual("1.0");

        private It should_have_html = () =>
        {
            var dummy = new XmlDocument();
            XmlNode node = dummy.CreateNode(XmlNodeType.Text, "html", "");
            node.Value = FakeOEmbed.Html;
            oEmbed.Html.ShouldEqual(node.Value);
        };
    }
}