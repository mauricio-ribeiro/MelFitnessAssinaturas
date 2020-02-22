using MelFitnessAssinaturas.Enums;
using MelFitnessAssinaturas.Util;
using System;

namespace MelFitnessAssinaturas.Models
{
    public class LogSync
    {

        public DateTime DtEvento { get; set; }

        public TipoLogEnum Tipo { get; set; }

        public string DescricaoTipo => EnumHelpersUtil.GetDescriptionValue(Tipo);

        public string Descricao { get; set; }

        public string NomeCliente { get; set; }

        public string IdApi { get; set; }

        public decimal Valor { get; set; }

        public DateTime DtDocumento { get; set; }


        public LogSync()
        {
            
        }



    }
}
