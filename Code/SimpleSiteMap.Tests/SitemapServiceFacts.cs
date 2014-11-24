using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Shouldly;
using SimpleSiteMap.Service;
using Xunit;

namespace SimpleSiteMap.Tests
{
    public class SitemapServiceFacts
    {
        public class ConvertToSitemapFacts
        {
            private static IList<SitemapNode> CreateFakeSitemapNodes(int numberOfNodes, DateTime startTime)
            {
                var result = new List<SitemapNode>();

                for (int i = 0; i < numberOfNodes; i++)
                {
                    result.Add(new SitemapNode(new Uri("http://www.foo.com/sitemap/foos"))
                    {
                        LastModified = startTime.AddSeconds(-i)
                    });
                }

                return result;
            }

            [Fact]
            public void GivenSomDataThatIsLargerThanASinglePage_ConvertToSiteMap_CreatesASitemapResult()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(100, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSitemapOrUrlset(data, 10);

                // Assert.
                var expectedXml = File.ReadAllText("Result Data\\100ItemsWithPageSize10.xml");

                CompareTwoSitemapDocuments(result, expectedXml);
            }

            [Fact]
            public void GivenSomeDataThatIsTheSameSizeAsASinglePage_ConvertToSiteMap_CreatesAUrlsetResult()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(2, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSitemapOrUrlset(data, 2);

                // Assert.
                var expectedXml = File.ReadAllText("Result Data\\2ItemsWithPageSize2.xml");

                // My sample data is a bit messed up - which is why I have to do the `replace`.
                CompareTwoUrlsetDocuments(result, expectedXml);
            }

            [Fact]
            public void GivenNoData_ConvertToSiteMap_CreatesAUrlsetWithNoNodes()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(0, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSitemapOrUrlset(data, 10);

                // Assert.
                var expectedXml = File.ReadAllText("Result Data\\0ItemsWithPageSize10.xml");

                // My sample data is a bit messed up - which is why I have to do the `replace`.
                CompareTwoUrlsetDocuments(result, expectedXml);
            }

            [Fact]
            public void GivenSomePartitionedData_ConvertToSitemap_CreatesASitemapResult()
            {
                // Arrange.
                const int pageSize = 10;
                var data = CreateFakeSitemapNodes(100, new DateTime(2014, 11, 21, 18, 58, 00));
                // Note: now we partition this data set, with the most recently modified, first.
                var partitionedData = data
                    .OrderByDescending(d => d.LastModified)
                    .Select((i, index) => new
                    {
                        i,
                        index
                    })
                    .GroupBy(group => group.index/pageSize, element => element.i)
                    .Select(x => x.First())
                    .ToList();

                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSitemapOrUrlset(partitionedData);

                // Assert.
                var expectedXml = File.ReadAllText("Result Data\\100ItemsWithPageSize10.xml");

                CompareTwoSitemapDocuments(result, expectedXml);
            }

            [Fact]
            public void GivenNoPartitionedData_ConvertToSitemap_CreatesASitemapResult()
            {
                // Arrange.
                var partitionedData = CreateFakeSitemapNodes(0, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSitemapOrUrlset(partitionedData);

                // Assert.
                var expectedXml = File.ReadAllText("Result Data\\0ItemsWithPageSize10.xml");

                CompareTwoUrlsetDocuments(result, expectedXml);
            }

            private static void CompareTwoSitemapDocuments(string actualXml, string expectedXml)
            {
                actualXml.ShouldNotBeNullOrEmpty();
                expectedXml.ShouldNotBeNullOrEmpty();

                var actualXmlDocument = XDocument.Parse(actualXml);
                var expectedXmlDocument = XDocument.Parse(expectedXml);

                actualXmlDocument.ToString().ShouldStartWith("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

                expectedXmlDocument.DescendantNodes().Count().ShouldBe(actualXmlDocument.DescendantNodes().Count());

                var actuaElements = actualXmlDocument.Root.Elements().ToList();
                var expectedElements = expectedXmlDocument.Root.Elements().ToList();

                for (int i = 0; i < actuaElements.Count ; i++)
                {
                    var actualChildrenElements = actuaElements[i].Elements().ToList();
                    var expectedChildrenElements = expectedElements[i].Elements().ToList();

                    // loc.
                    actualChildrenElements[0].Value.ShouldBe(expectedChildrenElements[0].Value);

                    // lastmod.
                    actualChildrenElements[1].Value.ShouldBe(expectedChildrenElements[1].Value);
                }
            }

            private static void CompareTwoUrlsetDocuments(string actualXml, string expectedXml)
            {
                actualXml.ShouldNotBeNullOrEmpty();
                expectedXml.ShouldNotBeNullOrEmpty();

                var actualXmlDocument = XDocument.Parse(actualXml);
                var expectedXmlDocument = XDocument.Parse(expectedXml);

                actualXmlDocument.ToString().ShouldStartWith("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");

                expectedXmlDocument.DescendantNodes().Count().ShouldBe(actualXmlDocument.DescendantNodes().Count());

                var actuaElements = actualXmlDocument.Root.Elements().ToList();
                var expectedElements = expectedXmlDocument.Root.Elements().ToList();

                for (int i = 0; i < actuaElements.Count; i++)
                {
                    var actualChildrenElements = actuaElements[i].Elements().ToList();
                    var expectedChildrenElements = expectedElements[i].Elements().ToList();

                    // loc.
                    actualChildrenElements[0].Value.ShouldBe(expectedChildrenElements[0].Value);

                    // lastmod.
                    actualChildrenElements[1].Value.ShouldBe(expectedChildrenElements[1].Value);

                    // changefreq.
                    actualChildrenElements[2].Value.ShouldBe(expectedChildrenElements[2].Value);

                    // priority.
                    actualChildrenElements[3].Value.ShouldBe(expectedChildrenElements[3].Value);
                }
            }
        }
    }
}