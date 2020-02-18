using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class AssinaturaController
    {
        /// <summary>
        /// Pesquisa novas assinaturas no banco de dados, popula seus dados e relacionamentos e grava na API
        /// </summary>
        /// <returns>quantas assinaturas novas foram gravadas.</returns>
        public int CadastraNovasAssinaturas()
        {
            try
            {
                var contAssinaturasGravadas = 0;

                var assinaturaDal = new AssinaturaDAL();

                var listaNovasAssinaturas = assinaturaDal.ListaAssinaturasDb("N");

                foreach (var assinatura in listaNovasAssinaturas)
                {
                    //transferir as assinaturas do banco para objetos da Api e registrar
                    CreateSubscriptionRequest assinaturaApi = ConverteAssinaturaDbEmApi(assinatura);
                    String id_api = GravaAssinaturaApi(assinaturaApi);
                    contAssinaturasGravadas++;
                    var log = new LogApiMundipaggController();
                    log.Incluir(new LogApiMundipagg()
                    {
                        Descricao = String.Format("Assinatura {0} gravada", assinatura.Texto_Fatura),
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
            throw new NotImplementedException();
        }

        private String GravaAssinaturaApi(CreateSubscriptionRequest assinaturaApi)
        {
            throw new NotImplementedException();
        }
    }
}
