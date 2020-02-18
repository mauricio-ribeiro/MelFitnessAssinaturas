using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.Models
{
    public class AssinaturaDb
    {
        public int Id { get; set; }
        public DateTime Dt_Inicio { get; set; }
        public String Intervalo { get; set; }
        public int Intervalo_Quantidade { get; set; }
        public int Dia_Cobranca { get; set; }
        public int Quant_Parcelas { get; set; }
        public String Texto_Fatura { get; set; }
        public Double Valor_Minimo { get; set; }
        public String Status { get; set; }
        public String Id_Api { get; set; }

        public List<AssinaturaItemDb> ItensAssinatura { get; set; } = new List<AssinaturaItemDb>();
        public ClienteDb Cliente { get; set; }
        public MeioPagamentoDb MeioPagamento { get; set; }


    }
}
