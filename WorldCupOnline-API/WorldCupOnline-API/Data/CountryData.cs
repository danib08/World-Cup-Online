using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class CountryData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all countries
        /// </summary>
        /// <returns>List of ValueStringBody objects</returns>
        public async Task<List<ValueStringBody>> GetCountries()
        {
            var list = new List<ValueStringBody>();///Create list of ValueStingBody object
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getCountries", sql)) ///Calls stored procedure via sql connection
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedure
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            ///Read from Database
                            var country = new ValueStringBody(); ///Create ValueStingBody object
                            country.value = (string)item["value"];
                            country.label = (string)item["label"];
                            list.Add(country);/// Add object to list
                        }
                    }
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain one country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Country> GetOneCountry(string id)
        {
            var country = new Country(); ///Create object Country
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneCountry", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedre
                cmd.Parameters.AddWithValue("@id", id); ///Add parameter with value id

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from Database
                    country = new Country
                    {
                        id = (string)item["id"],
                        name = (string)item["name"]
                    };
                }
            }
            return country; ///Return object
        }

        /// <summary>
        /// Method to create a country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task CreateCountry(Country country)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertCountry", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedre
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", country.id);
            cmd.Parameters.AddWithValue("@name", country.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task EditCountry(string id, Country country)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editCountry", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedre
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", country.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete a country by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCountry(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteCountry", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
