using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Type = WorldCupOnline_API.Models.Type;

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

        /// <summary>
        /// Method to get all created states
        /// </summary>
        /// <returns>JSONResult with all states</returns>
        [HttpGet]
        public JsonResult GetTypes()
        {
            string query = @"exec proc_type 0,'','Select'"; ///sql query

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
        /// Method to get one state by its id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string GetType(int id)
        {
            ///Created label
            string lbl_name;
            string lbl_id;


            ///SQL Query
            string query = @"
                            exec proc_type @id,'','Select One'";

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
        /// Method to create states
        /// </summary>
        /// <param name="state"></param>
        /// <returns>JSON of the state created</returns>
        [HttpPost]
        public JsonResult PostType(Type type)
        {
            //SQL Query
            string query = @"
                             exec proc_type @id,@name,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@id", type.id);
                myCommand.Parameters.AddWithValue("@name", type.name);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }

            return new JsonResult(table); ///Returns table with info

        }

        /// <summary>
        /// Method to delete a state by its id
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteType(int id)
        {
            ///SQL Query
            string query = @"
                            exec proc_type @id,'','Delete'
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
    }
}
