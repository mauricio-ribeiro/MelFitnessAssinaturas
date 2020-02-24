using MelFitnessAssinaturas.Models;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.DAL
{
    public class AssinaturaApi
    {

        private AssinaturaDal assinaturaDal = new AssinaturaDal();

        public void CancelaAssinaturaApi(string id_Api)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                string subscrptionId = id_Api;

                var request = new CreateCancelSubscriptionRequest
                {
                    CancelPendingInvoices = true
                };

                var response = client.Subscriptions.CancelSubscription(subscrptionId, request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string GravaAssinaturaApi(CreateSubscriptionRequest assinaturaApi)
        {
            // Secret key fornecida pela Mundipagg
            var basicAuthUserName = "sk_test_4tdVXpseumRmqbo";

            // Senha em branco. Passando apenas a secret key
            var basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            var response = client.Subscriptions.CreateSubscription(assinaturaApi);

            assinaturaDal.AssinaturaGravadaNaApiAtualizaBanco(assinaturaApi.Metadata["Id"], response.Id);

            return response.Id;
        }

        public void ItemIncluirNaAssinatura(AssinaturaDb assinaturaDb, AssinaturaItemDb assinaturaItemDb, CreateSubscriptionItemRequest assinaturaItemApi)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Subscriptions.CreateSubscriptionItem(assinaturaDb.Id_Api, assinaturaItemApi);

                assinaturaDal.ItemAssinaturaGravadaNaApiAtualizaBanco(assinaturaItemDb.Id.ToString(), response.Id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AlteraCartaoEmAssinatura(string id_Api, CreateCardRequest cartaoApi, string idCartaoDb)
        {
            try
            {

                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var assinaturaApi = new UpdateSubscriptionCardRequest
                {
                    Card = cartaoApi
                };

                var response = client.Subscriptions.UpdateSubscriptionCard(id_Api, assinaturaApi);

                var cartaoDal = new CartaoDal();

                cartaoDal.CartaoGravadoNaApiAtualizaBanco(idCartaoDb, response.Card.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AlteraDataInicioAssinatura(string id_Api, int dias)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);


                var request = new UpdateSubscriptionStartAtRequest
                {
                    StartAt = DateTime.UtcNow.AddDays(2)
                };


                client.Subscriptions.UpdateSubscriptionStartAt(id_Api, request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            throw new NotImplementedException();
        }

        public void AlteraDataFaturamentoAssinatura(string id_Api, int dias)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var request = new UpdateSubscriptionBillingDateRequest
                {
                    NextBillingAt = DateTime.UtcNow.AddDays(dias)
                };

                client.Subscriptions.UpdateSubscriptionBillingDate(id_Api, request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ItemEditarNaAssinatura(AssinaturaDb assinaturaDb, AssinaturaItemDb assinaturaItemDb, UpdateSubscriptionItemRequest assinaturaItemApi)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Subscriptions.UpdateSubscriptionItem(assinaturaDb.Id_Api, assinaturaItemDb.Id.ToString(), assinaturaItemApi);

                assinaturaDal.ItemAssinaturaGravadaNaApiAtualizaBanco(assinaturaItemDb.Id.ToString(), response.Id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ItemRemoverNaAssinatura(string assinaturaId_api, string itemAssinatudaId_api)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Subscriptions.DeleteSubscriptionItem(assinaturaId_api, itemAssinatudaId_api);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
