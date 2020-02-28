namespace MelFitnessAssinaturas.Singletons
{
    public sealed class SistemaSingleton
    {

        private static SistemaSingleton _instancia;
        private static readonly object Padlock = new object();
        
        public string TokenApi;

        private SistemaSingleton() { }

        public static SistemaSingleton Instancia
        {
            get
            {
                lock (Padlock)
                {
                    return _instancia ?? (_instancia = new SistemaSingleton());
                }

            }

        }


    }
}
