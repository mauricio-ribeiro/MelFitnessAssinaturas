using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Util;
using System;
using System.IO;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Controllers
{
    public class MainController
    {

        private AssinaturaController AssinaturaCtrl = new AssinaturaController();
        private ClienteController ClienteCtrl = new ClienteController();
        private FaturaController FaturaCtrl = new FaturaController();
        private EventoDal EventoDAL = new EventoDal();
        private Scheduler tarefa_1;
        private readonly string fileTrigger = "_sync.gat";

        public void IniciaRelogioAssinatura()
        {
            tarefa_1 = new Scheduler(tempo =>
            {
                Console.WriteLine(@"Verificando eventos");
                VerificaEventos();
            });

            tarefa_1.ID = "verificarEventos";
            tarefa_1.Frequencia = new TimeSpan(0, 0, 10);
            tarefa_1.StartWithDelay(null, new TimeSpan(0, 0, 5));
            tarefa_1.MaxExec = 1;// em produção deve REMOVER essa linha
        }

        public void StarFileWatch()
        {
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Path = Application.StartupPath;
            fsw.Filter = fileTrigger;
            fsw.Created += FileSystemWatcher_Created;
            fsw.EnableRaisingEvents = true;

        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            VerificaEventos();
            File.Delete(Application.StartupPath + "\\" + fileTrigger);
        }


        private void VerificaEventos()
        {
            var listaEventos = EventoDAL.ListaEventosNaoProcessados();

            foreach (var evento in listaEventos)
            {
                try
                {
                    switch (evento.Sigla)
                    {
                        // Nova assinatura. Ainda não existe na API, deve ser cadastrada
                        case "A_N":
                            Console.WriteLine(@"Buscando e cadastrando nova assinatura");
                            AssinaturaCtrl.CadastraNovaAssinaturaApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        //  Essa assinatura existe, foi editada no banco e precisa ser atualizada na API
                        case "A_E":

                            break;
                        // Um cartão foi alterado para ser usado em uma assinatura.
                        case "A_ECT":
                            Console.WriteLine(@"Buscando assinatura e o novo cartão para alterar");
                            AssinaturaCtrl.AlteraCartaoNaAssinatura(evento.IdTabela, evento.CodAux1);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        // Foi alterada a data de pagamento da assinatura  
                        case "A_EDP ":
                            Console.WriteLine(@"Alterando a data de faturamento da assinatura");  // Não foi testado por completo.
                            AssinaturaCtrl.AlterarDataFaturameto(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        // Assinatura cancelada. Deve ser cancelada na API também.
                        case "A_C":
                            Console.WriteLine(@"Buscando e registrando assinatura cancelada");
                            AssinaturaCtrl.CancelarAssinaturaApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        //case "A_EDTI":
                        //    //1 - Não é possível atualizar a data de início da assinatura para o dia anterior ao dia atual.
                        //    //2 - Não é possível atualizar a data de início de uma assinatura que já começou.
                        //    Console.WriteLine(@"Alterando a data de início da assinatura");
                        //    AssinaturaCtrl.AlterarDataInicio(evento.IdTabela, Convert.ToInt32(evento.CodAux1));
                        //    EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                        //    break;
                        //  Foi soliticado renovação do ciclo de assinatura. Reportar a API.
                        case "A_RC":

                            break;
                        // Cliente foi editado. Deve verificar se é um cliente que usa a API e atualizar seus dados lá também.
                        case "CLI_E":
                            Console.WriteLine(@"Atualizando cadastro do cliente {0}", evento.IdTabela);
                            ClienteCtrl.AtualizaClienteApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        // Um item ( atividade ) vai ser INCLUÍDO na assinatura. Deve atualiar a API.
                        case "IA_I":
                            Console.WriteLine(@"Incluindo um item na assinatura do cliente");
                            AssinaturaCtrl.IncluirItemApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        //  Um item ( atividade ) de assinatura foi EDITADO. Deve atualiar a API.
                        case "IA_E":
                            Console.WriteLine(@"Alterando um item na assinatura do cliente");
                            AssinaturaCtrl.EditarItemApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        // Um item ( atividade ) de assinatura foi REMOVIDO. Deve atualiar a API.
                        case "IA_R":
                            Console.WriteLine(@"Removendo um item na assinatura do cliente");
                            AssinaturaCtrl.RemoverItemApi(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        // Usuário que atualizar as informações de fatura de uma assinatura
                        case "F_AA":
                            Console.WriteLine(@"Atualizando faturas de uma assinatura");
                            FaturaCtrl.AtualizarPorAssinatura(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        case "F_C":
                            Console.WriteLine(@"Listando faturas de um cliente");
                            FaturaCtrl.CancelarFatura(evento.IdTabela);
                            EventoDAL.MarcaRegistroProcessadoComo("P", evento.Id_Guid);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    if (File.Exists(Application.StartupPath + "\\" + fileTrigger))
                        File.Delete(Application.StartupPath + "\\" + fileTrigger);

                    EventoDAL.MarcaRegistroProcessadoComo("E", evento.Id_Guid);
                }
            }
        }
    }
}
