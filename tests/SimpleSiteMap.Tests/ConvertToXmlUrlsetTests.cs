using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace SimpleSiteMap.Tests
{
    public class ConvertToXmlUrlsetTests
    {
        [Fact]
        public void GivenSomeData_ConvertToSiteMap_CreatesAUrlsetResult()
        {
            // Arrange.
            var startDate = DateTime.SpecifyKind(new DateTime(2014, 11, 21, 18, 58, 00), DateTimeKind.Utc);
            var data = SimpleSiteMapHelpers.CreateFakeSitemapNodes(10, startDate);
            var siteMapService = new SitemapService();

            // Act.
            var result = siteMapService.ConvertToXmlUrlset(data);

            // Assert.
            var expectedXml = File.ReadAllText("Result Data//UrlsetWith10Items.xml");

            // My sample data is a bit messed up - which is why I have to do the `replace`.
            CompareTwoUrlsetDocuments(result, expectedXml);
        }

        [Fact]
        public void GivenSomeDataAndNoChangeFreqOrPrioritySet_ConvertToSiteMap_CreatesAUrlsetResult()
        {
            // Arrange.
            var startDate = DateTime.SpecifyKind(new DateTime(2014, 11, 21, 18, 58, 00), DateTimeKind.Utc);
            var data = SimpleSiteMapHelpers.CreateFakeSitemapNodes(10, startDate, null, null);
            var siteMapService = new SitemapService();

            // Act.
            var result = siteMapService.ConvertToXmlUrlset(data);

            // Assert.
            var expectedXml = File.ReadAllText("Result Data//UrlsetWith10ItemsNoChangeFreqOrPriority.xml");

            // My sample data is a bit messed up - which is why I have to do the `replace`.
            CompareTwoUrlsetDocuments(result, expectedXml);
        }

        [Fact]
        public void GivenNoData_ConvertToSiteMap_CreatesAUrlsetWithNoNodes()
        {
            // Arrange.
            var startDate = DateTime.SpecifyKind(new DateTime(2014, 11, 21, 18, 58, 00), DateTimeKind.Utc);
            var data = SimpleSiteMapHelpers.CreateFakeSitemapNodes(0, startDate);
            var siteMapService = new SitemapService();

            // Act.
            var result = siteMapService.ConvertToXmlUrlset(data);

            // Assert.
            var expectedXml = File.ReadAllText("Result Data//UrlsetWith0Items.xml");

            // My sample data is a bit messed up - which is why I have to do the `replace`.
            CompareTwoUrlsetDocuments(result, expectedXml);
        }

        private static void CompareTwoUrlsetDocuments(string actualXml, string expectedXml)
        {
            actualXml.ShouldNotBeNullOrEmpty();
            expectedXml.ShouldNotBeNullOrEmpty();

            var actualXmlDocument = XDocument.Parse(actualXml);
            var expectedXmlDocument = XDocument.Parse(expectedXml);

            actualXmlDocument.ToString()
                .ShouldStartWith("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"");

            expectedXmlDocument.DescendantNodes().Count().ShouldBe(actualXmlDocument.DescendantNodes().Count());

            var actuaElements = actualXmlDocument.Root.Elements().ToList();
            var expectedElements = expectedXmlDocument.Root.Elements().ToList();

            for (var i = 0; i < actuaElements.Count; i++)
            {
                var actualChildrenElements = actuaElements[i].Elements().ToList();
                var expectedChildrenElements = expectedElements[i].Elements().ToList();

                for (var x = 0; x < expectedChildrenElements.Count; x++)
                {
                    actualChildrenElements[x].Value.ShouldBe(expectedChildrenElements[x].Value);
                }
            }
        }
    }
}
