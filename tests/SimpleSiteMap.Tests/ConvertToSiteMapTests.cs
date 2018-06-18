using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace SimpleSiteMap.Tests
{
    public class ConvertToSiteMapTests
    {
        [Fact]
        public void GivenDataWith10Items_ConvertToSiteMap_CreatesASitemapResult()
        {
            // Arrange.
            var startDate = DateTime.SpecifyKind(new DateTime(2014, 11, 21, 18, 58, 00), DateTimeKind.Utc);
            var data = SimpleSiteMapHelpers.CreateFakeSitemapNodes(100, startDate).ToPartition(10);
            var siteMapService = new SitemapService();

            // Act.
            var result = siteMapService.ConvertToXmlSitemap(data);

            // Assert.
            var expectedXml = File.ReadAllText("Result Data\\SitemapWith10Items.xml");

            CompareTwoSitemapDocuments(result, expectedXml);
        }

        [Fact]
        public void GivenNoPartitionedData_ConvertToSitemap_CreatesASitemapResult()
        {
            // Arrange.r
            var startDate = DateTime.SpecifyKind(new DateTime(2014, 11, 21, 18, 58, 00), DateTimeKind.Utc);
            var partitionedData = SimpleSiteMapHelpers.CreateFakeSitemapNodes(0, startDate);
            var siteMapService = new SitemapService();

            // Act.
            var result = siteMapService.ConvertToXmlSitemap(partitionedData);

            // Assert.
            var expectedXml = File.ReadAllText("Result Data\\SitemapWith0Items.xml");

            CompareTwoSitemapDocuments(result, expectedXml);
        }

        private static void CompareTwoSitemapDocuments(string actualXml, string expectedXml)
        {
            actualXml.ShouldNotBeNullOrEmpty();
            expectedXml.ShouldNotBeNullOrEmpty();

            var actualXmlDocument = XDocument.Parse(actualXml);
            var expectedXmlDocument = XDocument.Parse(expectedXml);

            actualXmlDocument
                .ToString()
                .ShouldStartWith("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");

            expectedXmlDocument
                .DescendantNodes()
                .Count()
                .ShouldBe(actualXmlDocument.DescendantNodes().Count());

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
            }
        }
    }
}
