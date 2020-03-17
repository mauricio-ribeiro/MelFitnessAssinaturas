using System;

namespace MelFitnessAssinaturas.Models
{
    public class FaturaDb
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public double Valor { get; set; }
        public string FormaPagamento { get; set; }
        public int QuantParcelas { get; set; }
        public string Status { get; set; }
        public DateTime? DtCobranca { get; set; }
        public DateTime? DtVencimento { get; set; }
        public DateTime? DtCriacao { get; set; }
        public DateTime? DtCancelamento { get; set; }
        public int IdAssinatura { get; set; }
        public int CodCli { get; set; }
        public double? TotalDescontos { get; set; }
        public double? TotalAcrescimos { get; set; }
        public string IdApi { get; set; }
    }
}
