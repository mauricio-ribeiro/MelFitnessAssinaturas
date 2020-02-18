using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas.Models
{
    public class ClienteDb
    {

        public string Nome { get; }
        public string Email { get; }
        public string Tipo { get; }
        public string Documento { get; }
        public string Sexo { get; }
        public string Codigo { get; set; }
        public Dictionary<string,string> Metadata { get; }
        public CreateAddressRequest Endereco { get; }
        public CreatePhonesRequest Telefones { get; }
        public String id_Api { get; set; }



    }
}
