![SimpleSitemap](http://i.imgur.com/Dex0etR.png)
---
[![Build status](https://ci.appveyor.com/api/projects/status/93rpnnv58hhxo9fi?svg=true)](https://ci.appveyor.com/project/PureKrome/simplesitemap) [![](http://img.shields.io/nuget/v/SimpleSitemap.svg?style=flat-square)](http://www.nuget.org/packages/SimpleSitemap/) ![](http://img.shields.io/nuget/dt/SimpleSitemap.svg?style=flat-square)

This simple package helps generate search engine sitemap's for ASP.NET web applications.

---

## Installation

[![](http://i.imgur.com/vig3SXL.png)](https://www.nuget.org/packages/SimpleSitemap/)

Package Name: `SimpleSitemap`  
CLI: `install-package SimpleSitemap`  

---

What is a Sitemap? Are sitemaps and robots.txt the same thing? Why do I need a sitemap at all?

The wiki [has answers all these questions](https://github.com/PureKrome/SimpleSitemap/wiki).
    
## Code examples:
- [Start out generating a Sitemap Index.](https://github.com/PureKrome/SimpleSitemap/wiki/sitemap-index-example).
- [Next, create the Urlset list.](https://github.com/PureKrome/SimpleSitemap/wiki/urlset-example).

---
Output examples:

```xml
    <sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
        <sitemap>
            <loc>http://www.myWebSite.com/sitemap/products/?page=1</loc>
            <lastmod>2014-11-24T09:22-08:00</lastmod>
        </sitemap>
        <sitemap>...</sitemap>
        ....
    </sitemapindex>
```

or

```xml
    <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
        <url>
            <loc>http://www.myWebSite.com/products/1</loc>
            <lastmod>2014-11-21T06:31-08:00</lastmod>
            <changefreq>hourly</changefreq>
            <priority>1</priority>
        </url>
        <url>....</url>
        ....
    </urlset>
```

---
Robots will :heart: you!  
![](http://i.giphy.com/rSCVJasn8uZP2.gif)

---
[![I'm happy to accept tips](http://img.shields.io/gittip/purekrome.svg?style=flat-square)](https://gratipay.com/PureKrome/)  
![Lic: MIT](http://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)
