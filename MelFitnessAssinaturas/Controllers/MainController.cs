using MelFitnessAssinaturas.Util;
using System;

namespace MelFitnessAssinaturas.Controllers
{
    public class MainController
    {
        private AssinaturaController AssinaturaCtrl = new AssinaturaController();

        public void IniciaRelogioAssinatura()
        {
            var tarefa = new Scheduler(tempo =>
            {
                Console.WriteLine(@"Buscando e cadastrando novas assinaturas");
                var numAssinaturas = AssinaturaCtrl.CadastraNovasAssinaturas();
                Console.WriteLine($@"{numAssinaturas} novas assinaturas cadastradas");
            });

            tarefa.ID = "cadastraNovasAssinaturas";
            tarefa.Frequencia = new TimeSpan(0, 0, 30);
            tarefa.StartWithDelay(null, new TimeSpan(0, 0, 30));
        }
    }
}
