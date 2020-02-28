using System.IO;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Controllers
{
    public class VerificaDiretorioLogErrorController : AcessoBaseController
    {
        public override void ProcessaVerificacao()
        {
            var logerrorDir = Application.StartupPath;

            if (!Directory.Exists(logerrorDir + @"\LogError"))
                Directory.CreateDirectory(logerrorDir + @"\LogError");

            ProximaVerificacao.ProcessaVerificacao();
            
        }
    }
}
