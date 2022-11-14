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

/**
/// <summary>
/// Method to get all created countries
/// </summary>
/// <returns>JSONResult with all countries</returns>
[HttpGet]
public JsonResult GetCountries()
{
    string query = @"exec proc_country '','','Select WebApp'"; ///sql query

    DataTable table = new DataTable(); //Create datatable
    string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
    SqlDataReader myReader;
    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
    {
        myCon.Open(); ///Open connection
        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        {
            myReader = myCommand.ExecuteReader();
            table.Load(myReader); ///Data is loaded into table
            myReader.Close();
            myCon.Close(); ///Closed connection
        }
    }

    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
    foreach (DataColumn column in table.Columns)
    {
        column.ColumnName = ti.ToLower(column.ColumnName); ///Make all lowercase to avoid conflicts with communication
    }

    return new JsonResult(table); ///Return JSON Of the data table
}



/// <summary>
/// Method to get one country by its id
/// </summary>
/// <param name="name"></param>
/// <returns></returns>
[HttpGet("{id}")]
public string GetType(string id)
{
    ///Created label
    string lbl_name;
    string lbl_id;


    ///SQL Query
    string query = @"
                    exec proc_country @id,'','Select One'";

    DataTable table = new DataTable();///Created table to store data
    string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
    SqlDataReader myReader;
    using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
    {
        myCon.Open();///Open connection
        using (SqlCommand myCommand = new SqlCommand(query, myCon))///Command with query and connection
        {
            ///Added parameters
            myCommand.Parameters.AddWithValue("@id", id);
            myReader = myCommand.ExecuteReader();
            table.Load(myReader); ///Load data to table
            myReader.Close();
            myCon.Close(); ///Close connection
        }
    }

    ///Verify if table is empty
    if (table.Rows.Count > 0)
    {

        DataRow row = table.Rows[0];

        ///Manipulation of every row of datatable and parse them to string
        lbl_id = row["id"].ToString();
        lbl_name = row["name"].ToString();

        ///Creation of the JSON
        var data = new JObject(new JProperty("id", lbl_id), new JProperty("name", lbl_name));

        return data.ToString(); ///Return created JSON
    }
    else
    {
        var data = new JObject(new JProperty("Existe", "no"));
        return data.ToString(); ///Return message if table is empty
    }

}

/// <summary>
/// Method to create countries
/// </summary>
/// <param name=""></param>
/// <returns>JSON of the type created</returns>
[HttpPost]
public JsonResult PostCountry(Country country)
{
    //SQL Query
    string query = @"
                     exec proc_country @id,@name,'Insert'
                    ";
    DataTable table = new DataTable();
    string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
    SqlDataReader myReader;
    using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
    {
        myCon.Open(); ///Opened connection
        SqlCommand myCommand = new SqlCommand(query, myCon);

        ///Parameters added with values
        myCommand.Parameters.AddWithValue("@id", country.id);
        myCommand.Parameters.AddWithValue("@name", country.name);
        myReader = myCommand.ExecuteReader();
        table.Load(myReader);
        myReader.Close();
        myCon.Close();///Closed connection

    }

    return new JsonResult(table); ///Returns table with info

}

/// <summary>
/// Method to delete a country by its id
/// </summary>
/// <param id="id"></param>
/// <returns></returns>
[HttpDelete("{id}")]
public ActionResult DeleteType(string id)
{
    ///SQL Query
    string query = @"
                    exec proc_country @id,'','Delete'
    ";
    DataTable table = new DataTable();
    string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
    SqlDataReader myReader;
    using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
    {
        myCon.Open();///Open connection
        using (SqlCommand myCommand = new SqlCommand(query, myCon)) ///Command with query and connection
        {
            myCommand.Parameters.AddWithValue("@id", id);
            myReader = myCommand.ExecuteReader();
            table.Load(myReader);
            myReader.Close();
            myCon.Close();///Closed connection
        }
    }
    return Ok(); ///Returns acceptance
}

**/

