using System;
using MelFitnessAssinaturas.Singletons;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas.DAL
{
    public class ClienteApi
    {
        /// <summary>
        /// Edita o cliente cadastrado na API
        /// </summary>
        /// <param name="id_api">codigo do id da APi. ex: "cus_6l5dMWZ0hkHZ4XnE"</param>
        /// <param name="cliApi">Modelo da entidade cliente no formado de requisição da api</param>
        public void EditaClienteApi( string id_api, UpdateCustomerRequest cliApi)
        {
            try
            {
                // Secret key fornecida pela Mundipagg
                string basicAuthUserName = SistemaSingleton.Instancia.TokenApi;
                // Senha em branco. Passando apenas a secret key
                string basicAuthPassword = "";

                var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

                var response = client.Customers.UpdateCustomer(id_api, cliApi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
