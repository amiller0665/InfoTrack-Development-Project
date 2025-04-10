using GoogleAppearances.Repository.Models;
using GoogleAppearances.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAppearances.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchResultsController(ISearchResultRepository searchResultRepository, 
    ILogger<GoogleAppearancesController> logger) : ControllerBase
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
            logger.LogError(ex, "An error occurred while retrieving all search results.");
            return StatusCode(500, "An error occurred while retrieving all search results.");
        }
    }

    // Get search results by query
    [HttpGet]
    [Route("Query/{searchQuery}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByQuery(string searchQuery)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsByQuery(searchQuery);
            return Ok(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving search results.");
            return StatusCode(500, "An error occurred while retrieving search results by query.");
        }
    }

    [HttpGet]
    [Route("Url/{url}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByUrl(string url)
    {
        try
        {
            var decodedUrl = Uri.UnescapeDataString(url);
            var results = searchResultRepository.GetSearchResultsByUrl(decodedUrl);
            return Ok(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving all search results.");
            return StatusCode(500, "An error occurred while retrieving search results by URL.");
        }
    }

    [HttpGet]
    [Route("QueryUrl/{searchQuery}/{url}")]
    public ActionResult<List<SearchResults>> GetSearchResultsByQueryAndUrl(
        string searchQuery,
        string url)
    {
        try
        {
            var decodedUrl = Uri.UnescapeDataString(url);
            var results = searchResultRepository.GetSearchResultsByQueryAndUrl(searchQuery, decodedUrl);
            return Ok(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving search results.");
            return StatusCode(500, "An error occurred while retrieving search results by query and URL.");
        }
    }

    [HttpGet]
    [Route("AfterDate")]
    public ActionResult<List<SearchResults>> GetSearchResultsAfterDate([FromQuery] DateTime searchDate)
    {
        try
        {
            var results = searchResultRepository.GetSearchResultsAfterDate(searchDate);
            return Ok(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving search results.");
            return StatusCode(500, "An error occurred while retrieving search results after the specified date.");
        }
    }

    [HttpGet]
    [Route("ByQueryUrlDate")]
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
            logger.LogError(ex, "An error occurred while retrieving search results.");
            return StatusCode(500, "An error occurred while retrieving search results by query, URL, and date.");
        }
    }

}