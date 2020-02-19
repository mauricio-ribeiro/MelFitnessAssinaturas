using System;
using System.Data.SqlClient;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.DAL
{
    public class MeioPagamentoDAL : BaseDAL
    {
        public MeioPagamentoDb GetCartaoDb(string id)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("select c.cod_cli, c.numero_cartao, c.bandeira, c.cpf, " +
                     " c.cvc, c.val_mes, c.val_ano, c.status, c.id, c.id_api " +
                     " from cad_clientes_cartao c " +
                     " where cli.cod_cli = @codCartao", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@codCartao";
                param.Value = id;

                cmd.Parameters.Add(param);

                reader = cmd.ExecuteReader();

                var cartao = new MeioPagamentoDb();


                cartao.Cod_Cli = reader.GetInt32(reader.GetOrdinal("cod_cli"));
                cartao.Numero_Cartao = reader["numero_cartao"].ToString();
                cartao.Bandeira = reader["bandeira"].ToString();
                cartao.Cpf = reader["cpf"].ToString();
                cartao.Cvc = reader["cvc"].ToString();
                cartao.Val_Mes = reader.GetInt32(reader.GetOrdinal("val_mes"));
                cartao.Val_Ano = reader.GetInt32(reader.GetOrdinal("val_ano"));
                cartao.Status = reader["status"].ToString();
                cartao.Id = reader.GetInt32(reader.GetOrdinal("id"));
                cartao.Id_Api = reader["id_api"].ToString();


                return cartao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
