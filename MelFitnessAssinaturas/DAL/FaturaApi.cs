using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Singletons;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;

namespace MelFitnessAssinaturas.DAL
{
    public class FaturaApi
    {
        private FaturaDal faturaDal = new FaturaDal();

        public void CancelaAssinaturaApi(FaturaDb faturaDb)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = SistemaSingleton.Instancia.TokenApi;
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Invoices.CancelInvoice(faturaDb.IdApi);

                faturaDal.FaturaCanceladaAtualizaStatus(response, faturaDb.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ListInvoicesResponse ListaFaturasPorAssinatura(string _id_Api, int _page = 1)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = SistemaSingleton.Instancia.TokenApi;
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Invoices.GetInvoices(subscriptionId: _id_Api, page: _page);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
