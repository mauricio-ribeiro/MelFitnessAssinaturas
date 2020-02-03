using MelFitnessAssinaturas.Enums;
using System;

namespace MelFitnessAssinaturas.Models
{
    public class LogApiMundipagg
    {

        public DateTime DtEvento { get; set; }

        public TipoEnum Tipo { get; set; }

        public string Descricao { get; set; }

        public string CodCliente { get; set; }

        public string IdApi { get; set; }

        public decimal Valor { get; set; }

        public DateTime DtDocumento { get; set; }

    }
}
