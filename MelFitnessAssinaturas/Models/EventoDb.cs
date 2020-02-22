using System;

namespace MelFitnessAssinaturas.Models
{
    public class EventoDb
    {
        public DateTime DtEvento { get; set; }
        public string Processado { get; set; }
        public string Sigla { get; set; }
        public string IdTabela { get; set; }
        public string CodAux1 { get; set; }
        public string CodAux2 { get; set; }
    }
}
