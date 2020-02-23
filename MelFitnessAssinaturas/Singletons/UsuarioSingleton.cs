namespace MelFitnessAssinaturas.Singletons
{
    public sealed class UsuarioSingleton
    {

        private static UsuarioSingleton _instancia;
        private static readonly object Padlock = new object();

        public int Id;
        public string Nome;
        public string Senha;

        private UsuarioSingleton()
        { }
        
        public static UsuarioSingleton Instancia
        {
            get
            {
                lock (Padlock)
                {
                    return _instancia ?? (_instancia = new UsuarioSingleton());
                }

            }

        }

    }
}
