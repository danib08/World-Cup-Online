using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CountryData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new CountryData(); 
        }

        [HttpGet]
        public async Task<ActionResult<List<ValueStringBody>>> Get()
        {
            return await _funct.GetCountries();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetOne(string id)
        {
            return await _funct.GetOneCountry(id);
        }

        [HttpPost]
        public async Task Post([FromBody] Country country)
        {
            await _funct.CreateCountry(country);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Country country)
        {
            await _funct.EditCountry(id, country);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteCountry(id);
        }
    }
}

