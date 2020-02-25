namespace MelFitnessAssinaturas.Models
{
    public class Usuario
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string SenhaApi { get; set; }
        public bool EhUsuario { get; set; }
        public bool Ativo { get; set; }
        public bool Admin { get; set; }

        public Usuario()
        { }

    }
}
