using MelFitnessAssinaturas.Enums;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.DAL
{
    public class LogApiMundipaggDal
    {

        private const string Camada = "LogApiMundipaggDal";

        public void Incluir(LogApiMundipagg entidade)
        {

            const string Metodo = "Incluir";
            

            try
            {

                var sql = new StringBuilder();

                sql.Append("INSERT INTO log_apimundipagg (dt_evento,tipo,descricao,cliente,id_api,valor,dt_documento) VALUES ");
                sql.Append("(@dt_evento,@tipo,@descricao,@cliente,@id_api,@valor,@dt_documento);");
                
                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@dt_evento",entidade.DtEvento);
                        cmd.Parameters.AddWithValue("@tipo",Enum.GetName(typeof(TipoLogEnum),entidade.Tipo));
                        cmd.Parameters.AddWithValue("@descricao", entidade.Descricao);
                        cmd.Parameters.AddWithValue("@cliente", entidade.NomeCliente);
                        cmd.Parameters.AddWithValue("@id_api", entidade.IdApi);
                        cmd.Parameters.AddWithValue("@valor", entidade.Valor);
                        cmd.Parameters.AddWithValue("@dt_documento", entidade.DtDocumento);
                        cmd.ExecuteNonQuery();
                    }
                }
                
            }
            catch (SqlException sqlException)
            {

                string strMensagem = "";
                strMensagem = LogDatabaseErrorUtil.CreateErrorDatabaseMessage(sqlException);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, Metodo);

                sqlException.Data["MensagemCustomizada"] = LogDatabaseErrorUtil.ValidateDataBaseErrorNumber(sqlException.Number);
                sqlException.Data["Metodo"] = Metodo;
                sqlException.Data["Classe"] = Camada;
                sqlException.Data["Hora"] = DateTime.Now;

                throw;

            }
            catch (Exception ex)
            {

                string strMensagem = "";

                strMensagem = LogDatabaseErrorUtil.CreateErrorMessage(ex);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, Metodo);

                ex.Data["MensagemCustomizada"] = "Ocorreu um erro ao tentar executar a operação.";
                ex.Data["Metodo"] = Metodo;
                ex.Data["Classe"] = Camada;
                ex.Data["Hora"] = DateTime.Now;

                throw;

            }
        }

        public IEnumerable<LogApiMundipagg> ObterTodos(params object[] parametros)
        {

            const string Metodo = "ObterTodos";
            var listaLogApi = new List<LogApiMundipagg>();

            var nomeCliente = Convert.ToString(parametros[0]);
            var strDtInicial = $"{Convert.ToDateTime(parametros[1]):yyyy-MM-dd}";
            var strDtFinal = $"{Convert.ToDateTime(parametros[2]):yyyy-MM-dd}";
            var tipo = (TipoLogEnum)parametros[3] == TipoLogEnum.To ? (object)"%" : Enum.GetName(typeof(TipoLogEnum),(TipoLogEnum) parametros[3]);


            try
            {

                var sql = new StringBuilder();

                sql.Append("SELECT dt_evento,tipo,descricao,cliente,id_api,valor,dt_documento FROM log_apimundipagg ");
                sql.Append("WHERE CAST(dt_evento as date) BETWEEN @strDtInicial AND @strDtFinal AND cliente LIKE @cliente AND tipo LIKE @tipo;");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@strDtInicial", strDtInicial);
                        cmd.Parameters.AddWithValue("@strDtFinal", strDtFinal);
                        cmd.Parameters.AddWithValue("@cliente", "%" + nomeCliente + "%");
                        cmd.Parameters.AddWithValue("@tipo", tipo);

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    
                                    var logApiMundipagg = new LogApiMundipagg
                                    {
                                        DtEvento = Convert.ToDateTime(dr["dt_evento"]),
                                        Tipo = (TipoLogEnum) Enum.Parse(typeof(TipoLogEnum),Convert.ToString(dr["tipo"])),
                                        Descricao = Convert.ToString(dr["descricao"]),
                                        NomeCliente = Convert.ToString(dr["cliente"]),
                                        IdApi = Convert.ToString(dr["id_api"]),
                                        Valor = Convert.ToDecimal(dr["valor"]),
                                        DtDocumento = Convert.ToDateTime(dr["dt_documento"])
                                    };

                                    listaLogApi.Add(logApiMundipagg);

                                }
                            }
                        }


                    }
                }


                return listaLogApi;

            }
            catch (SqlException sqlException)
            {

                string strMensagem = "";
                strMensagem = LogDatabaseErrorUtil.CreateErrorDatabaseMessage(sqlException);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, Metodo);

                sqlException.Data["MensagemCustomizada"] = LogDatabaseErrorUtil.ValidateDataBaseErrorNumber(sqlException.Number);
                sqlException.Data["Metodo"] = Metodo;
                sqlException.Data["Classe"] = Camada;
                sqlException.Data["Hora"] = DateTime.Now;

                throw;

            }
            catch (Exception ex)
            {

                string strMensagem = "";

                strMensagem = LogDatabaseErrorUtil.CreateErrorMessage(ex);
                LogDatabaseErrorUtil.LogFileWrite(strMensagem, Metodo);

                ex.Data["MensagemCustomizada"] = "Ocorreu um erro ao tentar executar a operação.";
                ex.Data["Metodo"] = Metodo;
                ex.Data["Classe"] = Camada;
                ex.Data["Hora"] = DateTime.Now;

                throw;

            }

        }
    }
}
