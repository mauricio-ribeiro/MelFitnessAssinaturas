using MelFitnessAssinaturas.DAL;
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
                Console.WriteLine(@"Verificando evntos");
                VerificaEventos();
            });

            tarefa_1.ID = "verificarEventos";
            tarefa_1.Frequencia = new TimeSpan(0, 0, 30);
            tarefa_1.StartWithDelay(null, new TimeSpan(0, 0, 30));
        }

        private void VerificaEventos()
        {
            var eventoDal = new EventoDal();
            var listaEventos = eventoDal.ListaEventosNaoProcessados();

            foreach (var evento in listaEventos)
            {
                switch (evento.Sigla)
                {
                    // Nova assinatura. Ainda não existe na API, deve ser cadastrada
                    case "A_N":
                        Console.WriteLine(@"Buscando e cadastrando novas assinaturas");
                        var numAssinaturasNovas = AssinaturaCtrl.CadastraNovasAssinaturas();
                        Console.WriteLine($@"{numAssinaturasNovas} novas assinaturas cadastradas");
                        break;
                    //  Essa assinatura existe e foi editada no banco e precisa ser atualizada na API
                    case "A_E":

                        break;
                    // Um cartão foi alterado.
                    case "A_ECT":

                        break;
                    // Foi alterada a data de pagamento da assinatura  
                    case "A_EDP ":

                        break;
                    // Assinatura cancelada. Deve ser cancelada na API também.
                    case "A_C":
                        Console.WriteLine(@"Buscanso e registrando assinaturas canceladas");
                        var numAssinaturasCanceladas = AssinaturaCtrl.CancelarAssinaturas();
                        Console.WriteLine($@"{numAssinaturasCanceladas} assinaturas canceladas");
                        break;
                    //  Foi soliticado renovação do ciclo de assinatura. Reportar a API.
                    case "A_RC":

                        break;
                    // Cliente foi editado. Deve verificar se é um cliente que usa a API e atualizar seus dados lá também.
                    case "CLI_E":

                        break;
                    // Um item ( atividade ) foi INCLUÍDO na assinatura. Deve atualiar a API.
                    case "IA_I":

                        break;
                    //  Um item ( atividade ) de assinatura foi EDITADO. Deve atualiar a API.
                    case "IA_E":

                        break;
                    // Um item ( atividade ) de assinatura foi REMOVIDO. Deve atualiar a API.
                    case "IA_R":

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
