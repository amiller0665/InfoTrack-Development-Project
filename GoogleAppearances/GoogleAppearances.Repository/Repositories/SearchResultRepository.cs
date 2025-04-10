using GoogleAppearances.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace GoogleAppearances.Repository.Repositories;

public class SearchResultRepository(DbContextOptions<SearchResultRepository> options) : DbContext(options), 
    ISearchResultRepository
{
    public DbSet<SearchResults> SearchResults { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchResults>().ToTable("SearchResults");
        base.OnModelCreating(modelBuilder);
    }
    
    public List<SearchResults> GetAllSearchResults()
    {
        return SearchResults.ToList();
    }
    
    public List<SearchResults> GetSearchResultsByQuery(string searchQuery)
    {
        return SearchResults
            .Where(sr => sr.SearchQuery == searchQuery)
            .ToList();
    }

    public List<SearchResults> GetSearchResultsByUrl(string url)
    {
        return SearchResults
            .Where(sr => sr.Url == url)
            .ToList();
    }
    
    public List<SearchResults> GetSearchResultsByQueryAndUrl(string searchQuery, string url)
    {
        return SearchResults
            .Where(sr => sr.Url == url && sr.SearchQuery == searchQuery)
            .ToList();
    }
    
    public List<SearchResults> GetSearchResultsAfterDate(DateTime searchDate)
    {
        return SearchResults
            .Where(sr => sr.SearchDate.Date > searchDate.Date)
            .ToList();
    }
    
    public List<SearchResults> GetSearchResultsByQueryUrlDate(DateTime searchDate, string searchQuery, string url)
    {
        return SearchResults
            .Where(sr => sr.SearchDate.Date == searchDate.Date
            && sr.SearchQuery == searchQuery 
            && sr.Url == url)
            .ToList();
    }
    
    public void AddSearchResult(SearchResults searchResult)
    {
        SearchResults.Add(searchResult);
        SaveChanges();
    }
    
}

public interface ISearchResultRepository
{
    public List<SearchResults> GetAllSearchResults();
    public List<SearchResults> GetSearchResultsByQuery(string searchQuery);
    public List<SearchResults> GetSearchResultsByUrl(string url);
    public List<SearchResults> GetSearchResultsByQueryAndUrl(string searchQuery, string url);
    public List<SearchResults> GetSearchResultsAfterDate(DateTime searchDate);
    public List<SearchResults> GetSearchResultsByQueryUrlDate(DateTime searchDate, string searchQuery, string url);
    void AddSearchResult(SearchResults searchResult);

}