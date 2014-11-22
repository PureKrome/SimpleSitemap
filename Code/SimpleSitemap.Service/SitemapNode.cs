using System;

namespace SimpleSiteMap.Service
{
    public class SitemapNode
    {
        public SitemapNode(Uri url)
        {
            Url = url;
            Priority = 0.5;
            Frequency = SitemapFrequency.Daily;
            LastModified = DateTime.UtcNow;
        }

        public Uri Url { get; set; }
        public DateTime LastModified { get; set; }
        public SitemapFrequency Frequency { get; set; }
        public double Priority { get; set; }
    }
}