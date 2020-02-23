using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.DTO;
using MelFitnessAssinaturas.Models;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class ClienteController
    {
        private ClienteDal clienteDal = new ClienteDal();
        private ClienteApi clienteApi = new ClienteApi();

        /// <summary>
        /// Atualiza um cliente editado no sistema para a API
        /// </summary>
        /// <param name="idTabela">codigo do cliente na tabela do banco de dados</param>
        public void AtualizaClienteApi(string idTabela)
        {
            try
            {
                var clienteDb = clienteDal.GetClienteDb(idTabela);
                var cliApi = ClienteDTO.ConverteEditadoClienteDbEmApi(clienteDb);
                clienteApi.EditaClienteApi(clienteDb.Id_Api, cliApi);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Cliente {clienteDb.Codigo} editado.",
                    DtEvento = DateTime.Now,
                    NomeCliente = clienteDb.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = clienteDb.Id_Api
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
