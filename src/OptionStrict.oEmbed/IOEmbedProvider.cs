using System;

namespace OptionStrict.oEmbed
{
    public interface IOEmbedProvider
    {
        Guid Id { get; }
        bool Matches(Guid id);
        bool Matches(string url);
        string Api { get; }
        string Scheme { get; }

        /// <summary>
        /// Builds the oembed request based on the supplied parameter(s).
        /// </summary>
        /// <param name="fileId">The file id for the resource</param>
        /// <returns>oEmbedRequest object</returns>
        oEmbedRequest GetOEmbedRequest(string fileId);
    }
}