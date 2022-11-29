using Newtonsoft.Json.Linq;

namespace WorldCupOnline_API.Conection
{
    public class DbConection
    {
        private string connectionString = String.Empty;

        public DbConection()
        {
            var constructor = new ConfigurationBuilder().SetBasePath
                (Directory.GetCurrentDirectory()).AddJsonFile
                ("appsettings.json").Build();
            connectionString = constructor.GetSection("ConnectionStrings:WorldCupOnline").Value;
        }

        public string SQLCon()
        {
            return connectionString;
        }
    }
}
