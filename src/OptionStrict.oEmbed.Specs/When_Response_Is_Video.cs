using Machine.Specifications;

namespace OptionStrict.oEmbed.Specs
{
    [Subject("Response")]
    public class When_Response_Is_Video : oEmbedReaderTestBase
    {
        protected static oEmbed oEmbed;

        private Because of = () =>
        {
            FakeOEmbed = GetFakeOEmbed();
            FakeOEmbed.Type = oEmbedType.Video;
            XmlResponse = oEmbedSerializer.SerializeXml(FakeOEmbed);
            oEmbed = oEmbedSerializer.DeserializeXml(XmlResponse);
        };

        private It should_be_video_type = () => oEmbed.Type.ShouldEqual(oEmbedType.Video);
    }
}