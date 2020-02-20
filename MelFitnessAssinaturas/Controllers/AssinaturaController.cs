using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;

namespace MelFitnessAssinaturas.Controllers
{
    public class AssinaturaController
    {
        private AssinaturaDal assinaturaDal = new AssinaturaDal();
        /// <summary>
        /// Pesquisa novas assinaturas no banco de dados, popula seus dados e relacionamentos e grava na API
        /// </summary>
        /// <returns>quantas assinaturas novas foram gravadas.</returns>
        public int CadastraNovasAssinaturas()
        {
            try
            {
                var contAssinaturasGravadas = 0;

                var listaNovasAssinaturas = assinaturaDal.ListaAssinaturasDb("N");

                foreach (var assinatura in listaNovasAssinaturas)
                {
                    //transferir as assinaturas do banco para objetos da Api e registrar
                    var assinaturaApi = ConverteAssinaturaDbEmApi(assinatura);
                    var id_api = GravaAssinaturaApi(assinaturaApi);
                    contAssinaturasGravadas++;

                    var log = new LogApiMundipaggController();
                    log.Incluir(new LogApiMundipagg()
                    {
                        Descricao = $"Assinatura {assinatura.Texto_Fatura} gravada",
                        DtEvento = DateTime.Now,
                        NomeCliente = assinatura.Cliente.Nome,
                        Tipo = Enums.TipoLogEnum.As,
                        IdApi = id_api
                    });
                }
                return contAssinaturasGravadas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        private CreateSubscriptionRequest ConverteAssinaturaDbEmApi(AssinaturaDb assinatura)
        {

            var billinAddress = new CreateAddressRequest
            {
                Line1 = assinatura.Cliente.Endereco_1,
                Line2 = assinatura.Cliente.Endereco_2,
                ZipCode = assinatura.Cliente.Cep,
                City = assinatura.Cliente.Cidade,
                State = assinatura.Cliente.Uf,
                Country = "BR"
            };

            var card = new CreateCardRequest
            {
                HolderName = assinatura.MeioPagamento.Nome_Cartao,
                Number = assinatura.MeioPagamento.Numero_Cartao,
                ExpMonth = assinatura.MeioPagamento.Val_Mes,
                ExpYear = assinatura.MeioPagamento.Val_Ano,
                Cvv = assinatura.MeioPagamento.Cvc,
                BillingAddress = billinAddress
            };

            // por enquanto não vai trabalhar com descontos
            //var discounts = new List<CreateDiscountRequest>
            //{
            //    new CreateDiscountRequest
            //    {
            //        Cycles = assinatura.
            //    }
            //}

            var items = new List<CreateSubscriptionItemRequest>();

            foreach (var item in assinatura.ItensAssinatura)
            {
                var i = new CreateSubscriptionItemRequest
                {
                    Description = item.Descricao,
                    Quantity = item.Quant,
                    PricingScheme = new CreatePricingSchemeRequest
                    {
                        Price = item.GetValor()
                    }
                };
            }

            var metadata = new Dictionary<string, string>
            {
                {"id", assinatura.Id.ToString()}
            };

            var request = new CreateSubscriptionRequest
            {
                PaymentMethod = "credit_card",
                Currency = "BRL",
                Interval = assinatura.Intervalo,
                IntervalCount = assinatura.Intervalo_Quantidade,
                BillingType = "prepaid",
                Installments = assinatura.Quant_Parcelas,
                Customer = new CreateCustomerRequest
                {
                    Name = assinatura.Cliente.Nome,
                    Email = assinatura.Cliente.Email
                },
                Card = card,
                Discounts = null,
                Items = items,
                Metadata = metadata
            };

            return request;

        }

        private string GravaAssinaturaApi(CreateSubscriptionRequest assinaturaApi)
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
    }
}
