namespace OptionStrict.oEmbed.Specs
{
    public abstract class When_Reading_Live_Sources
    {
        public static oEmbedReader _reader;
        public static oEmbedResponse _result;
        public static string Api;
        public static oEmbedFormat Format;
        public static string Url;
    }

    public class When_Requesting_Xml_Format : When_Reading_Live_Sources
    {
        protected Establish context_each = () =>
        {
            _reader = new oEmbedReader();
            Format = oEmbedFormat.Xml;
        };

        protected Because of = () => _result = _reader.Read(Api, Url, Format);
    }

    public class When_Reading_YouTube_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://www.youtube.com/oembed";
            Url = "http://www.youtube.com/watch?v=-UUx10KOWIE";
        };

        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
        private It should_be_video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);
    }

    public class When_Reading_Flickr_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://www.flickr.com/services/oembed/";
            Url = "http://flickr.com/photos/bees/2362225867/";
        };

        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
        private It should_be_photo_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Photo);
    }

    public class When_Reading_Viddler_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://lab.viddler.com/services/oembed/";
            Url = "http://www.viddler.com/explore/cdevroe/videos/424/";
        };

        private It should_be_Video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_Qik_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://qik.com/api/oembed.xml";
            Url = "http://qik.com/video/49565";
        };

        [Ignore]
        private It should_be_Video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_Revision3_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://revision3.com/api/oembed/";
            Url = "http://revision3.com/diggnation/2008-04-17xsanned/";
        };

        [Ignore]
        private It should_be_Video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);
        [Ignore]
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_Hulu_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://www.hulu.com/api/oembed.xml";
            Url = "http://www.hulu.com/watch/20807/late-night-with-conan-obrien-wed-may-21-2008";
        };

        [Ignore]
        private It should_be_Video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);      
        [Ignore]
		private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_Vimeo_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://www.vimeo.com/api/oembed.xml";
            Url = "http://www.vimeo.com/757219";
        };

        [Ignore]
        private It should_be_Video_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Video);
        [Ignore]
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_MyOpera_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://my.opera.com/service/oembed";
            Url = "http://my.opera.com/cstrep/albums/show.dml?id=504322";
        };

        [Ignore]
        private It should_be_Link_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Link);

        [Ignore]
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }

    public class When_Reading_ClearSpring_Xml_Format : When_Requesting_Xml_Format
    {
        private Establish context = () =>
        {
            Api = "http://widgets.clearspring.com/widget/v1/oembed/";
            Url = "http://www.clearspring.com/widgets/480fbb38b51cb736";
        };

        [Ignore]
        private It should_be_Photo_type = () => _result.oEmbed.Type.ShouldEqual(oEmbedType.Rich);
        [Ignore]
        private It should_be_Xml = () => _result.Format.ShouldEqual(oEmbedFormat.Xml);
    }
}