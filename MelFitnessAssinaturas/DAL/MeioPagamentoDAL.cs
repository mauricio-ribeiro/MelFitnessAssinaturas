using System;
using System.Data.SqlClient;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.DAL
{
    public class MeioPagamentoDal : BaseDal
    {
        public MeioPagamentoDb GetCartaoDb(string id)
        {
            try
            {

                var cmd = new SqlCommand("select c.cod_cli, c.numero_cartao, c.bandeira, c.cpf, " +
                                         " c.cvc, c.val_mes, c.val_ano, c.status, c.id, c.id_api " +
                                         " from cad_clientes_cartao c " +
                                         " where cli.cod_cli = @codCartao", Conn);

                var param = new SqlParameter
                {
                    ParameterName = "@codCartao",
                    Value = id
                };

                cmd.Parameters.Add(param);

                Reader = cmd.ExecuteReader();

                var cartao = new MeioPagamentoDb
                {
                    Cod_Cli = Reader.GetInt32(Reader.GetOrdinal("cod_cli")),
                    Numero_Cartao = Reader["numero_cartao"].ToString(),
                    Bandeira = Reader["bandeira"].ToString(),
                    Cpf = Reader["cpf"].ToString(),
                    Cvc = Reader["cvc"].ToString(),
                    Val_Mes = Reader.GetInt32(Reader.GetOrdinal("val_mes")),
                    Val_Ano = Reader.GetInt32(Reader.GetOrdinal("val_ano")),
                    Status = Reader["status"].ToString(),
                    Id = Reader.GetInt32(Reader.GetOrdinal("id")),
                    Id_Api = Reader["id_api"].ToString()
                };

                return cartao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (Reader != null)
                {
                    Reader.Close();
                }

                if (Conn != null)
                {
                    Conn.Close();
                }
            }
        }
    }
}
