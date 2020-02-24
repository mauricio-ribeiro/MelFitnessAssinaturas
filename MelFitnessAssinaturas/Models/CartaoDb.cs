using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.Models
{
    public class CartaoDb
    {
        public int Id { get; set; }
        public int Cod_Cli { get; set; }
        public string Numero_Cartao { get; set; }
        public string Bandeira { get; set; }
        public string Nome_Cartao { get; set; }
        public string Cpf { get; set; }
        public string Cvc { get; set; }
        public int Val_Mes { get; set; }
        public int Val_Ano { get; set; }
        public string Status { get; set; }
        public string Id_Api { get; set; }

    }
}
