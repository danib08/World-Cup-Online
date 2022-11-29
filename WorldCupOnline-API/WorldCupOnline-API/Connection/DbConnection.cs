namespace WorldCupOnline_API.Connection
{
    public class DbConnection
    {
        private string connectionString = string.Empty;

        public DbConnection()
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
