using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.Models
{
    public class MeioPagamentoDb
    {
        public int Id { get; set; }
        public int Cod_Cli { get; set; }
        public String Numero_Cartao { get; set; }
        public String Bandeira { get; set; }
        public String Nome_Cartao { get; set; }
        public String Cpf { get; set; }
        public String Cvc { get; set; }
        public int Val_Mes { get; set; }
        public int Val_Ano { get; set; }
        public String Status { get; set; }
        public String Id_Api { get; set; }

    }
}
