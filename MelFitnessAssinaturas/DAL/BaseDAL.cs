using System.Data.SqlClient;

namespace MelFitnessAssinaturas.DAL
{
    public class BaseDal
    {
        protected SqlConnection Conn;
        protected SqlDataReader Reader = null;
    }
}
