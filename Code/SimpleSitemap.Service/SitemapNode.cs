using System;

namespace SimpleSiteMap.Service
{
    public class SitemapNode
    {
        public SitemapNode(Uri url,
            DateTime lastModified,
            SitemapFrequency? frequency = SitemapFrequency.Daily,
            double? priority = 0.5)
        {
            Url = url;
            Priority = priority;
            Frequency = frequency;
            LastModified = lastModified;
        }

        public Uri Url { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public SitemapFrequency? Frequency { get; set; }
        public double? Priority { get; set; }

        public override string ToString()
        {
            return $"{(Url == null ? "--no url set" : Url.AbsoluteUri)}; {LastModified}";
        }
    }
}