using System.Collections.Generic;

namespace MelFitnessAssinaturas.Interfaces
{
    public interface IRepositorio<T> where T : class
    {

        void Incluir(T intity);
        void Alterar(T entity);
        IEnumerable<T> ObterTodos();

    }
}
