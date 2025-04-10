using GoogleAppearances.Services;
using NUnit.Framework;

namespace GoogleAppearances.Tests;

[TestFixture]
public class HtmlResultParserTests
{
    [Test]
    public void ParseHtml_ValidHtmlWithResults_ReturnsListOfUrls()
    {
        // Arrange
        var html = @"
            <html>
                <body>
                    <a class=""fuLhoc ZWRArf"" href=""http://example1.com"">Example 1</a>
                    <a class=""fuLhoc ZWRArf"" href=""http://example2.com"">Example 2</a>
                </body>
            </html>";

        // Act
        var result = HtmlResultParser.ParseHtml(html);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0], Is.EqualTo("http://example1.com"));
        Assert.That(result[1], Is.EqualTo("http://example2.com"));
    }

    [Test]
    public void ParseHtml_EmptyHtml_ReturnsEmptyList()
    {
        // Arrange
        var html = "";

        // Act
        var result = HtmlResultParser.ParseHtml(html);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ParseHtml_HtmlWithoutMatchingResults_ReturnsEmptyList()
    {
        // Arrange
        var html = @"
            <html>
                <body>
                    <a href=""http://example1.com"">Example 1</a>
                    <div class=""notTheClassYouAreLookingFor"">No results here</div>
                </body>
            </html>";

        // Act
        var result = HtmlResultParser.ParseHtml(html);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ParseHtml_HtmlWithMalformedHref_ReturnsUrlsWithPlaceholder()
    {
        // Arrange
        var html = @"
            <html>
                <body>
                    <a class=""fuLhoc ZWRArf"" href="""">Empty Href</a>
                    <a class=""fuLhoc ZWRArf"" href=""http://example2.com"">Example 2</a>
                </body>
            </html>";

        // Act
        var result = HtmlResultParser.ParseHtml(html);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0], Is.EqualTo("")); // Empty/malformed href is preserved
        Assert.That(result[1], Is.EqualTo("http://example2.com"));
    }

    [Test]
    public void ParseHtml_HtmlWithMultipleSpacesOrTags_ReturnsCleanedUrls()
    {
        // Arrange
        var html = @"
            <html>
                <body>
                    <a class=""fuLhoc ZWRArf"" href=""  http://example1.com  ""> Example 1 </a>
                    <a class=""fuLhoc ZWRArf"" href=""http://example2.com"">Example 2</a>
                </body>
            </html>";

        // Act
        var result = HtmlResultParser.ParseHtml(html);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0], Is.EqualTo("http://example1.com"));
        Assert.That(result[1], Is.EqualTo("http://example2.com"));
    }

    [Test]
    public void StripHtmlTags_InputWithTags_RemovesAllHtmlTags()
    {
        // Arrange
        var input = "<div>Hello <b>world</b>!</div>";

        // Act
        var result = HtmlResultParser.StripHtmlTags(input);

        // Assert
        Assert.That(result, Is.EqualTo("Hello world!"));
    }

    [Test]
    public void StripHtmlTags_InputWithoutTags_ReturnsSameString()
    {
        // Arrange
        var input = "No HTML tags here";

        // Act
        var result = HtmlResultParser.StripHtmlTags(input);

        // Assert
        Assert.That(result, Is.EqualTo("No HTML tags here"));
    }

    [Test]
    public void StripHtmlTags_EmptyInput_ReturnsEmptyString()
    {
        // Arrange
        var input = "";

        // Act
        var result = HtmlResultParser.StripHtmlTags(input);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
