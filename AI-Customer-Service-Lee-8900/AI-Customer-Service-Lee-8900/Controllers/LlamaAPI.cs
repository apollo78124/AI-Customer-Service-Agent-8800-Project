using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AI_Customer_Service_Lee_8900.Controllers
{
    [Route("api")]
    [ApiController]
    public class LlamaAPI : ControllerBase
    {
        // GET: api/<LlamaAPI>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LlamaAPI>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LlamaAPI>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LlamaAPI>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LlamaAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
