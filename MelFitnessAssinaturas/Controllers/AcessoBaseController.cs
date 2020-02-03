namespace MelFitnessAssinaturas.Controllers
{
    public abstract class AcessoBaseController
    {

        protected AcessoBaseController ProximaVerificacao;

        public abstract void ProcessaVerificacao();

        public void SetProximaVerificacao(AcessoBaseController proximaVerificacao)
        {
            ProximaVerificacao = proximaVerificacao;
        }


    }
}
