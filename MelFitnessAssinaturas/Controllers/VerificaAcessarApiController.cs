using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;

namespace MelFitnessAssinaturas.Controllers
{
    public class VerificaAcessarApiController : AcessoBaseController
    {
        public override void ProcessaVerificacao()
        {
            var connectionString = string.Empty;
            var connectionStringController = new ConnectionStringController();

            var config = new Config
            {
                Servidor = ConfigIniUtil.Read("SERVIDOR", "servidor"),
                Banco = ConfigIniUtil.Read("SERVIDOR", "banco"),
                Instancia = ConfigIniUtil.Read("SERVIDOR", "instancia"),
                Porta = Convert.ToInt32(ConfigIniUtil.Read("SERVIDOR", "porta")),
                Usuario = ConfigIniUtil.Read("SERVIDOR", "usuario"),
                Senha = ConfigIniUtil.Read("SERVIDOR", "senha"),
                TokenApi = ConfigIniUtil.Read("MUNDIPAGG", "basicAuthUserName")
            };

            connectionString = connectionStringController.MontaConnectionString(config);

            if (!connectionStringController.TestConnectionString(connectionString))
            {
                new FrmConfigConexaoBanco().ShowDialog();
                Environment.Exit(0);
            }

        }
    }
}
