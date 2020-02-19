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
                ClienteDAL clienteDal = new ClienteDAL();
                MeioPagamentoDAL meioPagamentoDal = new MeioPagamentoDAL();

                List<AssinaturaDb> listaAssinaturaDb = new List<AssinaturaDb>();

                SqlCommand cmd = new SqlCommand("select a.id, a.dt_inicio, a.intervalo_quantidade, " + 
                    " a.dia_cobranca, a.quant_parcelas, a.texto_fatura, a.valor_minimo, a.status, " +
                    " a.id_cliente, a.id_cartao " +
                    " from rec_assinatura a " +
                    " where a.status = @Status ", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Status";
                param.Value = _status;

                cmd.Parameters.Add(param);

                reader = cmd.ExecuteReader();


                while (reader.Read())
                {

                    var assinatura = new AssinaturaDb();

                    assinatura.Id = reader.GetInt32(reader.GetOrdinal("id"));
                    assinatura.Dt_Inicio = reader.GetDateTime(reader.GetOrdinal("dt_inicio"));
                    assinatura.Intervalo = reader["intervalo"].ToString();
                    assinatura.Intervalo_Quantidade = reader.GetInt32(reader.GetOrdinal("intervalo_quantidade"));
                    assinatura.Dia_Cobranca = reader.GetInt32(reader.GetOrdinal("dia_cobranca"));
                    assinatura.Quant_Parcelas = reader.GetInt32(reader.GetOrdinal("quant_parcelas"));
                    assinatura.Texto_Fatura = reader["texto_fatura"].ToString();
                    assinatura.Valor_Minimo = reader.GetDouble(reader.GetOrdinal("valor_minimo"));
                    assinatura.Status = reader["status"].ToString();

                    // popular o model com os itens da assinatura . Pode colocar aqui ou em um método privado


                    assinatura.Cliente = clienteDal.GetClienteDb(reader["id_cliente"].ToString());
                    assinatura.MeioPagamento = meioPagamentoDal.GetCartaoDb(reader["id_cartao"].ToString());
                    assinatura.ItensAssinatura = GetItensAssinatura(assinatura.Id);

                    listaAssinaturaDb.Add(assinatura);

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
        /// Monta os itens de assinatura
        /// </summary>
        /// <param name="id">id da assinatura</param>
        /// <returns></returns>
        private List<AssinaturaItemDb> GetItensAssinatura(int id)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("select i.id_assinatura, i.descricao, i.ciclos, i.quant, i.status " +
                    " from rec_assinatura_item i " +
                    " where i.status = 'A' and i.id_assinatura = @id", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@id";
                param.Value = id;

                cmd.Parameters.Add(param);

                reader = cmd.ExecuteReader();

                var listaItens = new List<AssinaturaItemDb>();

                while (reader.Read())
                {
                    var item = new AssinaturaItemDb();
                    item.Id_Assinatura = reader.GetInt32(reader.GetOrdinal("id_assinatura"));
                    item.Descricao = reader["descricao"].ToString();
                    item.Ciclos = reader.GetInt32(reader.GetOrdinal("ciclos"));
                    item.Quant = reader.GetInt32(reader.GetOrdinal("quant"));
                    item.Status = reader["status"].ToString();

                    listaItens.Add(item);
                }

                return listaItens;
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
