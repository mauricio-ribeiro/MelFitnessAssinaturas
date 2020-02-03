using System.ComponentModel;

namespace MelFitnessAssinaturas.Enums
{
    public enum TipoEnum
    {
        [Description("CLIENTE")]
        Cl,
        [Description("MEIO DE PAGAMENTO")]
        Mp,
        [Description("FATURAMENTO")]
        Ft,
        [Description("PLANO DE PAGAMENTO")]
        Pl,
        [Description("OUTRAS OPERAÇÕES")]
        Oo,
        [Description("ASSINATURA")]
        As,
        [Description("ERRO SISTÊMICO")]
        Er
    }
}
