namespace OptionStrict.oEmbed.Specs
{
    [Subject("Response")]
    public class When_Response_Is_Photo : oEmbedReaderTestBase
    {
        protected static oEmbed oEmbed;

        private Because of = () =>
        {
            FakeOEmbed = GetFakeOEmbed();
            FakeOEmbed.Type = oEmbedType.Photo;
            XmlResponse = oEmbedSerializer.SerializeXml(FakeOEmbed);
            oEmbed = oEmbedSerializer.DeserializeXml(XmlResponse);
        };

        private It should_be_photo_type = () => oEmbed.Type.ShouldEqual(oEmbedType.Photo);
    }
}