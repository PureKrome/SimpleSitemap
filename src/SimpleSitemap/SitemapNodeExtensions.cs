using System.Collections.Generic;
using System.Linq;

namespace SimpleSiteMap
{
    public static class SitemapNodeExtensions
    {
        /// <summary>
        /// Returns a Partitioned collection of sitemapnodes. 
        /// </summary>
        /// <param name="sitemapNodes">the source sitemap nodes.</param>
        /// <param name="pageSize">the page size of the partitioning.</param>
        /// <returns>NOTICE/WARNING: this extension shouldn't really be used in production. Why? Because you shouldn't be returning -all- the records from your repository .. but the *partitioned results* from the repository. Imagine you have 1mil results, paritioned by 25,000 records-per-section? That's a huge amount of records to return over the wire BEFORE this method breaks them up. So when is this good to be used? In your unit tests against your fake data :)</returns>
        public static ICollection<SitemapNode> ToPartition(this IEnumerable<SitemapNode> sitemapNodes, int pageSize)
        {
            return sitemapNodes
                .OrderByDescending(d => d.LastModified)
                .Select((i, index) => new
                {
                    i,
                    index
                })
                .GroupBy(group => group.index/pageSize, element => element.i)
                .Select(x => x.First())
                .ToList();
        }
    }
}