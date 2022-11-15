using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class CountryData
    {
        private readonly DbConnection _con = new();

        public async Task<List<ValueStringBody>> GetCountries()
        {
            var list = new List<ValueStringBody>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getCountries", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var country = new ValueStringBody();
                            country.value = (string)item["value"];
                            country.label = (string)item["label"];
                            list.Add(country);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<Country> GetOneCountry(string id)
        {
            var country = new Country();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneCountry", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    country = new Country
                    {
                        id = (string)item["id"],
                        name = (string)item["name"]
                    };
                }
            }
            return country;
        }
        public async Task CreateCountry(Country country)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertCountry", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", country.id);
            cmd.Parameters.AddWithValue("@name", country.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditCountry(string id, Country country)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editCountry", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", country.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteCountry(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteCountry", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
