using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SCB_API.Models;
using SCB_API.Models.RequestModels;
using SCB_API.Models.ResponseModels;
using SCB_API.Services;
using SCB_API.Tools.Helpers;

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
            var check = new Checkers();

            string region = bornRequestDto.Region;
            string? year = bornRequestDto.Year;
            string? gender = bornRequestDto.Gender;

            var response = new SCBResponse<List<BornStatistic>>();

            if(!check.IsNullOrUndefined(gender) && !check.IsNullOrUndefined(year))
            {
                response = _scbHandler.GetBornStatistic(region, year, gender);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            else if (check.IsNullOrUndefined(gender) && !check.IsNullOrUndefined(year))
            {
                response = _scbHandler.GetBornStatistic(region, year);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            else if(!check.IsNullOrUndefined(gender) && check.IsNullOrUndefined(year))
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

        [HttpGet]
        [Route("/regions")]
        public IActionResult GetRegions()
        {
            var response = new SCBResponse<List<string>>();

            response = _scbHandler.GetRegions();
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }
}
