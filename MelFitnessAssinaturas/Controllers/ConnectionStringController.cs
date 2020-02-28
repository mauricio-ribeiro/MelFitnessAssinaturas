using MelFitnessAssinaturas.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.Controllers
{
    public class ConnectionStringController
    {
        
        public ConnectionStringController()
        {
            
        }


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


        public string MontaConnectionString(Config config)
        {

            var connectionString = string.Empty;

            // QUANDO FOR INTANCIA
            // Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
            if (config.Porta == 0)
            {
                connectionString = $@"Server={config.Servidor}" + ";" +
                                   "Database=" + config.Banco + ";" +
                                   "User ID=" + config.Usuario + ";" +
                                   $@"Password={config.Senha}";

                var server = new StringBuilder();
                server.Append(config.Servidor);

                if (string.IsNullOrEmpty(config.Instancia))
                {
                    server.Append(",");
                    server.Append(config.Porta);
                }
                else
                {
                    server.Append(@"\");
                    server.Append(config.Instancia);
                }

                connectionString = $@"Server={server}" + ";" +
                                   "Database=" + config.Banco + ";" +
                                   "User ID=" + config.Usuario + ";" +
                                   $@"Password={config.Senha}" +
                                   ";";
            }
            else
            {
                // SERVIDOR noRMAL
                // Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;     

                connectionString = $@"Server={config.Servidor},{config.Porta.ToString()}" + ";" +
                                   "Database=" + config.Banco + ";" +
                                   "User ID=" + config.Usuario + ";" +
                                   $@"Password={config.Senha}" +
                                   ";";
            }


            return connectionString;

        }


    }
}
