using System;
using System.Collections.Generic;
using System.IO;
using Shouldly;
using SimpleSitemap.Core;
using Xunit;

namespace SimpleSiteMap.Tests
{
    public class SitemapServiceFacts
    {
        public class ConvertToSitemapFacts
        {
            private static ICollection<SitemapNode> CreateFakeSitemapNodes(int numberOfNodes, DateTime startTime)
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
            public void GivenSomDataThatIsLargerThanASinglePage_ConvertToSiteMapFacts_CreatesASiteMapWithNoNodes()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(100, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSiteMap(data, 10);

                // Assert.
                var resultXml = File.ReadAllText("Result Data\\100ItemsWithPageSize10.xml");

                // My sample data is a bit messed up - which is why I have to do the `replace`.
                result.Replace("\r", string.Empty).ShouldBe(resultXml);
            }

            [Fact]
            public void GivenSomeDataThatIsTheSameSizeAsASinglePage_ConvertToSiteMapFacts_CreatesASiteMapWithNoNodes()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(2, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSiteMap(data, 2);

                // Assert.
                var resultXml = File.ReadAllText("Result Data\\2ItemsWithPageSize2.xml");

                // My sample data is a bit messed up - which is why I have to do the `replace`.
                result.Replace("\r", string.Empty).ShouldBe(resultXml);
            }

            [Fact]
            public void GivenNoData_ConvertToSiteMapFacts_CreatesASiteMapWithNoNodes()
            {
                // Arrange.
                var data = CreateFakeSitemapNodes(0, new DateTime(2014, 11, 21, 18, 58, 00));
                var siteMapService = new SitemapService();

                // Act.
                var result = siteMapService.ConvertToXmlSiteMap(data, 10);

                // Assert.
                var resultXml = File.ReadAllText("Result Data\\0ItemsWithPageSize10.xml");

                // My sample data is a bit messed up - which is why I have to do the `replace`.
                result.Replace("\r", string.Empty).ShouldBe(resultXml);
            }
        }
    }
}