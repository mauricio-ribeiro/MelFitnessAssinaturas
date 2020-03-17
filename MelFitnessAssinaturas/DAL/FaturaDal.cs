using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using MundiAPI.PCL.Models;
using System;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.DAL
{
    public class FaturaDal
    {
        private const string Camada = "FaturaDal";


        public FaturaDb GetFaturaDb(string idTabela)
        {
            const string metodo = "GetFaturaDb";

            try
            {
                FaturaDb fatura = null;
                var sql = new StringBuilder();

                sql.Append(" SELECT id, url, valor, forma_pagamento, quant_parcelas, status, dt_cobranca, dt_vencimento, ");
                sql.Append(" dt_criacao, dt_cancelamento, id_assinatura, cod_cli, total_descontos, total_acrescimos, id_api ");
                sql.Append(" FROM rec_fatura ");
                sql.Append("where id = @codigo");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@codigo", idTabela);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                fatura = new FaturaDb
                                {
                                    Id = Convert.ToInt32(dr["id"]),
                                    Url = dr["url"].ToString(),
                                    Valor = Convert.ToDouble(dr["valor"]),
                                    FormaPagamento = dr["forma_pagamento"].ToString(),
                                    QuantParcelas = Convert.ToInt32(dr["quant_parcelas"]),
                                    Status = dr["status"].ToString(),
                                    DtCobranca = Convert.ToDateTime(dr["dt_cobranca"]),
                                    DtVencimento = Convert.ToDateTime(dr["dt_vencimento"]),
                                    DtCriacao = Convert.ToDateTime(dr["dt_criacao"]),
                                    IdAssinatura = Convert.ToInt32(dr["id_assinatura"]),
                                    CodCli = Convert.ToInt32(dr["cod_cli"]),
                                    TotalDescontos = Convert.ToDouble(dr["total_descontos"]),
                                    TotalAcrescimos = Convert.ToDouble(dr["total_acrescimos"]),
                                    IdApi = dr["id_api"].ToString()
                                };

                                if (fatura.DtCancelamento != null)
                                {
                                    fatura.DtCancelamento = Convert.ToDateTime(dr["dt_cancelamento"]);
                                }
                            }
                        }
                    }
                }

                return fatura;

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

        public FaturaDb GetFaturaDbByIdApi(string idApi)
        {
            const string metodo = "GetFaturaDbByIdApi";

            try
            {
                FaturaDb fatura = null;
                var sql = new StringBuilder();

                sql.Append(" SELECT id, url, valor, forma_pagamento, quant_parcelas, status, dt_cobranca, dt_vencimento, ");
                sql.Append(" dt_criacao, dt_cancelamento, id_assinatura, cod_cli, total_descontos, total_acrescimos, id_api ");
                sql.Append(" FROM rec_fatura ");
                sql.Append("where id_api = @id_api");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id_api", idApi);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                fatura = new FaturaDb
                                {
                                    Id = Convert.ToInt32(dr["id"]),
                                    Url = dr["url"].ToString(),
                                    Valor = Convert.ToDouble(dr["valor"]),
                                    FormaPagamento = dr["forma_pagamento"].ToString(),
                                    QuantParcelas = Convert.ToInt32(dr["quant_parcelas"]),
                                    Status = dr["status"].ToString(),
                                    DtCobranca = Convert.ToDateTime(dr["dt_cobranca"]),
                                    DtVencimento = Convert.ToDateTime(dr["dt_vencimento"]),
                                    DtCriacao = Convert.ToDateTime(dr["dt_criacao"]),
                                    IdAssinatura = Convert.ToInt32(dr["id_assinatura"]),
                                    CodCli = Convert.ToInt32(dr["cod_cli"]),
                                    TotalDescontos = Convert.ToDouble(dr["total_descontos"]),
                                    TotalAcrescimos = Convert.ToDouble(dr["total_acrescimos"]),
                                    IdApi = dr["id_api"].ToString()
                                };

                                if (fatura.DtCancelamento != null)
                                {
                                    fatura.DtCancelamento = Convert.ToDateTime(dr["dt_cancelamento"]);
                                }
                            }
                        }
                    }
                }

                return fatura;

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

        public void RegistraFatura(FaturaDb faturaDb)
        {
            var faturaExistente = this.GetFaturaDbByIdApi(faturaDb.IdApi);

            if (faturaExistente == null)
            {
                this.CriaNoDb(faturaDb);
            }
            else
            {
                faturaDb.Id = faturaExistente.Id;
                this.AtualizaNoDb(faturaDb);
            }
        }

        private void CriaNoDb(FaturaDb fatura)
        {
            const string metodo = "CriaFaturaNoDb";

            try
            {
                var sql = new StringBuilder();

                sql.Append(" INSERT INTO rec_fatura ( url, valor, forma_pagamento, quant_parcelas, status, dt_cobranca, dt_vencimento, ");
                sql.Append(" dt_criacao, dt_cancelamento, id_assinatura, cod_cli, total_descontos, total_acrescimos, id_api ) VALUES (");
                sql.Append(" @url, @valor, @forma_pagamento, @quant_parcelas, @status, @dt_cobranca, @dt_vencimento, ");
                sql.Append(" @dt_criacao, @dt_cancelamento, @id_assinatura, @cod_cli, @total_descontos, @total_acrescimos, @id_api ) ");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@url", fatura.Url);
                        cmd.Parameters.AddWithValue("@valor", fatura.Valor);
                        cmd.Parameters.AddWithValue("@forma_pagamento", fatura.FormaPagamento);
                        cmd.Parameters.AddWithValue("@quant_parcelas", fatura.QuantParcelas);
                        cmd.Parameters.AddWithValue("@status", fatura.Status);
                        cmd.Parameters.AddWithValue("@dt_cobranca", fatura.DtCobranca);
                        cmd.Parameters.AddWithValue("@dt_vencimento", fatura.DtVencimento);
                        cmd.Parameters.AddWithValue("@dt_criacao", fatura.DtCriacao);
                        if (fatura.DtCancelamento != null)
                        {
                            cmd.Parameters.AddWithValue("@dt_cancelamento", fatura.DtCancelamento);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dt_cancelamento", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@id_assinatura", fatura.IdAssinatura);
                        cmd.Parameters.AddWithValue("@cod_cli", fatura.CodCli);
                        cmd.Parameters.AddWithValue("@total_descontos", fatura.TotalDescontos);
                        cmd.Parameters.AddWithValue("@total_acrescimos", fatura.TotalAcrescimos);
                        cmd.Parameters.AddWithValue("@id_api", fatura.IdApi);
                        cmd.ExecuteNonQuery();

                    }
                }
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

        private void AtualizaNoDb(FaturaDb fatura)
        {
            const string metodo = "AtualizaFaturnaNoDb";

            try
            {
                var sql = new StringBuilder();

                sql.Append("UPDATE rec_fatura SET url=@url, valor=@valor, forma_pagamento=@forma_pagamento, quant_parcelas=@quant_parcelas, ");
                sql.Append(" status=@status, dt_cobranca=@dt_cobranca, dt_vencimento=@dt_vencimento, dt_criacao=@dt_criacao, dt_cancelamento=@dt_cancelamento, ");
                sql.Append(" id_assinatura=@id_assinatura, cod_cli=@cod_cli, total_descontos=@total_descontos, total_acrescimos=@total_acrescimos, id_api=@id_api ");
                sql.Append(" WHERE id=@id ");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@url", fatura.Url);
                        cmd.Parameters.AddWithValue("@valor", fatura.Valor);
                        cmd.Parameters.AddWithValue("@forma_pagamento", fatura.FormaPagamento);
                        cmd.Parameters.AddWithValue("@quant_parcelas", fatura.QuantParcelas);
                        cmd.Parameters.AddWithValue("@status", fatura.Status);
                        cmd.Parameters.AddWithValue("@dt_cobranca", fatura.DtCobranca);
                        cmd.Parameters.AddWithValue("@dt_vencimento", fatura.DtVencimento);
                        cmd.Parameters.AddWithValue("@dt_criacao", fatura.DtCriacao);
                        if (fatura.DtCancelamento != null)
                        {
                            cmd.Parameters.AddWithValue("@dt_cancelamento", fatura.DtCancelamento);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dt_cancelamento", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@id_assinatura", fatura.IdAssinatura);
                        cmd.Parameters.AddWithValue("@cod_cli", fatura.CodCli);
                        cmd.Parameters.AddWithValue("@total_descontos", fatura.TotalDescontos);
                        cmd.Parameters.AddWithValue("@total_acrescimos", fatura.TotalAcrescimos);
                        cmd.Parameters.AddWithValue("@id_api", fatura.IdApi);
                        cmd.Parameters.AddWithValue("@id", fatura.Id);
                        cmd.ExecuteNonQuery();

                    }
                }
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


        public int FaturaCanceladaAtualizaStatus(GetInvoiceResponse _response, int _id)
        {
            const string metodo = "FaturaCanceladaAtualizaStatus";

            try
            {

                var sql = new StringBuilder();
                int rowsAffected;

                sql.Append("update rec_fatura set status=@status, dt_cancelamento=@dtCancelamento where id = @id");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", _id);
                        cmd.Parameters.AddWithValue("@dtCancelamento", _response.CanceledAt);
                        cmd.Parameters.AddWithValue("@status", _response.Status);
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
