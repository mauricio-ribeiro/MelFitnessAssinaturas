using MelFitnessAssinaturas.InfraEstruturas;
using System.Data.SqlClient;
using MundiAPI.PCL.Models;
using System.Collections.Generic;
using MelFitnessAssinaturas.Models;
using System;


namespace MelFitnessAssinaturas.DAL
{
    public class ClienteDal : BaseDal
    {
        private CreateCustomerRequest cliApi;

        public ClienteDb GetClienteDb(string codCli)
        {
            var cmd = new SqlCommand("select cli.cod_cli, cli.nome, cli.email, cli.cpf, " +
                                     " case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc, " +
                                     " cli.endereco as address_1, cli.bairro as address_2, cli.cep, cid.cid_nome as cidade, " +
                                     " cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api, cli.status_api " +
                                     " from cad_clientes cli " +
                                     " left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo " +
                                     " where cli.cod_cli = @codCli", Conn);

            var param = new SqlParameter
            {
                ParameterName = "@codCli",
                Value = Reader["cod_cli"].ToString()
            };

            cmd.Parameters.Add(param);

            Reader = cmd.ExecuteReader();

            var cliente = new ClienteDb
            {
                Codigo = Reader["cod_cli"].ToString(),
                Nome = Reader["nome"].ToString(),
                Email = Reader["email"].ToString(),
                Documento = Reader["cpf"].ToString(),
                Sexo = Reader["sexo"].ToString(),
                Dt_Nascimento = Reader["data_nasc"].ToString(),
                Endereco_1 = Reader["address_1"].ToString(),
                Endereco_2 = Reader["bairro"].ToString(),
                Cep = Reader["cep"].ToString(),
                Cidade = Reader["cidade"].ToString(),
                Uf = Reader["uf"].ToString(),
                Fone1 = Reader["fone1"].ToString(),
                Fone2 = Reader["fone2"].ToString(),
                Id_Api = Reader["id_api"].ToString(),
                Status_Api = Reader["status_api"].ToString()
            };


            return cliente;
        }

        public List<CreateCustomerRequest> ListaClientes(string _statusCli)
        {
            try
            {
                Conn = ConexaoBd.GetConnection();

                var listaCliApi = new List<CreateCustomerRequest>();

                var cmd = new SqlCommand("select cli.cod_cli, cli.nome, cli.email, cli.cpf, " +
                                         " case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc, " +
                                         " cli.endereco as address_1, cli.bairro as address_2, cli.cep, cid.cid_nome as cidade, " +
                                         " cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api, cli.status_api " +
                                         " from cad_clientes cli " +
                                         " left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo " +
                                         " where cli.status_api = @StatusCli ", Conn);

                var param = new SqlParameter
                {
                    ParameterName = "@StatusCli",
                    Value = _statusCli
                };

                cmd.Parameters.Add(param);

                Reader = cmd.ExecuteReader();


                while (Reader.Read())
                {
                    var metadata = new Dictionary<string, string>();
                    metadata.Add("id", Reader["cod_cli"].ToString());
                    metadata.Add("id_api", Reader["id_api"].ToString());

                    var address = new CreateAddressRequest
                    {
                        Line1 = Reader["address_1"].ToString(),
                        Line2 = Reader["address_2"].ToString(),
                        ZipCode = Reader["cep"].ToString(),
                        City = Reader["cidade"].ToString(),
                        State = Reader["uf"].ToString(),
                        Country = "BR"
                    };

                    var phones = new CreatePhonesRequest
                    {
                        HomePhone = new CreatePhoneRequest
                        {
                            AreaCode = Reader["fone1"].ToString().Substring(0, 2),
                            CountryCode = "55",
                            Number = Reader["fone1"].ToString().Substring(2, Reader["fone1"].ToString().Length - 3)
                        },
                        MobilePhone = new CreatePhoneRequest
                        {
                            AreaCode = Reader["fone2"].ToString().Substring(0, 2),
                            CountryCode = "55",
                            Number = Reader["fone2"].ToString().Substring(2, Reader["fone2"].ToString().Length - 3)
                        },
                    };


                    cliApi = new CreateCustomerRequest
                    {
                        Name = Reader["nome"].ToString(),
                        Email = Reader["email"].ToString(),
                        Type = "individual",
                        Document = Reader["cpf"].ToString(),
                        Gender = Reader["sexo"].ToString(),
                        Code = Reader["cod_cli"].ToString(),
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

        public int ClienteGravado(string _code, string _id_api)
        {
            try
            {
                Conn = ConexaoBd.GetConnection();

                SqlCommand cmd = new SqlCommand("update cad_clientes " +
                " set status_api = 'F', id_api = @_id_api " +
                " where cod_cli = @codigo ", Conn);

                SqlParameter param1 = new SqlParameter
                {
                    ParameterName = "@codigo",
                    Value = _code
                };

                cmd.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter
                {
                    ParameterName = "@_id_api",
                    Value = _id_api
                };

                cmd.Parameters.Add(param2);

                var rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                if (Conn != null)
                {
                    Conn.Close();
                }
            }
        }

    }

}
