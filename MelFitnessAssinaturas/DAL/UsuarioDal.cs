using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Data.SqlClient;
using System.Text;

namespace MelFitnessAssinaturas.DAL
{
    public class UsuarioDal
    {

        private const string Camada = "UsuarioDal";


        public Usuario ObterUsuarioPorId(int id)
        {

            const string metodo = "ObterUsuarioPorId";
            Usuario usuario = null;

            try
            {

                var sql = new StringBuilder();

                sql.Append("SELECT id,nome,senha FROM rec_usuario_api WHERE id = @id;");

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
                                usuario = new Usuario
                                {
                                    Id = Convert.ToInt32(dr["id"]),
                                    Nome = dr["nome"] == DBNull.Value ? string.Empty : Convert.ToString(dr["nome"]),
                                    Senha = Convert.ToString(dr["senha"])
                                };
                            }
                        }
                    }
                }

                return usuario;
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
