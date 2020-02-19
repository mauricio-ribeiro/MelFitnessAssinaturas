using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelFitnessAssinaturas.Models
{
    public class AssinaturaItemDb
    {
        public int Id_Assinatura { get; set; }
        public string Descricao { get; set; }
        public int Ciclos { get; set; }
        public int Quant { get; set; }
        public string Status { get; set; }
        public double Valor { private get; set; }

        public int? GetValor()
        {
            return Convert.ToInt32(this.Valor * 100);
        }
    }
}
