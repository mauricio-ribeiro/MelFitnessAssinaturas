using MelFitnessAssinaturas.Controllers;
using System;
using System.Windows.Forms;

namespace MelFitnessAssinaturas
{
    static class Program
    {

        private static readonly VerificaConfigIniController _verificaConfigIni = new VerificaConfigIniController();
        private static readonly  VerificaDiretorioLogErrorController _verificaDiretorioLogError = new VerificaDiretorioLogErrorController();
        private static readonly  VerificaAcessarApiController _verificaAcessarApi = new VerificaAcessarApiController();
        
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _verificaConfigIni.SetProximaVerificacao(_verificaDiretorioLogError);
            _verificaDiretorioLogError.SetProximaVerificacao(_verificaAcessarApi);
            _verificaConfigIni.ProcessaVerificacao();
            
            Application.Run(new FrmPrincipal());
        }
    }
}
