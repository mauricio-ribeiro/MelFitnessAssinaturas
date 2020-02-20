using System.Collections.Generic;
using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class LogApiMundipaggController
    {
        private readonly LogApiMundipaggDal _logApiMundipaggDal;

        public LogApiMundipaggController()
        {
            _logApiMundipaggDal = new LogApiMundipaggDal();
        }
        
        public void Incluir(LogApiMundipagg logApiMundipagg)
        {
            _logApiMundipaggDal.Incluir(logApiMundipagg);
        }

        public IEnumerable<LogApiMundipagg> ObterTodos(params object[] parametros)
        {
            return _logApiMundipaggDal.ObterTodos(parametros);
        }



    }
}
