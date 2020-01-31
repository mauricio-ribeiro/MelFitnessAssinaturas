using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelFitnessAssinaturas.Util;

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
