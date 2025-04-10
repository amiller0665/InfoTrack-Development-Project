using GoogleAppearances.Repository.Models;
using GoogleAppearances.Repository.Repositories;
using Microsoft.Extensions.Logging;

namespace GoogleAppearances.Services
{
    public interface IGoogleSearchScraperService
    {
        List<string> ScrapeGoogleSearchResults(string searchQuery, string uri);
    }
    
    public class GoogleSearchScraperService(HttpClient httpClient, ISearchResultRepository searchResultRepository,
        ILogger<GoogleSearchScraperService> logger) : IGoogleSearchScraperService
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public List<string> ScrapeGoogleSearchResults(string searchQuery, string uri)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query cannot be null or empty.", nameof(searchQuery));
            }

            try
            {
                //convert URI back to original string format
                var url = Uri.UnescapeDataString(uri);

                var existingResults = searchResultRepository.GetSearchResultsByQueryUrlDate(DateTime.Today, searchQuery, url);

                if (existingResults.Any())
                {
                    return existingResults.First().Positions.Split(',').ToList();
                }
                
                // Build Google Search URL with top 100 results
                var googleSearchUrl = $"https://www.google.co.uk/search?num=100&q={searchQuery}";
                
                //Google uses javascript to display the results,
                //so we need to add a user-agent to mimic that behaviour and get the results
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Lynx/2.8.6rel.5 libwww-FM/2.14");
                var response = _httpClient.GetAsync(googleSearchUrl).Result;

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"Failed to fetch search results. Status code: {response.StatusCode}");
                    throw new HttpRequestException($"Failed to fetch search results. Status code: {response.StatusCode}");
                }
                
                var resultUrls = HtmlResultParser.ParseHtml(response.Content.ReadAsStringAsync().Result);

                // Match the urls to the search url (case-insensitive) and collect the matching indices
                var matchingIndices = new List<string>();
                for (var i = 0; i < resultUrls.Count; i++)
                {
                    if (resultUrls[i].IndexOf(url, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        matchingIndices.Add((i+1).ToString()); // We want results to start from 1
                    }
                }
                
                // Save the results to the database
                searchResultRepository.AddSearchResult(new SearchResults()
                {
                    SearchQuery = searchQuery,
                    Url = url,
                    SearchDate = DateTime.Today,
                    Positions = string.Join(",", matchingIndices)
                });

                return matchingIndices;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while scraping.");
                throw;
            }
        }
    }
}