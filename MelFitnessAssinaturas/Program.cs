using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.DAL;
using System;
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
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                _verificaConfigIni.SetProximaVerificacao(_verificaDiretorioLogError);
                _verificaDiretorioLogError.SetProximaVerificacao(_verificaAcessarApi);
                _verificaConfigIni.ProcessaVerificacao();

                new EventoDal().ConfigSql();
                MainController mainCtrl = new MainController();
                mainCtrl.StarFileWatch();
                //mainCtrl.IniciaRelogioAssinatura();

                Application.Run(new FrmPrincipal());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
