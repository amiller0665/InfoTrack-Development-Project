namespace GoogleAppearances.Services
{
    public interface IGoogleSearchScraperService
    {
        List<int> ScrapeGoogleSearchResults(string searchQuery, string uri);
    }
    
    public class GoogleSearchScraperService(HttpClient httpClient) : IGoogleSearchScraperService
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public List<int> ScrapeGoogleSearchResults(string searchQuery, string uri)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query cannot be null or empty.", nameof(searchQuery));
            }

            try
            {
                //convert URI back to original string format
                var url = Uri.UnescapeDataString(uri);
                
                // Build Google Search URL with top 100 results
                var googleSearchUrl = $"https://www.google.co.uk/search?num=100&q={searchQuery}";
                
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Lynx/2.8.6rel.5 libwww-FM/2.14");
                var response = _httpClient.GetAsync(googleSearchUrl).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch search results. Status code: {response.StatusCode}");
                }
                
                var resultUrls = HtmlResultParser.ParseHtml(response.Content.ReadAsStringAsync().Result);

                // Match the urls to the search url (case-insensitive) and collect the matching indices
                var matchingIndices = new List<int>();
                for (var i = 0; i < resultUrls.Count; i++)
                {
                    if (resultUrls[i].IndexOf(url, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchingIndices.Add(i);
                    }
                }

                return matchingIndices;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while scraping: {ex.Message}", ex);
            }
        }
    }
}