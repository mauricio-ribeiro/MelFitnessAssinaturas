using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MelFitnessAssinaturas.Util;

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
