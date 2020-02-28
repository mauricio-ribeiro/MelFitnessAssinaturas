using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Singletons;
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
            SistemaSingleton.Instancia.TokenApi = config.TokenApi;

            if (!connectionStringController.TestConnectionString(connectionString) || String.IsNullOrEmpty(config.TokenApi))
            {
                new FrmConfigConexaoBanco().ShowDialog();
                Environment.Exit(0);
            }

        }
    }
}
