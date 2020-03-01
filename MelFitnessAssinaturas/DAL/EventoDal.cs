using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.DAL
{
    public class EventoDal
    {

        private const string Camada = "EventoDal";

        public List<EventoDb> ListaEventosNaoProcessados()
        {
            const string metodo = "ListaEventosNaoProcessados";

            var listaEventos = new List<EventoDb>();

            try
            {
                EventoDb evento = null;
                var sql = new StringBuilder();

                sql.Append("select e.dt_evento, e.processado, e.sigla, ");
                sql.Append(" e.id_tabela, e.codaux_1, e.codaux_2, e.id_guid ");
                sql.Append(" from util_eventos_mundipagg e where e.processado = 'N' ");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using( var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        using( var dr = cmd.ExecuteReader())
                        {
                            while ( dr.Read())
                            {
                                evento = new EventoDb
                                {
                                    DtEvento = Convert.ToDateTime(dr["dt_evento"].ToString()),
                                    Processado = dr["processado"].ToString(),
                                    Sigla = dr["sigla"].ToString(),
                                    IdTabela = dr["id_tabela"].ToString(),
                                    CodAux1 = dr["codaux_1"].ToString(),
                                    CodAux2 = dr["codaux_2"].ToString(),
                                    Id_Guid = dr["id_guid"].ToString()
                                };

                                listaEventos.Add(evento);
                            }
                        }
                    }
                }
                return listaEventos;
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

        public int MarcaRegistroProcessadoComo(string _status, string _guid)
        {

            const string metodo = "MarcaRegistroProcessadoComo";


            try
            {

                if (!"N;P;E".Contains(_status))
                {
                    throw new Exception(String.Format("Status {0} inválido para o evento {1}",_status, _guid));
                }

                int rowsAffected;
                var sql = new StringBuilder();

                sql.Append("update util_eventos_mundipagg set processado = @status ");
                sql.Append("where id_guid = @id_guid ");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@status", _status);
                        cmd.Parameters.AddWithValue("@id_guid", _guid);
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
