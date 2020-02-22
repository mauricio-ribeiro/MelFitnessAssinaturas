using System.Collections.Generic;
using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class LogSyncController
    {
        private readonly LogSyncDal _logSyncDal;

        public LogSyncController()
        {
            _logSyncDal = new LogSyncDal();
        }
        
        public void Incluir(LogSync logApiMundipagg)
        {
            _logSyncDal.Incluir(logApiMundipagg);
        }

        public IEnumerable<LogSync> ObterTodos(params object[] parametros)
        {
            return _logSyncDal.ObterTodos(parametros);
        }



    }
}
