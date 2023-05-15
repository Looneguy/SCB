using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SCB_API.Models;
using SCB_API.Models.RequestModels;
using SCB_API.Models.ResponseModels;
using SCB_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SCB_API.Controllers
{
    [Route("api/SCB")]
    [ApiController]
    public class SCBController : ControllerBase
    {
        private readonly SCBHandler _scbHandler;

        public SCBController(SCBHandler scbHandler) 
        {
            _scbHandler = scbHandler;
        }
        
        [HttpGet]
        [Route("/born-statistics")]
        public IActionResult GetStatistics([FromQuery] BornRequestDTO bornRequestDto)
        {
            // TODO Split into years/gender since its a to big of a fetch
            // TODO put stats in proper response model

            string region = bornRequestDto.Region;
            string year = bornRequestDto.Year;
            string gender = bornRequestDto.Gender;

            var response = new SCBResponse<List<BornStatistic>>();

            if(gender != null && year != null)
            {
                response = _scbHandler.GetBornStatistic(region, year, gender);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            else if (gender == null && year != null)
            {
                response = _scbHandler.GetBornStatistic(region, year);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            else if(gender != null && year == null)
            {
                response.ErrorMessage = "No value for 'Year' was provided.";
                return BadRequest(response);
            }
            else
            {
                response = _scbHandler.GetBornStatistic(region);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
        }
    }
}
