﻿using MundiAPI.PCL;
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

        public void ItemIncluirNaAssinatura(string id_Api, CreateSubscriptionItemRequest assinaturaItemApi)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = "sk_test_4tdVXpseumRmqbo";
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var request = new CreateSubscriptionItemRequest
                {
                    Description = assinaturaItemApi.Description,
                    Cycles = assinaturaItemApi.Cycles,
                    Quantity = assinaturaItemApi.Quantity,
                    PricingScheme = assinaturaItemApi.PricingScheme
                };

                var response = client.Subscriptions.CreateSubscriptionItem(id_Api, request);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}