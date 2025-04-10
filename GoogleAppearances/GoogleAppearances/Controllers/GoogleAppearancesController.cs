using Microsoft.AspNetCore.Mvc;
using GoogleAppearances.Services;

namespace GoogleAppearances.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleAppearancesController(IGoogleSearchScraperService scrapingService, 
        ILogger<GoogleAppearancesController> logger) : ControllerBase
    {
        // GET: api/GoogleAppearances
        [HttpGet]
        [Route("GetCurrentAppearances/{query}/{url}")]
        public ActionResult<List<string>> GoogleAppearances(string query, string url)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(url))
            {
                logger.LogInformation($"Request parameters cannot be null or empty. query: '{query}', url: '{url}'.");
                return BadRequest("Request parameters cannot be null or empty.");
            }
               

            try
            {
                var numbers = scrapingService.ScrapeGoogleSearchResults(query, url);
                return Ok(numbers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, 
                    $"An error occurred while processing the GetCurrentAppearances request with parameters: query '{query}', url '{url}'.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}