using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.DTO;
using MelFitnessAssinaturas.Models;
using System;

namespace MelFitnessAssinaturas.Controllers
{
    public class AssinaturaController
    {
        private AssinaturaDal assinaturaDal = new AssinaturaDal();
        private AssinaturaApi assinaturaApi = new AssinaturaApi();

        /// <summary>
        /// Pesquisa nova assinatura no banco de dados, popula seus dados e relacionamentos e grava na API
        /// </summary>
        /// <returns>true ou false</returns>
        public bool CadastraNovaAssinaturaApi(string id_assinatura)
        {
            try
            {
                var NovaAssinatura = assinaturaDal.GetAssinaturaDb(id_assinatura);

                //transferir as assinaturas do banco para objetos da Api e registrar
                var assinaturaModelApi = AssinaturaDTO.ConverteAssinaturaDbEmApi(NovaAssinatura);
                var id_api = assinaturaApi.GravaAssinaturaApi(assinaturaModelApi);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Assinatura {NovaAssinatura.Texto_Fatura} gravada",
                    DtEvento = DateTime.Now,
                    NomeCliente = NovaAssinatura.Cliente.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = id_api
                });

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// Altera o cartao em uma assinatura
        /// </summary>
        /// <param name="idTabela">id da assinatura</param>
        /// <param name="codAux1">id do novo cartao</param>
        public void AlteraCartaoNaAssinatura(string idTabela, string codAux1)
        {
            try
            {
                var cartaoDal = new CartaoDal();
                var clienteDal = new ClienteDal();

                var assinatura = assinaturaDal.GetAssinaturaDb(idTabela);
                var cartao = cartaoDal.GetCartaoDb(codAux1);
                var cliente = clienteDal.GetClienteDb(cartao.Cod_Cli.ToString());

                var cartaoApi = CartaoDTO.ConverteNovoCartaoDbEmApi(cartao, cliente);

                assinaturaApi.AlteraCartaoEmAssinatura(assinatura.Id_Api, cartaoApi, cartao.Id.ToString());


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Busca assinaturas elegíveis a serem canceladas. Cancela na API e "fecha" elas no Banco de dados.
        /// </summary>
        /// <returns>numero de assinaturas canceladas para registro</returns>
        public void CancelarAssinaturaApi(string _id)
        {
            try
            {
                var assinatura = assinaturaDal.GetAssinaturaDb(_id);

                assinaturaApi.CancelaAssinaturaApi(assinatura.Id_Api);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Assinatura {assinatura.Texto_Fatura}/{assinatura.Id_Api} cancelada",
                    DtEvento = DateTime.Now,
                    NomeCliente = assinatura.Cliente.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = assinatura.Id_Api
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Incluir item em uma assinatura na API
        /// </summary>
        /// <param name="idTabela">código do item de assinatura</param>
        public void IncluirItemApi(string idTabela)
        {
            try
            {
                var assinaturaItem = assinaturaDal.GetItemAssinatura(idTabela);
                var assinatura = assinaturaDal.GetAssinaturaDb(assinaturaItem.Id_Assinatura.ToString());
                var assinaturaItemApi = AssinaturaDTO.ConverteItemNovoDbEmApi(assinaturaItem);

                assinaturaApi.ItemIncluirNaAssinatura(assinatura, assinaturaItem, assinaturaItemApi);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Incluído item de assinatura {assinaturaItem.Descricao} na Assinatura no.{assinatura.Id}",
                    DtEvento = DateTime.Now,
                    NomeCliente = assinatura.Cliente.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = assinatura.Id_Api
                });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoverItemApi(string idTabela)
        {
            try
            {
                var assinaturaItem = assinaturaDal.GetItemAssinatura(idTabela);
                var assinatura = assinaturaDal.GetAssinaturaDb(assinaturaItem.Id_Assinatura.ToString());

                assinaturaApi.ItemRemoverNaAssinatura(assinatura.Id_Api, assinaturaItem.Id_Api);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Removido item de assinatura {assinaturaItem.Descricao} na Assinatura no.{assinatura.Id}",
                    DtEvento = DateTime.Now,
                    NomeCliente = assinatura.Cliente.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = assinatura.Id_Api
                });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EditarItemApi(string idTabela)
        {
            try
            {
                var assinaturaItem = assinaturaDal.GetItemAssinatura(idTabela);
                var assinatura = assinaturaDal.GetAssinaturaDb(assinaturaItem.Id_Assinatura.ToString());
                var assinaturaItemApi = AssinaturaDTO.ConverteItemAlteradoDbEmApi(assinaturaItem);

                assinaturaApi.ItemEditarNaAssinatura(assinatura, assinaturaItem, assinaturaItemApi);

                var log = new LogSyncController();
                log.Incluir(new LogSync()
                {
                    Descricao = $"Editado item de assinatura {assinaturaItem.Descricao} na Assinatura no.{assinatura.Id}",
                    DtEvento = DateTime.Now,
                    NomeCliente = assinatura.Cliente.Nome,
                    Tipo = Enums.TipoLogEnum.As,
                    IdApi = assinatura.Id_Api
                });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
