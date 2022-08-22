using System.Data.SqlClient;

namespace YammerReader.Server.DAL
{
    public class BaseDAL
    {
        protected SqlConnection GetSqlConnection()
        {
            IConfiguration config = GetConfig();
            string connectionString = config.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }

        protected IConfiguration GetConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();
            return config;
        }
    }
}
