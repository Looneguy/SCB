using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SCB_API.Controllers
{
    [Route("api/SCB")]
    [ApiController]
    public class SCBController : ControllerBase
    {
        // GET: api/<SCBController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SCBController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SCBController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SCBController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SCBController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
