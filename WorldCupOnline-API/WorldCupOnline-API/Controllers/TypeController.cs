using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Type = WorldCupOnline_API.Models.Type;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Type>>> Get()
        {
            var function = new TypeData();
            var list = await function.GetTypes();
            return list;
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Type>>> GetOne(int id)
        {
            var function = new TypeData();
            var type = new Type();
            type.id = id;
            var list = await function.GetOneType(type);
            return list;
        }

        /// <summary>
        /// Method to create types
        /// </summary>
        /// <param name="type"></param>
        /// <returns>JSON of the type created</returns>
        [HttpPost]
        public async Task Post([FromBody] Type type)
        {
            var function = new TypeData();
            await function.PostType(type);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Type type)
        {
            var function = new TypeData();
            type.id = id;
            await function.PutType(type);
            
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var function = new TypeData();
            var type = new Type();
            type.id = id;
            await function.DeleteType(type);  
        }

    }
}
