namespace OptionStrict.oEmbed.Specs
{
    [Subject("Response")]
    public class When_Response_Is_Json : oEmbedReaderTestBase
    {
        protected static oEmbed oEmbed;

        private Because of = () =>
        {
            FakeOEmbed = GetFakeOEmbed();
            JsonResponse = oEmbedSerializer.SerializeJson(FakeOEmbed);
            oEmbed = oEmbedSerializer.DeserializeJson(JsonResponse);
        };

        private It should_be_rich_type = () => oEmbed.Type.ShouldEqual(FakeOEmbed.Type);
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
        private It should_have_html = () => oEmbed.Html.ShouldEqual(FakeOEmbed.Html);
    }

    [Subject("Write JSON")]
    public class When_Writing_Json
    {
        private static oEmbed FakeOEmbed;
        private static string Response;

        private Because of = () =>
        {
            FakeOEmbed = oEmbedReaderTestBase.GetFakeOEmbed();
            var writer = new oEmbedWriter();
            Response = writer.Write(FakeOEmbed, oEmbedFormat.Json);
        };

        private It should_be_json_deserializable = () => oEmbedSerializer.DeserializeJson(Response).ShouldNotBeNull();
    }
}