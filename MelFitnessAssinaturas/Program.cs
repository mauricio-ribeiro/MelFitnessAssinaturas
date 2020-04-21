using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.DAL;
using System;
using System.IO;
using System.Windows.Forms;

namespace MelFitnessAssinaturas
{
    static class Program
    {

        private static readonly VerificaConfigIniController _verificaConfigIni = new VerificaConfigIniController();
        private static readonly VerificaDiretorioLogErrorController _verificaDiretorioLogError = new VerificaDiretorioLogErrorController();
        private static readonly VerificaAcessarApiController _verificaAcessarApi = new VerificaAcessarApiController();

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
             const string fileTrigger = "_sync.gat";

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                _verificaConfigIni.SetProximaVerificacao(_verificaDiretorioLogError);
                _verificaDiretorioLogError.SetProximaVerificacao(_verificaAcessarApi);
                _verificaConfigIni.ProcessaVerificacao();

                if (File.Exists(Application.StartupPath + "\\" + fileTrigger))
                    File.Delete(Application.StartupPath + "\\" + fileTrigger);

                new EventoDal().ConfigSql();
                var mainCtrl = new MainController();
                mainCtrl.StarFileWatch();
                //mainCtrl.IniciaRelogioAssinatura();

                Application.Run(new FrmPrincipal());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data["MensagemCustomizada"] + Environment.NewLine + Environment.NewLine +
                                @"Mensagem original: " + ex.Message, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
