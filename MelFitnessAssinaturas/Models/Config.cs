namespace MelFitnessAssinaturas.Models
{
    public class Config
    {

        public string Servidor { get; set; }
        public string Instancia { get; set; }
        public int Porta { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Banco { get; set; }
        public string TokenApi { get; set; }

        public Config()
        {
            
        }

    }
}
