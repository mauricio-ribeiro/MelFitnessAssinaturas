using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Text;
using System.Windows.Forms;

namespace MelFitnessAssinaturas
{
    public partial class FrmConfigConexaoBanco : Form
    {
        
        public FrmConfigConexaoBanco()
        {
            InitializeComponent();
        }

        private void FrmConfigConexaoBanco_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }


        private void FrmConfigConexaoBanco_Load(object sender, EventArgs e)
        {
            txtServidor.Text = ConfigIniUtil.Read("SERVIDOR", "servidor");
            txtBanco.Text = ConfigIniUtil.Read("SERVIDOR", "banco");
            txtInstancia.Text = ConfigIniUtil.Read("SERVIDOR", "instancia");
            nupPorta.Text = ConfigIniUtil.Read("SERVIDOR", "porta");
            txtUsuario.Text = ConfigIniUtil.Read("SERVIDOR", "usuario");
            txtSenha.Text = ConfigIniUtil.Read("SERVIDOR", "senha");
            txtTokenApi.Text = ConfigIniUtil.Read("MUNDIPAGG", "basicAuthUserName");
        }


        private void btnTestarConexao_Click(object sender, EventArgs e)
        {

            var connectionStringController = new ConnectionStringController();
            var connectionString = string.Empty;
            var config = ObterDadosConfiguracao();

            connectionString = connectionStringController.MontaConnectionString(config);
            
            MessageBox.Show(connectionStringController.TestConnectionString(connectionString)
                ? @"Conexão realizada com sucesso !!"
                : @"Falha na conexão.");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            var config = ObterDadosConfiguracao();

            if (ValidacaoUtil.ValidarModelo(config))
            {
                ConfigIniUtil.Write("SERVIDOR", "servidor", config.Servidor);
                ConfigIniUtil.Write("SERVIDOR", "banco", config.Banco);
                ConfigIniUtil.Write("SERVIDOR", "instancia", config.Instancia);
                ConfigIniUtil.Write("SERVIDOR", "porta", config.Porta.ToString());
                ConfigIniUtil.Write("SERVIDOR", "usuario", config.Usuario);
                ConfigIniUtil.Write("SERVIDOR", "senha", config.Senha);
                ConfigIniUtil.Write("MUNDIPAGG", "basicAuthUserName", config.TokenApi);
            }
            
            Close();
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Config ObterDadosConfiguracao()
        {
            var config = new Config
            {
                Servidor = txtServidor.Text,
                Instancia = txtInstancia.Text,
                Porta = (int)nupPorta.Value,
                Usuario = txtUsuario.Text,
                Senha = txtSenha.Text,
                Banco = txtBanco.Text,
                TokenApi = txtTokenApi.Text
            };

            return config;

        }

    }
}
