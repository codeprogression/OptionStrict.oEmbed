using Machine.Specifications;

namespace OptionStrict.oEmbed.Specs
{
    public abstract class oEmbedReaderTestBase
    {
        protected static string JsonResponse;
        protected static oEmbedResponse Response;
        protected static string XmlResponse;
        protected static oEmbedXmlForSerialization FakeOEmbed;

        protected static Establish before_each = () => { XmlResponse = oEmbedSerializer.SerializeXml(GetFakeOEmbed()); };

        public static oEmbedXmlForSerialization GetFakeOEmbed()
        {
            return new oEmbedXmlForSerialization
            {
                AuthorName = "OptionStrict",
                AuthorUrl = "blog.coryisakson.com",
                CacheAge = 500,
                Height = 100,
                Width = 200,
                Html =
                    "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"437\" height=\"288\" id=\"viddlerplayer-1646c55\"><param name=\"movie\" value=\"http://www.viddler.com/player/1646c55/\" /><param name=\"allowScriptAccess\" value=\"always\" /><param name=\"wmode\" value=\"transparent\" /><param name=\"allowFullScreen\" value=\"true\" /><embed src=\"http://www.viddler.com/player/1646c55/\" width=\"437\" height=\"288\" type=\"application/x-shockwave-flash\" allowScriptAccess=\"always\" allowFullScreen=\"true\" wmode=\"transparent\" name=\"viddlerplayer-1646c55\" ></embed></object>",
                ProviderName = "mspec",
                ProviderUrl = "http://www.mspec.com",
                ThumbnailHeight = 101,
                ThumbnailWidth = 201,
                ThumbnailUrl = "http://www.mspec.com/thumb.jpg",
                Version = "1.0",
                Title = "Sample",
                Type = oEmbedType.Rich,
                Url = "http://blog.optionstrict.com/post/12345"
            };
        }
    }
}