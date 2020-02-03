using MelFitnessAssinaturas.Enums;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Interfaces;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.DAL
{
    public class LoApiMundipaggDal : ILogApiMundipagg<LogApiMundipagg>
    {

        private const string Camada = "LoApiMundipaggDal";

        public void Incluir(LogApiMundipagg entidade)
        {

            const string Metodo = "Incluir";
            

            try
            {

                var sql = new StringBuilder();

                sql.Append("INSERT INTO log_apimundipagg (dt_evento,tipo,descricao,cod_cliente,id_api,valor,dt_documento) VALUES ");
                sql.Append("(@dt_evento,@tipo,@descricao,@cod_cliente,@id_api,@valor,@dt_documento);");
                
                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@dt_evento",entidade.DtEvento);
                        cmd.Parameters.AddWithValue("@tipo",Enum.GetName(typeof(TipoEnum),entidade.Tipo));
                        cmd.Parameters.AddWithValue("@descricao", entidade.Descricao);
                        cmd.Parameters.AddWithValue("@cod_cliente", entidade.CodCliente);
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
    }
}
