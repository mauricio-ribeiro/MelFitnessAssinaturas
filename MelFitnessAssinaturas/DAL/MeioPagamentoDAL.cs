using System;
using System.Data.SqlClient;
using System.Text;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas.DAL
{
    public class MeioPagamentoDal
    {

        private const string Camada = "MeioPagamentoDal";
        

        public MeioPagamentoDb GetCartaoDb(string id)
        {

            const string metodo = "GetCartaoDb";


            try
            {
                MeioPagamentoDb cartao = null;
                var sql = new StringBuilder();

                sql.Append("select c.cod_cli, c.numero_cartao, c.bandeira, c.cpf,");
                sql.Append("c.cvc, c.val_mes, c.val_ano, c.status, c.id, c.id_api from cad_clientes_cartao c ");
                sql.Append("where cli.cod_cli = @codCartao;");


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
                                cartao = new MeioPagamentoDb
                                {
                                    Cod_Cli = dr.GetInt32(dr.GetOrdinal("cod_cli")),
                                    Numero_Cartao = dr["numero_cartao"].ToString(),
                                    Bandeira = dr["bandeira"].ToString(),
                                    Cpf = dr["cpf"].ToString(),
                                    Cvc = dr["cvc"].ToString(),
                                    Val_Mes = dr.GetInt32(dr.GetOrdinal("val_mes")),
                                    Val_Ano = dr.GetInt32(dr.GetOrdinal("val_ano")),
                                    Status = dr["status"].ToString(),
                                    Id = dr.GetInt32(dr.GetOrdinal("id")),
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
    }
}
