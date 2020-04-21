using MelFitnessAssinaturas.Models;
using MundiAPI.PCL.Models;
using System;
using CryptoDll;

namespace MelFitnessAssinaturas.DTO
{
    public static class CartaoDTO
    {
        /// <summary>
        /// Converte entidade cartao em modelo de API
        /// </summary>
        /// <param name="cartao">objeto do cartaoDb</param>
        /// <param name="cliente">objeto do clienteDb</param>
        /// <returns>requisição CreateCardRequest</returns>
        public static CreateCardRequest ConverteNovoCartaoDbEmApi(CartaoDb cartao, ClienteDb cliente)
        {
            try
            {
                var createCartao = new CreateCardRequest
                {
                    Number = Crypto.Decifra(cartao.Numero_Cartao),
                    HolderName = Crypto.Decifra(cartao.Nome_Cartao),
                    ExpMonth =  Convert.ToInt32(Crypto.Decifra(cartao.Val_Mes.ToString())),
                    ExpYear = Convert.ToInt32(Crypto.Decifra(cartao.Val_Ano.ToString())),
                    Cvv = Crypto.Decifra(cartao.Cvc),
                    BillingAddress = new CreateAddressRequest
                    {
                        Line1 = cliente.Endereco_1,
                        Line2 = cliente.Endereco_2,
                        ZipCode = cliente.Cep,
                        City = cliente.Cidade,
                        State = cliente.Uf,
                        Country = "BR"
                    }
                };

                return createCartao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
