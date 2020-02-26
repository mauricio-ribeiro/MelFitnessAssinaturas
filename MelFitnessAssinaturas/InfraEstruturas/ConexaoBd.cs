using MelFitnessAssinaturas.Util;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.InfraEstruturas
{
    public class ConexaoBd
    {
        
        private static readonly SqlConnectionStringBuilder _connectionStringBuilder = new SqlConnectionStringBuilder();

        public static SqlConnection GetConnection()
        {
            var servidor = ConfigIniUtil.Read("SERVIDOR", "servidor");
            var instancia = ConfigIniUtil.Read("SERVIDOR", "instancia");
            var porta = ConfigIniUtil.Read("SERVIDOR", "porta");

            var dataSource = new StringBuilder();
            dataSource.Append(servidor);

            if (string.IsNullOrEmpty(instancia))
            {
                dataSource.Append(",");
                dataSource.Append(porta);
            }
            else
            {
                dataSource.Append(@"\");
                dataSource.Append(instancia);
            }

            _connectionStringBuilder.DataSource = dataSource.ToString();
            _connectionStringBuilder.InitialCatalog = ConfigIniUtil.Read("SERVIDOR", "banco");
            _connectionStringBuilder.IntegratedSecurity = true;

            var conn = new SqlConnection(_connectionStringBuilder.ConnectionString);
            conn.Open();
            return conn;
        }

    }
}
