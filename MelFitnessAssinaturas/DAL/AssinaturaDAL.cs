using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.DAL
{
    public class AssinaturaDAL : BaseDAL
    {
        /// <summary>
        /// Busca as assinaturas pelo status
        /// </summary>
        /// <param name="_status"> N - nova, E - editada, C - Cancelada, F - Fechado</param>
        /// <returns>Lista de assinaturas com os itens populados</returns>
        public List<AssinaturaDb> ListaAssinaturasDb(string _status)
        {
            try
            {
                conn = ConexaoBd.GetConnection();
                List<AssinaturaDb> listaAssinaturaDb = new List<AssinaturaDb>();

                SqlCommand cmd = new SqlCommand("select a.id, a.dt_inicio, a.intervalo_quantidade, " + 
                    " a.dia_cobranca, a.quant_parcelas, a.texto_fatura, a.valor_minimo, a.status, " +
                    " from rec_assinatura a " +
                    " where a.status = @Status ", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Status";
                param.Value = _status;

                cmd.Parameters.Add(param);

                reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                 // popular o model com os itens da assinatura . Pode colocar aqui ou em um método privado
                }
                return listaAssinaturaDb;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Ao gravar/atualizar uma assinatura na API atualizar o status e a referência do id da API no banco de dados
        /// </summary>
        /// <param name="_code">codigo ID da assinatura no banco</param>
        /// <param name="_id_api">Código de identificação devolvido pela API</param>
        /// <returns>Número de linha atualizadas</returns>
        public int AssinaturaGravadaNaApiAtualizaBanco(string _code, string _id_api)
        {
            try
            {
                conn = ConexaoBd.GetConnection();

                SqlCommand cmd = new SqlCommand("update rec_assinatura " +
                " set status = 'F', id_api = @_id_api " +
                " where id = @id ", conn);

                SqlParameter param1 = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = _code
                };

                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter
                {
                    ParameterName = "@_id_api",
                    Value = _id_api
                };

                cmd.Parameters.Add(param2);

                var rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
