using System.Data;
using System.Data.SqlClient;

namespace MelFitnessAssinaturas.Controllers
{
    public class TesteConnectionStringController
    {


        public bool TestConnectionString(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    return (conn.State == ConnectionState.Open);
                }
                catch
                {
                    return false;
                }
            }
        }


    }
}
