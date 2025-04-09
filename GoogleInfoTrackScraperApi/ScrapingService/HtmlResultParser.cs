using System.Text.RegularExpressions;

namespace ScrapingService;

public static class HtmlResultParser
{
    public static List<string> ParseHtml(string html)
    {
        // Regular expression to match google results list
        var pattern = @"<a class=""fuLhoc ZWRArf"".*?href=""(.*?)"".*?>(.*?)<\/a>";
        
        var regex = new Regex(pattern, RegexOptions.Singleline);
        var matches = regex.Matches(html);
        
        // Extract the urls from the results
        // We will later compare the urls to our target url and add its index to our list
        var results = new List<string>();
        foreach (Match match in matches)
        {
            var url = StripHtmlTags(match.Groups[1].Value.Trim());
            results.Add(url);
        }

        return results;
    }
    
    private static string StripHtmlTags(string input)
    {
        // Strip all HTML tags from the input string
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
    
}