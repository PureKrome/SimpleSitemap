using System;

namespace SimpleSiteMap.Service
{
    public class SitemapNode
    {
        public SitemapNode(Uri url,
            DateTime lastModified)
        {
            Url = url;
            Priority = 0.5;
            Frequency = SitemapFrequency.Daily;
            LastModified = lastModified;
        }

        public Uri Url { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public SitemapFrequency Frequency { get; set; }
        public double Priority { get; set; }

        public override string ToString()
        {
            return string.Format("{0}; {1}",
                Url == null
                    ? "--no url set"
                    : Url.AbsoluteUri,
                LastModified);
        }
    }
}