using System;
using System.Collections.Generic;

namespace SimpleSiteMap.Tests
{
    internal static class SimpleSiteMapHelpers
    {
        internal static IList<SitemapNode> CreateFakeSitemapNodes(int numberOfNodes,
            DateTime startTime,
            SitemapFrequency? frequency = SitemapFrequency.Daily,
            double? priority = 0.5)
        {
            var result = new List<SitemapNode>();

            for (var i = 0; i < numberOfNodes; i++)
            {
                result.Add(new SitemapNode(
                    new Uri("http://www.foo.com/sitemap/foos"),
                    startTime.AddSeconds(-i), frequency, priority));
            }

            return result;
        }

    }
}
