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
    }
}
