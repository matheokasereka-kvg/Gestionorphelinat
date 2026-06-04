using System.Configuration;
using System.Data.SqlClient;

namespace GestionDecanat.DAL
{
    public static class DbConnectionFactory
    {
        public static SqlConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GestionDecanatDb"].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}
