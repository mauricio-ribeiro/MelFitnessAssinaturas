using MelFitnessAssinaturas.Util;
using System;

namespace MelFitnessAssinaturas.Controllers
{
    public class MainController
    {
        private AssinaturaController AssinaturaCtrl = new AssinaturaController();

        public void IniciaRelogioAssinatura()
        {
            var tarefa_1 = new Scheduler(tempo =>
            {
                Console.WriteLine(@"Buscando e cadastrando novas assinaturas");
                var numAssinaturasNovas = AssinaturaCtrl.CadastraNovasAssinaturas();
                Console.WriteLine($@"{numAssinaturasNovas} novas assinaturas cadastradas");

               
            });

            tarefa_1.ID = "cadastraNovasAssinaturas";
            tarefa_1.Frequencia = new TimeSpan(0, 0, 30);
            tarefa_1.StartWithDelay(null, new TimeSpan(0, 0, 30));

            var tarefa_2 = new Scheduler(tempo =>
            {
                Console.WriteLine(@"Buscanso e registrando assinaturas canceladas");
                var numAssinaturasCanceladas = AssinaturaCtrl.CancelarAssinaturas();
                Console.WriteLine($@"{numAssinaturasCanceladas} assinaturas canceladas");
            });
            tarefa_2.ID = "cancelaAssinaturas";
            tarefa_2.Frequencia = new TimeSpan(0, 0, 30);
            tarefa_2.StartWithDelay(null, new TimeSpan(0, 0, 30));
        }
    }
}
