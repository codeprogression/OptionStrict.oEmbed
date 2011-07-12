/* This oEmbed library is based on the OptionStrict.oEmbed library created by Cory Isakson. (blog.coryisakson.com) */

using System.Collections.Generic;
using System.Net;

namespace OptionStrict.oEmbed
{
    public class oEmbedResponse
    {
        public oEmbedResponse()
        {
            BrokenRules = new List<ValidationRule>();
        }

        public string RawResult { get; set; }

        public oEmbedFormat Format { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public oEmbed oEmbed { get; set; }

        public List<ValidationRule> BrokenRules { get; set; }

        public bool IsValid { get { return BrokenRules.Count == 0; } }
    }
}