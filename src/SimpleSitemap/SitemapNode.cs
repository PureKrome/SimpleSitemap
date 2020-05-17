using System;

namespace SimpleSiteMap
{
    public class SitemapNode
    {
        public SitemapNode(Uri url,
            DateTime lastModified,
            SitemapFrequency? frequency = SitemapFrequency.Daily,
            double? priority = 0.5,
            bool appendPageQueryParam = true)
        {
            Url = url;
            Priority = priority;
            Frequency = frequency;
            LastModified = lastModified;
            AppendPageQueryParam = appendPageQueryParam;
        }

        public Uri Url { get; }
        
        public DateTimeOffset LastModified { get; }
        
        public SitemapFrequency? Frequency { get; }

        public double? Priority { get; }
        
        public bool AppendPageQueryParam { get; } 

        public override string ToString()
        {
            return $"{(Url == null ? "--no url set" : Url.AbsoluteUri)}; {LastModified}";
        }
    }
}