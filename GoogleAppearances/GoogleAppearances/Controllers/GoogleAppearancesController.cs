using Microsoft.AspNetCore.Mvc;
using GoogleAppearances.Services;

namespace GoogleAppearances.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleAppearancesController(IGoogleSearchScraperService scrapingService) : ControllerBase
    {
        // GET: api/GoogleAppearances
        [HttpGet]
        [Route("GetCurrentAppearances/{query}/{url}")]
        public ActionResult<List<int>> GoogleAppearances(string query, string url)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(url))
                return BadRequest("Request parameters cannot be null or empty.");

            try
            {
                var numbers = scrapingService.ScrapeGoogleSearchResults(query, url);
                return Ok(numbers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}