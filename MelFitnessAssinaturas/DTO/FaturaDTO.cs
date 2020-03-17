using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;
using MundiAPI.PCL.Models;
using System;

namespace MelFitnessAssinaturas.DTO
{
    public class FaturaDTO
    {
        private ClienteDal clienteDal = new ClienteDal();

        public FaturaDb ConverteApiEmDB(GetInvoiceResponse fatura)
        {
            try
            {
                var cliente = clienteDal.GetClienteByIdApi(fatura.Customer.Id);
                var faturaDb = new FaturaDb
                {
                    CodCli = Convert.ToInt32(cliente.Codigo),
                    IdApi = fatura.Id,
                    Url = fatura.Url,
                    Valor = fatura.Amount / 100,
                    FormaPagamento = fatura.PaymentMethod,
                    QuantParcelas = fatura.Subscription.Installments,
                    Status = fatura.Status,
                    DtCobranca = fatura.DueAt,
                    DtVencimento = fatura.Cycle.BillingAt,
                    DtCriacao = fatura.CreatedAt,
                    IdAssinatura = Convert.ToInt32(fatura.Subscription.Metadata["id"]),
                    TotalDescontos = fatura.TotalDiscount /100,
                    TotalAcrescimos = fatura.TotalIncrement /100
                };

                if (fatura.CanceledAt != null)
                {
                    faturaDb.DtCancelamento = fatura.CanceledAt;
                }

                return faturaDb;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
