using MelFitnessAssinaturas.Models;
using MundiAPI.PCL.Models;
using System;
using System.Collections.Generic;

namespace MelFitnessAssinaturas.DTO
{
    public static class ClienteDTO
    {
        public static UpdateCustomerRequest ConverteEditadoClienteDbEmApi(ClienteDb clienteDb)
        {
            try
            {
                var updateCustomer = new UpdateCustomerRequest
                {
                    Name = clienteDb.Nome,
                    Code = clienteDb.Codigo,
                    Document = clienteDb.Documento,
                    Email = clienteDb.Email,
                    Gender = clienteDb.Sexo,
                    Type = "individual",
                    Address = new CreateAddressRequest
                    {
                        Line1 = clienteDb.Endereco_1,
                        Line2 = clienteDb.Endereco_2,
                        ZipCode = clienteDb.Cep,
                        City = clienteDb.Cidade,
                        State = clienteDb.Uf,
                        Country = "BR"
                    },
                    Phones = new CreatePhonesRequest
                    {
                        HomePhone = new CreatePhoneRequest
                        {
                            AreaCode = clienteDb.Fone1.Substring(0, 2),
                            CountryCode = "55",
                            Number = clienteDb.Fone1.Substring(3, clienteDb.Fone1.Length - 3)
                        },
                        MobilePhone = new CreatePhoneRequest
                        {
                            AreaCode = clienteDb.Fone2.Substring(0, 2),
                            CountryCode = "55",
                            Number = clienteDb.Fone2.Substring(3, clienteDb.Fone2.Length - 3)
                        },
                    }
                };
                return updateCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static CreateCustomerRequest ConverteNovoClienteDbEmApi(ClienteDb clienteDb)
        {
            try
            {
                var metadata = new Dictionary<string, string>();
                metadata.Add("id", clienteDb.Codigo);

                var address = new CreateAddressRequest
                {
                    Line1 = clienteDb.Endereco_1,
                    Line2 = clienteDb.Endereco_2,
                    ZipCode = clienteDb.Cep,
                    City = clienteDb.Cidade,
                    State = clienteDb.Uf,
                    Country = "BR"
                };

                var phones = new CreatePhonesRequest
                {
                    HomePhone = new CreatePhoneRequest
                    {
                        AreaCode = clienteDb.Fone1.Substring(0,2),
                        CountryCode = "55",
                        Number = clienteDb.Fone1.Substring(3, clienteDb.Fone1.Length - 3)
                    },
                    MobilePhone = new CreatePhoneRequest
                    {
                        AreaCode = clienteDb.Fone2.Substring(0, 2),
                        CountryCode = "55",
                        Number = clienteDb.Fone2.Substring(3, clienteDb.Fone2.Length - 3)
                    },
                };

                var request = new CreateCustomerRequest
                {
                    Name = clienteDb.Nome,
                    Email = clienteDb.Email,
                    Type = "individual",
                    Document = clienteDb.Documento,
                    Gender = clienteDb.Sexo,
                    Code =clienteDb.Codigo,
                    Phones = phones,
                    Address = address,
                    Metadata = metadata
                };

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
