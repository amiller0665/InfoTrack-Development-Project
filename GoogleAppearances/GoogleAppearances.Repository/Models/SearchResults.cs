namespace GoogleAppearances.Repository.Models;

public class SearchResults
{
    public int Id { get; set; }
    public string SearchQuery { get; set; }
    public string Url { get; set; }
    public string Positions { get; set; }
    public DateTime SearchDate { get; set; } = DateTime.UtcNow;
}
