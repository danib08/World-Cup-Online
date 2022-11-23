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

        /// <summary>
        /// Service to get all Countries
        /// </summary>
        /// <returns>List of ValueStringBody</returns>
        [HttpGet]
        public async Task<ActionResult<List<ValueStringBody>>> Get()
        {
            return await _funct.GetCountries();
        }

        /// <summary>
        /// Service to get one Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Country</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetOne(string id)
        {
            return await _funct.GetOneCountry(id);
        }

        /// <summary>
        /// Service to insert Countries
        /// </summary>
        /// <param name="country"></param>
        /// <returns>Task action result</returns>
        [HttpPost]
        public async Task Post([FromBody] Country country)
        {
            await _funct.CreateCountry(country);
        }

        /// <summary>
        /// Service to edit Countries
        /// </summary>
        /// <param name="id"></param>
        /// <param name="country"></param>
        /// <returns>Task action result</returns>
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Country country)
        {
            await _funct.EditCountry(id, country);
        }

        /// <summary>
        /// Service to delete Country
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task action result</returns>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteCountry(id);
        }
    }
}

