using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MelFitnessAssinaturas.DAL
{
    public class AssinaturaDal : BaseDal
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
                Conn = ConexaoBd.GetConnection();
                var clienteDal = new ClienteDal();
                var meioPagamentoDal = new MeioPagamentoDal();

                var listaAssinaturaDb = new List<AssinaturaDb>();

                var cmd = new SqlCommand("select a.id, a.dt_inicio, a.intervalo_quantidade, " + 
                                         " a.dia_cobranca, a.quant_parcelas, a.texto_fatura, a.valor_minimo, a.status, " +
                                         " a.id_cliente, a.id_cartao " +
                                         " from rec_assinatura a " +
                                         " where a.status = @Status ", Conn);

                var param = new SqlParameter
                {
                    ParameterName = "@Status",
                    Value = _status
                };

                cmd.Parameters.Add(param);

                Reader = cmd.ExecuteReader();


                while (Reader.Read())
                {

                    var assinatura = new AssinaturaDb
                    {
                        Id = Reader.GetInt32(Reader.GetOrdinal("id")),
                        Dt_Inicio = Reader.GetDateTime(Reader.GetOrdinal("dt_inicio")),
                        Intervalo = Reader["intervalo"].ToString(),
                        Intervalo_Quantidade = Reader.GetInt32(Reader.GetOrdinal("intervalo_quantidade")),
                        Dia_Cobranca = Reader.GetInt32(Reader.GetOrdinal("dia_cobranca")),
                        Quant_Parcelas = Reader.GetInt32(Reader.GetOrdinal("quant_parcelas")),
                        Texto_Fatura = Reader["texto_fatura"].ToString(),
                        Valor_Minimo = Reader.GetDouble(Reader.GetOrdinal("valor_minimo")),
                        Status = Reader["status"].ToString(),
                        Cliente = clienteDal.GetClienteDb(Reader["id_cliente"].ToString()),
                        MeioPagamento = meioPagamentoDal.GetCartaoDb(Reader["id_cartao"].ToString())
                    };


                    // popular o model com os itens da assinatura . Pode colocar aqui ou em um método privado

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
                Reader?.Close();
                Conn?.Close();
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

                var cmd = new SqlCommand("select i.id_assinatura, i.descricao, i.ciclos, i.quant, i.status " +
                                         " from rec_assinatura_item i " +
                                         " where i.status = 'A' and i.id_assinatura = @id", Conn);

                var param = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = id
                };

                cmd.Parameters.Add(param);

                Reader = cmd.ExecuteReader();

                var listaItens = new List<AssinaturaItemDb>();

                while (Reader.Read())
                {
                    var item = new AssinaturaItemDb();
                    item.Id_Assinatura = Reader.GetInt32(Reader.GetOrdinal("id_assinatura"));
                    item.Descricao = Reader["descricao"].ToString();
                    item.Ciclos = Reader.GetInt32(Reader.GetOrdinal("ciclos"));
                    item.Quant = Reader.GetInt32(Reader.GetOrdinal("quant"));
                    item.Status = Reader["status"].ToString();

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
                Reader?.Close();
                Conn?.Close();
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
                Conn = ConexaoBd.GetConnection();

                var cmd = new SqlCommand("update rec_assinatura " +
                                         " set status = 'F', id_api = @_id_api " +
                                         " where id = @id ", Conn);

                var param1 = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = _code
                };

                cmd.Parameters.Add(param1);

                var param2 = new SqlParameter
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
                Conn?.Close();
            }
        }
    }
}
