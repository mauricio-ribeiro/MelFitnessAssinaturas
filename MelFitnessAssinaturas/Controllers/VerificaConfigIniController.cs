using System;
using MelFitnessAssinaturas.Singletons;
using MelFitnessAssinaturas.Util;
using System.IO;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Controllers
{
    public class VerificaConfigIniController : AcessoBaseController
    {
        private readonly string filePath = Application.StartupPath + "\\Config.ini";

        public override void ProcessaVerificacao()
        {
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
                
                // Sessão SERVIDOR
                ConfigIniUtil.Write("SERVIDOR", "servidor", "");
                ConfigIniUtil.Write("SERVIDOR", "banco", "");
                ConfigIniUtil.Write("SERVIDOR", "instancia", "");
                ConfigIniUtil.Write("SERVIDOR", "porta", "");
                ConfigIniUtil.Write("SERVIDOR", "usuario", "");
                ConfigIniUtil.Write("SERVIDOR", "senha", "");

                // Sessão MUNDIPAGG
                ConfigIniUtil.Write("MUNDIPAGG", "basicAuthUserName", "");

                new FrmConfigConexaoBanco().ShowDialog();
                Environment.Exit(0);
            }

            ProximaVerificacao.ProcessaVerificacao();
        }


    }
}
