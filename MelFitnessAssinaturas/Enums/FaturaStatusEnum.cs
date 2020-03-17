using System.ComponentModel;

namespace MelFitnessAssinaturas.Enums
{
    enum FaturaStatusEnum
    {
        [Description("PENDING")]
        pending,
        [Description("PAID")]
        paid,
        [Description("CANCELED")]
        canceled,
        [Description("SCHEDULED")]
        scheduled,
        [Description("FAILED")]
        failed
    }
}
