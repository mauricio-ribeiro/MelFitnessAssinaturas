using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas.DAL
{
    public class AssinaturaDal
    {

        private const string Camada = "AssinaturaDal";
        

        /// <summary>
        /// Busca as assinaturas pelo status
        /// </summary>
        /// <param name="_status"> N - nova, E - editada, C - Cancelada, F - Fechado</param>
        /// <returns>Lista de assinaturas com os itens populados</returns>
        public List<AssinaturaDb> ListaAssinaturasDb(string _status)
        {

            const string metodo = "ListaAssinaturasDb";

            var clienteDal = new ClienteDal();
            var meioPagamentoDal = new MeioPagamentoDal();
            var listaAssinaturaDb = new List<AssinaturaDb>();


            try
            {
            
                var sql = new StringBuilder();

                sql.Append("select a.id, a.dt_inicio, a.intervalo_quantidade,");
                sql.Append("a.dia_cobranca, a.quant_parcelas, a.texto_fatura, a.valor_minimo, a.status,");
                sql.Append("a.id_cliente, a.id_cartao from rec_assinatura a ");
                sql.Append("where a.status = @Status;");
                
                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(),conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Status",_status);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                var assinatura = new AssinaturaDb
                                {
                                    Id = dr.GetInt32(dr.GetOrdinal("id")),
                                    Dt_Inicio = dr.GetDateTime(dr.GetOrdinal("dt_inicio")),
                                    Intervalo = dr["intervalo"].ToString(),
                                    Intervalo_Quantidade = dr.GetInt32(dr.GetOrdinal("intervalo_quantidade")),
                                    Dia_Cobranca = dr.GetInt32(dr.GetOrdinal("dia_cobranca")),
                                    Quant_Parcelas = dr.GetInt32(dr.GetOrdinal("quant_parcelas")),
                                    Texto_Fatura = dr["texto_fatura"].ToString(),
                                    Valor_Minimo = dr.GetDouble(dr.GetOrdinal("valor_minimo")),
                                    Status = dr["status"].ToString(),
                                    Cliente = clienteDal.GetClienteDb(dr["id_cliente"].ToString()),
                                    MeioPagamento = meioPagamentoDal.GetCartaoDb(dr["id_cartao"].ToString())
                                };

                                // popular o model com os itens da assinatura . Pode colocar aqui ou em um método privado
                                assinatura.ItensAssinatura = GetItensAssinatura(assinatura.Id);

                                listaAssinaturaDb.Add(assinatura);

                            }
                        }

                    }     
                }

                return listaAssinaturaDb;

            }
            catch (SqlException sqlException)
            {

                string strMensagem = "";
                strMensagem = LogDatabaseErrorUtil.CreateErrorDatabaseMessage(sqlException);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                sqlException.Data["MensagemCustomizada"] = LogDatabaseErrorUtil.ValidateDataBaseErrorNumber(sqlException.Number);
                sqlException.Data["Metodo"] = metodo;
                sqlException.Data["Classe"] = Camada;
                sqlException.Data["Hora"] = DateTime.Now;

                throw;

            }
            catch (Exception ex)
            {

                string strMensagem = "";

                strMensagem = LogDatabaseErrorUtil.CreateErrorMessage(ex);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                ex.Data["MensagemCustomizada"] = "Ocorreu um erro ao tentar executar a operação.";
                ex.Data["Metodo"] = metodo;
                ex.Data["Classe"] = Camada;
                ex.Data["Hora"] = DateTime.Now;

                throw;

            }
        }

        /// <summary>
        /// Monta os itens de assinatura
        /// </summary>
        /// <param name="id">id da assinatura</param>
        /// <returns></returns>
        private List<AssinaturaItemDb> GetItensAssinatura(int id)
        {

            const string metodo = "GetItensAssinatura";

            var listaItens = new List<AssinaturaItemDb>();

            try
            {
                
                var sql = new StringBuilder();

                sql.Append("select i.id_assinatura, i.descricao, i.ciclos, i.quant, i.status from rec_assinatura_item i ");
                sql.Append("where i.status = 'A' and i.id_assinatura = @id");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id",id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new AssinaturaItemDb
                                {
                                    Id_Assinatura = dr.GetInt32(dr.GetOrdinal("id_assinatura")),
                                    Descricao = dr["descricao"].ToString(),
                                    Ciclos = dr.GetInt32(dr.GetOrdinal("ciclos")),
                                    Quant = dr.GetInt32(dr.GetOrdinal("quant")),
                                    Status = dr["status"].ToString()
                                };

                                listaItens.Add(item);
                            }
                        }

                    }
                }

                return listaItens;
            }
            catch (SqlException sqlException)
            {

                string strMensagem = "";
                strMensagem = LogDatabaseErrorUtil.CreateErrorDatabaseMessage(sqlException);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                sqlException.Data["MensagemCustomizada"] = LogDatabaseErrorUtil.ValidateDataBaseErrorNumber(sqlException.Number);
                sqlException.Data["Metodo"] = metodo;
                sqlException.Data["Classe"] = Camada;
                sqlException.Data["Hora"] = DateTime.Now;

                throw;

            }
            catch (Exception ex)
            {

                string strMensagem = "";

                strMensagem = LogDatabaseErrorUtil.CreateErrorMessage(ex);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                ex.Data["MensagemCustomizada"] = "Ocorreu um erro ao tentar executar a operação.";
                ex.Data["Metodo"] = metodo;
                ex.Data["Classe"] = Camada;
                ex.Data["Hora"] = DateTime.Now;

                throw;

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

            const string metodo = "GetItensAssinatura";

            try
            {
                
                var sql = new StringBuilder();
                int rowsAffected;

                sql.Append("update rec_assinatura set status = 'F', id_api = @_id_api where id = @id;");
                
                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id",_code);
                        cmd.Parameters.AddWithValue("@_id_api", _code);
                        rowsAffected = cmd.ExecuteNonQuery();
                    }    
                }
                
                return rowsAffected;

            }
            catch (SqlException sqlException)
            {

                string strMensagem = "";
                strMensagem = LogDatabaseErrorUtil.CreateErrorDatabaseMessage(sqlException);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                sqlException.Data["MensagemCustomizada"] = LogDatabaseErrorUtil.ValidateDataBaseErrorNumber(sqlException.Number);
                sqlException.Data["Metodo"] = metodo;
                sqlException.Data["Classe"] = Camada;
                sqlException.Data["Hora"] = DateTime.Now;

                throw;

            }
            catch (Exception ex)
            {

                string strMensagem = "";

                strMensagem = LogDatabaseErrorUtil.CreateErrorMessage(ex);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, metodo);

                ex.Data["MensagemCustomizada"] = "Ocorreu um erro ao tentar executar a operação.";
                ex.Data["Metodo"] = metodo;
                ex.Data["Classe"] = Camada;
                ex.Data["Hora"] = DateTime.Now;

                throw;

            }
        }
    }
}
