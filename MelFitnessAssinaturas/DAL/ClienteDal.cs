using MelFitnessAssinaturas.InfraEstruturas;
using System.Data.SqlClient;
using MundiAPI.PCL.Models;
using System.Collections.Generic;
using System;

namespace MelFitnessAssinaturas.DAL
{
    public class ClienteDal
    {
        private SqlConnection conn = ConexaoBd.GetConnection();
        private SqlDataReader reader = null;
        private CreateCustomerRequest cliApi;

        public List<CreateCustomerRequest> ListaClientes(string _statusCli)
        {
            try
            {
                List<CreateCustomerRequest> listaCliApi = new List<CreateCustomerRequest>();

                SqlCommand cmd = new SqlCommand("select cli.cod_cli, cli.nome, cli.email, cli.cpf, " + 
                " case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc, " +
                " cli.endereco as address_1, cli.bairro as address_2, cli.cep, cid.cid_nome as cidade, " + 
                " cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api, cli.status_api " +
                " from cad_clientes cli " + 
                " left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo " + 
                " where cli.status_api = @StatusCli ", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@StatusCli";
                param.Value = _statusCli;

                cmd.Parameters.Add(param);

                reader = cmd.ExecuteReader();


                while(reader.Read())
                {
                        var metadata = new Dictionary<string, string>();
                        metadata.Add("id", reader["cod_cli"].ToString());

                        var address = new CreateAddressRequest
                        {
                            Line1 = reader["address_1"].ToString(),
                            Line2 = reader["address_2"].ToString(),
                            ZipCode = reader["cep"].ToString(),
                            City = reader["cidade"].ToString(),
                            State = reader["uf"].ToString(),
                            Country = "BR"
                        };

                        var phones = new CreatePhonesRequest
                        {
                            HomePhone = new CreatePhoneRequest
                            {
                                AreaCode = reader["fone1"].ToString().Substring(0, 2),
                                CountryCode = "55",
                                Number = reader["fone1"].ToString().Substring(2, reader["fone1"].ToString().Length - 3)
                            },
                            MobilePhone = new CreatePhoneRequest
                            {
                                AreaCode = reader["fone2"].ToString().Substring(0, 2),
                                CountryCode = "55",
                                Number = reader["fone2"].ToString().Substring(2, reader["fone2"].ToString().Length - 3)
                            },
                        };


                        cliApi = new CreateCustomerRequest
                        {
                            Name = reader["nome"].ToString(),
                            Email = reader["email"].ToString(),
                            Type = "individual",
                            Document = reader["cpf"].ToString(),
                            Gender = reader["sexo"].ToString(),
                            Code = reader["cod_cli"].ToString(),
                            Phones = phones,
                            Address = address,
                            Metadata = metadata
                        };
                    listaCliApi.Add(cliApi);
                    }
                return listaCliApi;

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

                if ( conn != null)
                {
                    conn.Close();
                }
            }
        }

        public int ClienteGravado(string _code)
        {            
            try
            {
                List<CreateCustomerRequest> listaCliApi = new List<CreateCustomerRequest>();

                SqlCommand cmd = new SqlCommand("update cad_clientes " +
                " set status_api = 'F' " +
                " where cod_cli = @codigo ", conn);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@codigo";
                param.Value = _code;

                cmd.Parameters.Add(param);

                var rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
