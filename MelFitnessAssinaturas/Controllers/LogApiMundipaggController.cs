using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Interfaces;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class LogApiMundipaggController
    {

        private readonly ILogApiMundipagg<LogApiMundipagg> _logApiMundipagg;

        public LogApiMundipaggController()
        {
            _logApiMundipagg = new LoApiMundipaggDal();
        }
        
        public void Incluir(LogApiMundipagg logApiMundipagg)
        {
            _logApiMundipagg.Incluir(logApiMundipagg);
        }

    }
}
