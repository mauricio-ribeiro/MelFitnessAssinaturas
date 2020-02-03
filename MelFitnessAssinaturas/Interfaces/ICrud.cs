using System.Collections.Generic;

namespace MelFitnessAssinaturas.Interfaces
{
    public interface ICrud<T> where T : class
    {
        void Incluir(T entidade);
        void Alterar(T entidade);
        void Excluir(T entidade);
        IEnumerable<T> ObterTodos();
    }
}
