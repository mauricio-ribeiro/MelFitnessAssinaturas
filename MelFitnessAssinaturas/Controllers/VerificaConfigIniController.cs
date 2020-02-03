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
                using (File.Create(filePath));
                ConfigIniUtil.Write("SERVIDOR","servidor","");
                ConfigIniUtil.Write("SERVIDOR", "banco", "");
            }
            
            ProximaVerificacao.ProcessaVerificacao();
        }
        

    }
}
