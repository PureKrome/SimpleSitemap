using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SimpleSiteMap.Service
{
    public class SitemapService
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        /// <summary>
        /// Creates a Sitemap or a Urlset from the given collection data.
        /// </summary>
        /// <remarks>the collection data can be either the <i>full</i> dataset or a <i>partitioned</i> dataset. If the data is a full dataset, then a page size is required.</remarks>
        /// <param name="sitemapNodes">the data collection.</param>
        /// <param name="pageSize">how many nodes per page. This is <b>required</b> when the data is not already partitioned.</param>
        /// <returns></returns>
        public string ConvertToXmlSitemapOrUrlset(IList<SitemapNode> sitemapNodes, int? pageSize = null)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException("sitemapNodes");
            }

            if (pageSize.HasValue &&
                pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize");
            }

            if (pageSize.HasValue &&
                pageSize > 50000)
            {
                throw new ArgumentOutOfRangeException("pageSize",
                    "PageSize argument is too large. Search engines have a common restriction of 50,000 items per 'page' (and also usually 10mb). Please reduce this pageSize value to a number <= 50,000.");
            }

            // We display either the full result OR the index with paged links.
            var root = (!pageSize.HasValue &&
                        sitemapNodes.Any()) ||
                       (pageSize.HasValue &&
                        sitemapNodes.Count > pageSize)
                ? CreateXmlSitemapIndex(sitemapNodes, pageSize)
                : CreateXmlUrlSet(sitemapNodes);

            // Convert the xml to a string.
            string xmlResult;
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    root.Save(writer);
                }

                xmlResult = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            if (!string.IsNullOrWhiteSpace(xmlResult) &&
                xmlResult.StartsWith(_byteOrderMarkUtf8))
            {
                xmlResult = xmlResult.Remove(0, _byteOrderMarkUtf8.Length);
            }

            return xmlResult;
        }

        private static XElement CreateXmlSitemapIndex(IList<SitemapNode> sitemapNodes, int? pageSize)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException();
            }

            XNamespace xmlns = SitemapsNamespace;

            var root = new XElement(xmlns + "sitemapindex");

            if (sitemapNodes.Any())
            {
                // If the collection isn't already partitioned into the 'pages', then we need to
                // calculate that now.
                var partitionedNodes = !pageSize.HasValue
                    ? sitemapNodes
                    : sitemapNodes
                        .Select((i, index) => new
                        {
                            i,
                            index
                        })
                        .GroupBy(group => group.index/pageSize, element => element.i)
                        .Select(x => x.First())
                        .ToList();

                // Display each 'page' link.
                for (int i = 0; i < partitionedNodes.Count; i++)
                {
                    var loc = new XElement(xmlns + "loc",
                        Uri.EscapeUriString(string.Format("{0}/?page={1}", partitionedNodes[i].Url, i + 1)));

                    var lastMod = new XElement(xmlns + "lastmod",
                        partitionedNodes[i].LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));

                    root.Add(new XElement(xmlns + "sitemap", loc, lastMod));
                }
            }

            return root;
        }

        private static XElement CreateXmlUrlSet(IEnumerable<SitemapNode> sitemapNodes)
        {
            if (sitemapNodes == null)
            {
                throw new ArgumentNullException();
            }

            XNamespace xmlns = SitemapsNamespace;

            var root = new XElement(xmlns + "urlset");

            foreach (var node in sitemapNodes)
            {
                var loc = new XElement(xmlns + "loc", Uri.EscapeUriString(node.Url.AbsoluteUri));
                var lastMod = new XElement(xmlns + "lastmod",
                    node.LastModified.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
                var changeFrequency = new XElement(xmlns + "changefreq", node.Frequency.ToString().ToLowerInvariant());
                var priority = new XElement(xmlns + "priority", node.Priority.ToString().ToLowerInvariant());

                root.Add(new XElement(xmlns + "url",
                    loc,
                    lastMod,
                    changeFrequency,
                    priority));
            }

            return root;
        }
    }
}