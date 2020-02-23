using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.Models;

namespace MelFitnessAssinaturas.Controllers
{
    public class UsuarioController
    {

        private UsuarioDal _usuarioDal;

        public UsuarioController()
        {
            _usuarioDal= new UsuarioDal();
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return _usuarioDal.ObterUsuarioPorId(id);
        }

    }
}
