using System;

namespace OptionStrict.oEmbed.MVC
{
    public class NullOEmbedReaderException : Exception
    {
        private const string NullReaderMessage = "An instance of IoEmbedReader was not registered with the current DependencyResolver."
                                                 +" The oEmbed HtmlHelper requires a registered IoEmbedReader instance.";

        public NullOEmbedReaderException() : base(NullReaderMessage) { }
    }
}