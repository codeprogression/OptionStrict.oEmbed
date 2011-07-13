using Machine.Specifications;

namespace OptionStrict.oEmbed.Specs
{
    [Subject("Response")]
    public class When_Response_Is_Link : oEmbedReaderTestBase
    {
        protected static oEmbed oEmbed;

        private Because of = () =>
        {
            FakeOEmbed = GetFakeOEmbed();
            FakeOEmbed.Type = oEmbedType.Link;
            XmlResponse = oEmbedSerializer.SerializeXml(FakeOEmbed);
            oEmbed = oEmbedSerializer.DeserializeXml(XmlResponse);
        };

        private It should_be_link_type = () => oEmbed.Type.ShouldEqual(oEmbedType.Link);
    }
}