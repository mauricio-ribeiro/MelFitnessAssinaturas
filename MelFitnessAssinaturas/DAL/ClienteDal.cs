using MelFitnessAssinaturas.InfraEstruturas;
using System.Data.SqlClient;
using MundiAPI.PCL.Models;
using System.Collections.Generic;
using MelFitnessAssinaturas.Models;
using System;
using System.Text;
using MelFitnessAssinaturas.Util;


namespace MelFitnessAssinaturas.DAL
{
    public class ClienteDal
    {

        private CreateCustomerRequest cliApi;
        private const string Camada = "ClienteDal";

        public ClienteDb GetClienteByIdApi(string idApi)
        {
            const string metodo = "GetClienteDb";

            try
            {
                ClienteDb cliente = null;
                var sql = new StringBuilder();

                sql.Append("select cli.cod_cli, cli.nome, cli.email, cli.cpf,");
                sql.Append("case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc,");
                sql.Append("cli.endereco as address_1, cli.bairro as address_2, cli.bairro, cli.cep, cid.cid_nome as cidade,");
                sql.Append("cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api from cad_clientes cli ");
                sql.Append("left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo ");
                sql.Append("where cli.id_api = @idApi");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@idApi", idApi);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                cliente = new ClienteDb
                                {
                                    Codigo = dr["cod_cli"].ToString(),
                                    Nome = dr["nome"].ToString(),
                                    Email = dr["email"].ToString(),
                                    Documento = dr["cpf"].ToString(),
                                    Sexo = dr["sexo"].ToString(),
                                    Dt_Nascimento = dr["data_nasc"].ToString(),
                                    Endereco_1 = dr["address_1"].ToString(),
                                    Endereco_2 = dr["bairro"].ToString(),
                                    Cep = dr["cep"].ToString(),
                                    Cidade = dr["cidade"].ToString(),
                                    Uf = dr["uf"].ToString(),
                                    Fone1 = dr["fone1"].ToString(),
                                    Fone2 = dr["fone2"].ToString(),
                                    Id_Api = dr["id_api"].ToString()
                                };

                            }
                        }
                    }
                }

                return cliente;

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

        public ClienteDb GetClienteDb(string codCli)
        {
            const string metodo = "GetClienteDb";

            try
            {
                ClienteDb cliente = null;
                var sql = new StringBuilder();

                sql.Append("select cli.cod_cli, cli.nome, cli.email, cli.cpf,");
                sql.Append("case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc,");
                sql.Append("cli.endereco as address_1, cli.bairro as address_2, cli.bairro, cli.cep, cid.cid_nome as cidade,");
                sql.Append("cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api from cad_clientes cli ");
                sql.Append("left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo ");
                sql.Append("where cli.cod_cli = @codCli");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@codCli",codCli);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                cliente = new ClienteDb
                                {
                                    Codigo = dr["cod_cli"].ToString(),
                                    Nome = dr["nome"].ToString(),
                                    Email = dr["email"].ToString(),
                                    Documento = dr["cpf"].ToString(),
                                    Sexo = dr["sexo"].ToString(),
                                    Dt_Nascimento = dr["data_nasc"].ToString(),
                                    Endereco_1 = dr["address_1"].ToString(),
                                    Endereco_2 = dr["bairro"].ToString(),
                                    Cep = dr["cep"].ToString(),
                                    Cidade = dr["cidade"].ToString(),
                                    Uf = dr["uf"].ToString(),
                                    Fone1 = dr["fone1"].ToString(),
                                    Fone2 = dr["fone2"].ToString(),
                                    Id_Api = dr["id_api"].ToString()                                   
                                };

                            }
                        }
                    }
                }

                return cliente;

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

        public string GetClienteByAssinatura(string _id)
        {
            const string metodo = "GetClienteByAssinatura";

            try
            {
                var sql = new StringBuilder();
                string idCliente = "";

                sql.Append(" select id_cliente from rec_assinatura where id = @_id");

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@_id", _id);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                idCliente = dr["id_cliente"].ToString();
                            }
                        }
                    }
                }

                if (String.IsNullOrEmpty(idCliente))
                {
                    throw new Exception("Cliente não encontrado");
                }

                return idCliente;
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

        public List<CreateCustomerRequest> ListaClientes(string _statusCli)
        {

            const string metodo = "ListaClientes";

            var listaCliApi = new List<CreateCustomerRequest>();

            try
            {

                var sql = new StringBuilder();

                sql.Append("select cli.cod_cli, cli.nome, cli.email, cli.cpf,");
                sql.Append("case when cli.sexo = 1 then 'male' else 'famale' end as sexo, cli.data_nasc,");
                sql.Append("cli.endereco as address_1, cli.bairro as address_2, cli.cep, cid.cid_nome as cidade,");
                sql.Append("cid.uf_sigla as uf, cli.fone1, cli.fone2, cli.id_api, cli.status_api from cad_clientes cli ");
                sql.Append("left join cad_cidade cid on cid.cid_codigo = cli.cid_codigo ");
                sql.Append("where cli.status_api = @StatusCli;");
                

                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@StatusCli",_statusCli);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var metadata = new Dictionary<string, string>
                                {
                                    {"id", dr["cod_cli"].ToString()},
                                    { "id_api", dr["id_api"].ToString()}
                                };

                                var address = new CreateAddressRequest
                                {
                                    Line1 = dr["address_1"].ToString(),
                                    Line2 = dr["address_2"].ToString(),
                                    ZipCode = dr["cep"].ToString(),
                                    City = dr["cidade"].ToString(),
                                    State = dr["uf"].ToString(),
                                    Country = "BR"
                                };

                                var phones = new CreatePhonesRequest
                                {
                                    HomePhone = new CreatePhoneRequest
                                    {
                                        AreaCode = dr["fone1"].ToString().Substring(0, 2),
                                        CountryCode = "55",
                                        Number = dr["fone1"].ToString().Substring(2, dr["fone1"].ToString().Length - 3)
                                    },
                                    MobilePhone = new CreatePhoneRequest
                                    {
                                        AreaCode = dr["fone2"].ToString().Substring(0, 2),
                                        CountryCode = "55",
                                        Number = dr["fone2"].ToString().Substring(2, dr["fone2"].ToString().Length - 3)
                                    },
                                };


                                cliApi = new CreateCustomerRequest
                                {
                                    Name = dr["nome"].ToString(),
                                    Email = dr["email"].ToString(),
                                    Type = "individual",
                                    Document = dr["cpf"].ToString(),
                                    Gender = dr["sexo"].ToString(),
                                    Code = dr["cod_cli"].ToString(),
                                    Phones = phones,
                                    Address = address,
                                    Metadata = metadata
                                };

                                listaCliApi.Add(cliApi);
                            }
                        }

                    }
                }
               
                return listaCliApi;

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

        public int ClienteGravado(string _code, string _id_api)
        {

            const string metodo = "ClienteGravado";


            try
            {

                int rowsAffected;
                var sql = new StringBuilder();

                sql.Append("update cad_clientes set id_api = @_id_api ");
                sql.Append("where cod_cli = @codigo ");


                using (var conn = ConexaoBd.GetConnection())
                {
                    using (var cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@codigo", _code);
                        cmd.Parameters.AddWithValue("@_id_api", _id_api);
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
