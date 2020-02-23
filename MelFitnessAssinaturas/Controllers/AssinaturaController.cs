﻿using MelFitnessAssinaturas.DAL;
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

                var log = new LogApiMundipaggController();
                log.Incluir(new LogApiMundipagg()
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
        /// Busca assinaturas elegíveis a serem canceladas. Cancela na API e "fecha" elas no Banco de dados.
        /// </summary>
        /// <returns>numero de assinaturas canceladas para registro</returns>
        public void CancelarAssinaturaApi(string _id)
        {
            try
            {
                var assinatura = assinaturaDal.GetAssinaturaDb(_id);

                    assinaturaApi.CancelaAssinaturaApi(assinatura.Id_Api);

                    var log = new LogApiMundipaggController();
                    log.Incluir(new LogApiMundipagg()
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

                assinaturaApi.ItemIncluirNaAssinatura(assinatura, assinaturaItemApi);

                var log = new LogApiMundipaggController();
                log.Incluir(new LogApiMundipagg()
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

        public void EditarItemApi(string idTabela)
        {
            try
            {
                var assinaturaItem = assinaturaDal.GetItemAssinatura(idTabela);
                var assinatura = assinaturaDal.GetAssinaturaDb(assinaturaItem.Id_Assinatura.ToString());
                var assinaturaItemApi = AssinaturaDTO.ConverteItemAlteradoDbEmApi(assinaturaItem);

                assinaturaApi.ItemEditarNaAssinatura(assinatura, assinaturaItem, assinaturaItemApi);

                var log = new LogApiMundipaggController();
                log.Incluir(new LogApiMundipagg()
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
    }
}
