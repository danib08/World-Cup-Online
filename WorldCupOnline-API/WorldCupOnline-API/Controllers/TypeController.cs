using Microsoft.AspNetCore.Mvc;
using Type = WorldCupOnline_API.Models.Type;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TypeData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TypeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new TypeData();
        }

        [HttpGet]
        public async Task<ActionResult<List<ValueIntBody>>> Get()
        {
            return await _funct.GetTypes(); ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Type>>> GetOne(int id)
        {
            return await _funct.GetOneType(id); ;
        }

       [HttpPost]
        public async Task Post([FromBody] Type type)
        {
            await _funct.CreateType(type);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Type type)
        {
            await _funct.EditType(id, type);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteType(id);  
        }
    }
}
