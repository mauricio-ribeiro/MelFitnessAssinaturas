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
            var banco = ConfigIniUtil.Read("SERVIDOR", "banco");
            var instancia = ConfigIniUtil.Read("SERVIDOR", "instancia");
            var porta = ConfigIniUtil.Read("SERVIDOR", "porta");
            var usuario = ConfigIniUtil.Read("SERVIDOR", "usuario");
            var senha = ConfigIniUtil.Read("SERVIDOR", "senha");

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
            _connectionStringBuilder.InitialCatalog = banco;
            _connectionStringBuilder.IntegratedSecurity = true;

            var conn = new SqlConnection(_connectionStringBuilder.ConnectionString);
            conn.Open();
            return conn;
        }

    }
}
