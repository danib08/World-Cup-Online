using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Type = WorldCupOnline_API.Models.Type;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Country>>> Get()
        {
            var function = new CountryData();

            var list = await function.GetCountry();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Country>>> GetOne(string id)
        {
            var function = new CountryData();
            var country = new Country();
            country.id = id;
            var list = await function.GetOneCountry(country);
            return list;
        }

        [HttpPost]
        public async Task Post([FromBody] Country country)
        {
            var function = new CountryData();
            await function.PostCountry(country);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Country country)
        {
            var function = new CountryData();
            country.id = id;
            await function.PutCountry(country);

        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var function = new CountryData();
            var country = new Country();
            country.id = id;
            await function.DeleteCountry(country);
        }
    }
}

