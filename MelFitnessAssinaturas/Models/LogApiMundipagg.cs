using MelFitnessAssinaturas.Enums;
using System;
using System.Collections.Generic;
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas.Models
{
    public class LogApiMundipagg
    {

        public DateTime DtEvento { get; set; }

        public TipoEnum Tipo { get; set; }

        public string DescricaoTipo => EnumHelpersUtil.GetDescriptionValue(Tipo);

        public string Descricao { get; set; }

        public string NomeCliente { get; set; }

        public string IdApi { get; set; }

        public decimal Valor { get; set; }

        public DateTime DtDocumento { get; set; }


        public LogApiMundipagg()
        {
            
        }



    }
}
