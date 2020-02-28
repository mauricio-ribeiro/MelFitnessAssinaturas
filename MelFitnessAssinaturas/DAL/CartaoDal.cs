using System;
using System.Data.SqlClient;
using System.Text;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas.DAL
{
    public class CartaoDal
    {

        private const string Camada = "MeioPagamentoDal";

        public CartaoDb GetCartaoDb(string id)
        {

            const string metodo = "GetCartaoDb";


            try
            {
                CartaoDb cartao = null;
                var sql = new StringBuilder();

                sql.Append("select c.cod_cli, c.numero_cartao, c.bandeira, c.cpf,");
                sql.Append("c.cvc, c.val_mes, c.val_ano, c.status, c.id, c.id_api from cad_clientes_cartao c ");
                sql.Append("where c.id = @codCartao;");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@codCartao",id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                cartao = new CartaoDb
                                {
                                    Cod_Cli = Convert.ToInt32(dr["cod_cli"]),
                                    Numero_Cartao = dr["numero_cartao"].ToString(),
                                    Bandeira = dr["bandeira"].ToString(),
                                    Cpf = dr["cpf"].ToString(),
                                    Cvc = dr["cvc"].ToString(),
                                    Val_Mes = Convert.ToInt32(dr["val_mes"]),
                                    Val_Ano = Convert.ToInt32(dr["val_ano"]),
                                    Status = dr["status"].ToString(),
                                    Id = Convert.ToInt32(dr["id"]),
                                    Id_Api = dr["id_api"].ToString()
                                };
                            }
                        }
                    }
                }

                return cartao;
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
            /// <param name="_code">codigo ID do cartao no banco</param>
            /// <param name="_id_api">Código de identificação devolvido pela API</param>
            /// <returns>Número de linha atualizada</returns>
        public int CartaoGravadoNaApiAtualizaBanco(string _code, string _id_api)
        {

                const string metodo = "CartaoGravadoNaApiAtualizaBanco";

                try
                {

                    var sql = new StringBuilder();
                    int rowsAffected;

                    sql.Append("update cad_clientes_cartao set id_api = @_id_api where id = @id;");

                    using (var conn = ConexaoBd.GetConnection())
                    {
                        using (var cmd = new SqlCommand(sql.ToString(), conn))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", _code);
                            cmd.Parameters.AddWithValue("@_id_api", _id_api);
                            cmd.Parameters.AddWithValue("@_id_api", _id_api);
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
