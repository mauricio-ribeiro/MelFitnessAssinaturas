using System;
using System.Collections.Generic;

namespace MelFitnessAssinaturas.Models
{
    public class AssinaturaDb
    {
        public int Id { get; set; }
        public DateTime Dt_Inicio { get; set; }
        public string Intervalo { get; set; }
        public int Intervalo_Quantidade { get; set; }
        public int Dia_Cobranca { get; set; }
        public int Quant_Parcelas { get; set; }
        public string Texto_Fatura { get; set; }
        public double Valor_Minimo { get; set; }
        public string Status { get; set; }
        public string Id_Api { get; set; }

        public List<AssinaturaItemDb> ItensAssinatura { get; set; } = new List<AssinaturaItemDb>();
        public ClienteDb Cliente { get; set; } = new ClienteDb();
        public CartaoDb MeioPagamento { get; set; }


    }
}
