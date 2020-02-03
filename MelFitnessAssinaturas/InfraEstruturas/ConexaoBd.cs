using MelFitnessAssinaturas.Util;
using System.Data.SqlClient;

namespace MelFitnessAssinaturas.InfraEstruturas
{
    public class ConexaoBd
    {
        
        private static readonly SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder();

        public static SqlConnection GetConnection()
        {
            _connectionStringBuilder.DataSource = ConfigIniUtil.Read("SERVIDOR", "servidor");
            _connectionStringBuilder.InitialCatalog = ConfigIniUtil.Read("SERVIDOR", "banco");
            _connectionStringBuilder.IntegratedSecurity = true;

            var conn = new SqlConnection(_connectionStringBuilder.ConnectionString);
            conn.Open();
            return conn;
        }

    }
}
