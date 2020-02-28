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
            var meioPagamentoDal = new CartaoDal();
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
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Status", _status);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                var assinatura = new AssinaturaDb
                                {
                                    Id = Convert.ToInt32(dr["id"]),
                                    Dt_Inicio = Convert.ToDateTime(dr["dt_inicio"]),
                                    Intervalo = dr["intervalo"].ToString(),
                                    Intervalo_Quantidade = Convert.ToInt32(dr["intervalo_quantidade"]),
                                    Dia_Cobranca = Convert.ToInt32(dr["dia_cobranca"]),
                                    Quant_Parcelas = Convert.ToInt32(dr["quant_parcelas"]),
                                    Texto_Fatura = dr["texto_fatura"].ToString(),
                                    Valor_Minimo = Convert.ToInt32(dr["valor_minimo"]),
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

        public int ItemAssinaturaGravadaNaApiAtualizaBanco(string _code, string _id_api)
        {
            const string metodo = "GetItensAssinatura";

            try
            {

                var sql = new StringBuilder();
                int rowsAffected;

                sql.Append("update rec_assinatura_item set status = 'F', id_api = @_id_api where id = @id;");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", _code);
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

        /// <summary>
        /// Busca as assinatura pelo id d tabela
        /// </summary>
        /// <param name="_id">id da tabela rec_assinatura</param>
        /// <returns>Model assinaturaDb com os itens populados</returns>
        public AssinaturaDb GetAssinaturaDb(string _id)
        {

            const string metodo = "GetAssinaturaDb";

            var clienteDal = new ClienteDal();
            var meioPagamentoDal = new CartaoDal();
            var assinatura = new AssinaturaDb();

            try
            {

                var sql = new StringBuilder();

                sql.Append("select a.id, a.dt_inicio, a.intervalo, a.intervalo_quantidade,");
                sql.Append("a.dia_cobranca, a.quant_parcelas, a.texto_fatura, a.valor_minimo, a.status,");
                sql.Append("a.id_cliente, a.id_cartao from rec_assinatura a ");
                sql.Append("where a.id = @id;");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", _id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                assinatura.Id = Convert.ToInt32(dr["id"]);
                                assinatura.Dt_Inicio = dr.GetDateTime(dr.GetOrdinal("dt_inicio"));
                                assinatura.Intervalo = dr["intervalo"].ToString();
                                assinatura.Intervalo_Quantidade = Convert.ToInt32(dr["intervalo_quantidade"]);
                                assinatura.Dia_Cobranca = Convert.ToInt32(dr["dia_cobranca"]);
                                assinatura.Quant_Parcelas = Convert.ToInt32(dr["quant_parcelas"]);
                                assinatura.Texto_Fatura = dr["texto_fatura"].ToString();
                                assinatura.Valor_Minimo = Convert.ToDouble(dr["valor_minimo"]);
                                assinatura.Status = dr["status"].ToString();
                                assinatura.Cliente = clienteDal.GetClienteDb(dr["id_cliente"].ToString());
                                assinatura.MeioPagamento = meioPagamentoDal.GetCartaoDb(dr["id_cartao"].ToString());

                                // popular o model com os itens da assinatura . Pode colocar aqui ou em um método privado
                                assinatura.ItensAssinatura = GetItensAssinatura(assinatura.Id);
                            }
                        }

                    }
                }

                return assinatura; ;

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

                sql.Append("select i.id, i.id_assinatura, i.descricao, i.ciclos, i.quant, i.status, i.id_api ");
                sql.Append(" from rec_assinatura_item i ");
                sql.Append("where i.status = 'A' and i.id_assinatura = @id");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var item = new AssinaturaItemDb
                                {
                                    Id_Assinatura = Convert.ToInt32(dr["id_assinatura"]),
                                    Descricao = dr["descricao"].ToString(),
                                    Ciclos = Convert.ToInt32(dr["ciclos"]),
                                    Quant = Convert.ToInt32(dr["quant"]),
                                    Status = dr["status"].ToString(),
                                    Id = Convert.ToInt32(dr["id"]),
                                    Id_Api = dr["id_api"].ToString()
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
        /// Busca UM item de assinatura
        /// </summary>
        /// <param name="id">id do item</param>
        /// <returns></returns>
        public AssinaturaItemDb GetItemAssinatura(string id)
        {

            const string metodo = "GetItemAssinatura";

            try
            {
                var item = new AssinaturaItemDb();

                var sql = new StringBuilder();

                sql.Append("select i.id, i.id_assinatura, i.descricao, i.ciclos, i.quant, i.status, i.id_api from rec_assinatura_item i ");
                sql.Append("where i.id = @id");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            item.Id = dr.GetInt32(dr.GetOrdinal("id"));
                            item.Id_Assinatura = dr.GetInt32(dr.GetOrdinal("id_assinatura"));
                            item.Descricao = dr["descricao"].ToString();
                            item.Ciclos = dr.GetInt32(dr.GetOrdinal("ciclos"));
                            item.Quant = dr.GetInt32(dr.GetOrdinal("quant"));
                            item.Status = dr["status"].ToString();
                            item.Id_Api = dr["id_api"].ToString();
                        }
                    }
                }

                return item;
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
                        cmd.Parameters.AddWithValue("@id", _code);
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
