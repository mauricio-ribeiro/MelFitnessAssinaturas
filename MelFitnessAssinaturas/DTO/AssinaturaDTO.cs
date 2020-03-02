using MelFitnessAssinaturas.Models;
using MundiAPI.PCL.Models;
using System.Collections.Generic;
using System;

namespace MelFitnessAssinaturas.DTO
{
    public static class AssinaturaDTO
    {
        public static CreateSubscriptionRequest ConverteAssinaturaDbEmApi(AssinaturaDb assinatura)
        {

            try
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
                //var discounts = new List<CreateDiscountRequest> {
                //new CreateDiscountRequest {
                //Cycles = 1,
                //Value = 0,
                //DiscountType = "percentage"
                //}
            //};

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

                    items.Add(i);
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
                    BillingType = "exact_day",
                    BillingDay = assinatura.Dia_Cobranca,
                    Installments = assinatura.Quant_Parcelas,
                    Customer = new CreateCustomerRequest
                    {
                        Name = assinatura.Cliente.Nome,
                        Email = assinatura.Cliente.Email
                    },
                    Card = card,
                   // Discounts = discounts,
                    Items = items,
                    Metadata = metadata
                };

                return request;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static CreateSubscriptionItemRequest ConverteItemNovoDbEmApi(AssinaturaItemDb assinaturaItem)
        {
            try
            {
                var item = new CreateSubscriptionItemRequest
                {
                    Description = assinaturaItem.Descricao,
                    Cycles = assinaturaItem.Ciclos,
                    Quantity = assinaturaItem.Quant,
                    PricingScheme = new CreatePricingSchemeRequest
                    {
                        Price = assinaturaItem.GetValor()
                    }
                };

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static UpdateSubscriptionItemRequest ConverteItemAlteradoDbEmApi(AssinaturaItemDb assinaturaItem)
        {
            try
            {
                var item = new UpdateSubscriptionItemRequest
                {
                    Description = assinaturaItem.Descricao,
                    Cycles = assinaturaItem.Ciclos,
                    Quantity = assinaturaItem.Quant,
                    PricingScheme = new UpdatePricingSchemeRequest
                    {
                        Price = assinaturaItem.GetValor()
                    },
                    Status = assinaturaItem.Status == "A" ? "active" : "inactive"
                };

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
