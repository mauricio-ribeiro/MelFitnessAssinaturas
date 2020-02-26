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

            var testeConnectionStringController = new TesteConnectionStringController();
            var connectionString = string.Empty;
            var config = ObterDadosConfiguracao();

            // QUANDO FOR INTANCIA
            // Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
            if (config.Porta == 0)
            {
                connectionString = $@"Server={config.Servidor}" + ";" +
                                   "Database=" + config.Banco + ";" +
                                   "User ID=" + config.Usuario + ";" +
                                   $@"Password={config.Senha}";
                
                // QUANDO FOR INTANCIA
                // Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
                // SERVIDOR noRMAL
                // Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;

                var server = new StringBuilder();
                server.Append(config.Servidor);

                if (string.IsNullOrEmpty(config.Instancia))
                {
                    server.Append(",");
                    server.Append(config.Porta);
                }
                else
                {
                    server.Append(@"\");
                    server.Append(config.Instancia);
                }

                connectionString = $@"Server={server}" + ";" +
                                      "Database=" + config.Banco + ";" +
                                      "User ID=" + config.Usuario + ";" +
                                      $@"Password={config.Senha}" +
                                      ";";
            }
            else
            {
                // SERVIDOR noRMAL
                // Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;     

                connectionString = $@"Server={config.Servidor},{config.Porta.ToString()}" + ";" +
                                   "Database=" + config.Banco + ";" +
                                   "User ID=" + config.Usuario + ";" +
                                   $@"Password={config.Senha}" +
                                   ";";
            }


            MessageBox.Show(testeConnectionStringController.TestConnectionString(connectionString)
                ? @"Conexão realizada com sucesso !!"
                : @"Falha na conexão.");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            ConfigIniUtil.Write("SERVIDOR", "servidor", txtServidor.Text);
            ConfigIniUtil.Write("SERVIDOR", "banco", txtBanco.Text);
            ConfigIniUtil.Write("SERVIDOR", "instancia", txtInstancia.Text);
            ConfigIniUtil.Write("SERVIDOR", "porta", nupPorta.Value.ToString());
            ConfigIniUtil.Write("SERVIDOR", "usuario", txtUsuario.Text);
            ConfigIniUtil.Write("SERVIDOR", "senha", txtSenha.Text);
            ConfigIniUtil.Write("MUNDIPAGG", "basicAuthUserName", txtTokenApi.Text);

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
