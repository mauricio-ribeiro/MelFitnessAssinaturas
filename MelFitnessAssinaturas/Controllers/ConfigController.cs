using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas.Controllers
{
    public class ConfigController
    {

        public ConfigController()
        {
            
        }
        public void SalvarDadosConfiguracao(Config config)
        {
            // Sessão SERVIDOR
            ConfigIniUtil.Write("SERVIDOR", "servidor", config.Servidor);
            ConfigIniUtil.Write("SERVIDOR", "banco", config.Banco);
            ConfigIniUtil.Write("SERVIDOR", "porta", config.Porta.ToString());
            ConfigIniUtil.Write("SERVIDOR", "senha", config.Senha);

            // Sessão MUNDIPAGG
            ConfigIniUtil.Write("MUNDIPAGG", "basicAuthUserName", config.TokenApi);
        }



    }
}
