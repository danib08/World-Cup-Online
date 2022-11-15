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

        /// <summary>
        /// Service to get all Type
        /// </summary>
        /// <returns>List of ValueIntBody</returns>
        [HttpGet]
        public async Task<ActionResult<List<ValueIntBody>>> Get()
        {
            return await _funct.GetTypes();
        }

        /// <summary>
        /// Service to get one Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Type>> GetOne(int id)
        {
            return await _funct.GetOneType(id); ;
        }

        /// <summary>
        /// Service to insert Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Type type)
        {
            await _funct.CreateType(type);
        }

        /// <summary>
        /// Service to edit Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Type type)
        {
            await _funct.EditType(id, type);
        }

        /// <summary>
        /// Service to delete Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteType(id);  
        }
    }
}
