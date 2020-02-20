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
                    var assinaturaModelApi = AssinaturaDTO.ConverteAssinaturaDbEmApi(assinatura);
                    var id_api = assinaturaApi.GravaAssinaturaApi(assinaturaModelApi);
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

        /// <summary>
        /// Busca assinaturas elegíveis a serem canceladas. Cancela na API e "fecha" elas no Banco de dados.
        /// </summary>
        /// <returns>numero de assinaturas canceladas para registro</returns>
        public int CancelarAssinaturas()
        {
            try
            {
                var numAssinaturasCancelas = 0;

                var listaAssinaturasCanceladas = assinaturaDal.ListaAssinaturasDb("C");

                foreach (var assinatura in listaAssinaturasCanceladas)
                {
                    assinaturaApi.CancelaAssinaturaApi(assinatura.Id_Api);

                    numAssinaturasCancelas++;

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

                return numAssinaturasCancelas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
