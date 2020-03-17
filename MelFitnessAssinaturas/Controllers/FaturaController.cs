using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.DTO;
using MelFitnessAssinaturas.Models;
using MundiAPI.PCL.Models;
using System;

namespace MelFitnessAssinaturas.Controllers
{
    public class FaturaController
    {
        private FaturaDal faturaDal = new FaturaDal();
        private FaturaDTO faturaDTO = new FaturaDTO();
        private AssinaturaDal assinaturaDal = new AssinaturaDal();
        private FaturaApi faturaApi = new FaturaApi();

        public void AtualizarPorAssinatura(string idTabela)
        {
            try
            {
                var assinatura = assinaturaDal.GetAssinaturaDb(idTabela);
                ListInvoicesResponse faturasDaAssinatura = faturaApi.ListaFaturasPorAssinatura(assinatura.Id_Api);
                if (faturasDaAssinatura.Data.Count > 0)
                {
                    var auxNex = faturasDaAssinatura.Paging.Next;
                    do
                    {
                        foreach (var fatura in faturasDaAssinatura.Data)
                        {
                            FaturaDb faturaDb = faturaDTO.ConverteApiEmDB(fatura);
                            faturaDal.RegistraFatura(faturaDb);
                        }

                        if (auxNex != null)
                        {
                            var nextPage = Convert.ToInt32(auxNex.Substring(auxNex.IndexOf("?") + 5, 1));
                            faturasDaAssinatura = faturaApi.ListaFaturasPorAssinatura(assinatura.Id_Api, nextPage);
                        }

                    } while (auxNex != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CancelarFatura(string idTabela)
        {
            try
            {
                FaturaDb faturaDb = faturaDal.GetFaturaDb(idTabela);
                faturaApi.CancelaAssinaturaApi(faturaDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
