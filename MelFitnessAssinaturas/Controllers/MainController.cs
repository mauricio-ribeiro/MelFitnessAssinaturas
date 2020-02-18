using MelFitnessAssinaturas.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.Controllers
{
    public class MainController
    {
        private AssinaturaController AssinaturaCtrl = new AssinaturaController();

        public void IniciaRelogioAssinatura()
        {
            Scheduler tarefa = new Scheduler(tempo =>
            {
                Console.WriteLine("Buscando e cadastrando novas assinaturas");
                var numAssinaturas = AssinaturaCtrl.CadastraNovasAssinaturas();
                Console.WriteLine(String.Format("{0} novas assinaturas cadastradas", numAssinaturas));
            });

            tarefa.ID = "cadastraNoasAssinaturas";
            tarefa.Frequencia = new TimeSpan(0, 0, 30);
            tarefa.StartWithDelay(null, new TimeSpan(0, 0, 30));
        }
    }
}
