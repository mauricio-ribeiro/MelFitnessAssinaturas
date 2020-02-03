using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.Interfaces
{
    public interface ILogApiMundipagg<T> where T : class
    {
        void Incluir(T entidade);
    }
}
