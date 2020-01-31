using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelFitnessAssinaturas.Interfaces;

namespace MelFitnessAssinaturas.Repositorios
{
    public class Repositorio<T> : IRepositorio<T> where  T : class
    {
        public void Incluir(T intity)
        {
            throw new NotImplementedException();
        }

        public void Alterar(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ObterTodos()
        {
            throw new NotImplementedException();
        }
    }
}
