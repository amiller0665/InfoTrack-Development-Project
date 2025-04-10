using GoogleAppearances.Repository.Models;
using GoogleAppearances.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAppearances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchResultsController(ISearchResultRepository searchResultRepository) : ControllerBase
{
    ActionResult<List<SearchResults>> GetAllSearchResults()
    {
        try
        {
            var results = searchResultRepository.GetAllSearchResults();
            return Ok(results);
        }
        catch (Exception ex)
        {
            // Log the exception details (optional: use a logging service)
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving all search results.");
        }
    }

    // Get search results by query
    [HttpGet("query/{searchQuery}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByQuery(string searchQuery)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsByQuery(searchQuery);
            return Ok(results);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving search results by query.");
        }
    }

    [HttpGet("url/{searchQuery}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByUrl(string url)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsByUrl(url);
            return Ok(results);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving search results by URL.");
        }
    }

    [HttpGet("query-url/{searchQuery}/{url}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByQueryAndUrl(
        string searchQuery,
        string url)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsByQueryAndUrl(searchQuery, url);
            return Ok(results);
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving search results by query and URL.");
        }
    }

    [HttpGet("after-date")]
    public ActionResult<List<SearchResults>> GetSearchResultsAfterDate([FromQuery] DateTime searchDate)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsAfterDate(searchDate);
            return Ok(results);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving search results after the specified date.");
        }
    }

    [HttpGet("by-query-url-date")]
    public ActionResult<List<SearchResults>> GetSearchResultsByQueryUrlDate(
        [FromQuery] DateTime searchDate,
        [FromQuery] string searchQuery,
        [FromQuery] string url)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsByQueryUrlDate(searchDate, searchQuery, url);
            return Ok(results);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving search results by query, URL, and date.");
        }
    }

}