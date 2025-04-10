using System.Net;
using GoogleAppearances.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace GoogleAppearances.Tests;

[TestFixture]
public class GoogleSearchScraperServiceTests
{
    private GoogleSearchScraperService _service;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;

    // Mocked HTML response to return in all HTTP requests
    private const string MockHtmlResponse = @"
            <html>
                <body>
                    <a class=""fuLhoc ZWRArf"" href=""http://example1.com"">Example 1</a>
                    <a class=""fuLhoc ZWRArf"" href=""http://example2.com"">Example 2</a>
                    <a class=""fuLhoc ZWRArf"" href=""https://target.com"">Target</a>
                    <a class=""fuLhoc ZWRArf"" href=""http://fakewebsite.com"">Fake website</a>
                    <a class=""fuLhoc ZWRArf"" href=""https://target.com/Page1"">Target 2</a>
                    <a class=""fuLhoc ZWRArf"" href=""https://anotherfake.com"">Fake website</a>
                </body>
            </html>";

    [SetUp]
    public void SetUp()
    {
        // Mock HttpClient
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(MockHtmlResponse)
            });

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        // Initialize service with mocked HttpClient
        _service = new GoogleSearchScraperService(httpClient);
    }
    
    [Test]
    public void ScrapeGoogleSearchResults_NullSearchQuery_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _service.ScrapeGoogleSearchResults(null, "http://example.com"));
        Assert.That(true,"Search query cannot be null or empty. (Parameter 'searchQuery')", exception.Message);
    }

    [Test]
    public void ScrapeGoogleSearchResults_EmptySearchQuery_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _service.ScrapeGoogleSearchResults(string.Empty, "http://example.com"));
        Assert.That(true,"Search query cannot be null or empty. (Parameter 'searchQuery')", exception.Message);
    }
    
    [Test]
    public void ScrapeGoogleSearchResults_MatchingUrl_ReturnsCorrectIndex()
    {
        var searchQuery = "dummy search";
        var targetUrl = "https://anotherfake.com";

        // Act
        var result = _service.ScrapeGoogleSearchResults(searchQuery, targetUrl);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(5));
    }

    [Test]
    public void ScrapeGoogleSearchResults_MultipleMatchingUrls_ReturnsAllMatchingIndices()
    {
        var searchQuery = "dummy search";
        var targetUrl = "https://target.com";

        // Act
        var result = _service.ScrapeGoogleSearchResults(searchQuery, targetUrl);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Contains(2));
        Assert.That(result.Contains(4));
    }

    [Test]
    public void ScrapeGoogleSearchResults_NoMatchingUrl_ReturnsEmptyList()
    {
        var searchQuery = "dummy search";
        var targetUrl = "https://infotrack.com";

        // Act
        var result = _service.ScrapeGoogleSearchResults(searchQuery, targetUrl);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ScrapeGoogleSearchResults_MatchingUrlCaseInsensitive_ReturnsCorrectIndices()
    {
        var searchQuery = "dummy search";
        var targetUrl = "https://TARGET.com";

        // Act
        var result = _service.ScrapeGoogleSearchResults(searchQuery, targetUrl);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Contains(2));
        Assert.That(result.Contains(4));
    }
}
