using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.Models;


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

        }
        

        private void btnTestarConexao_Click(object sender, EventArgs e)
        {

            var testeConnectionStringController = new TesteConnectionStringController();
            var connectionString = string.Empty;
            var dadosConfig = ObterDadosConfiguracao();
            
            // QUANDO FOR INTANCIA
            // Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
            if (dadosConfig.Porta == 0)
            {
                connectionString = $@"Server={dadosConfig.Servidor}" + ";" +
                                   "Database=" + dadosConfig.Banco + ";" +
                                   "User ID=" + dadosConfig.Usuario + ";" +
                                   $@"Password={dadosConfig.Senha}" +
                                   ";";
            }
            else
            {
                // SERVIDOR noRMAL
                // Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;     

                connectionString = $@"Server={dadosConfig.Servidor},{dadosConfig.Porta.ToString()}" + ";" +
                                   "Database=" + dadosConfig.Banco + ";" +
                                   "User ID=" + dadosConfig.Usuario + ";" +
                                   $@"Password={dadosConfig.Senha}" +
                                   ";";
            }


            MessageBox.Show(testeConnectionStringController.TestConnectionString(connectionString)
                ? @"Conexão realizada com sucesso !!"
                : @"Falha na conexão.");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            var dadosConfig = ObterDadosConfiguracao();

            


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
                Porta = (int)nupPorta.Value,
                Usuario = txtUsuario.Text,
                Senha = txtSenha.Text,
                Banco = txtBanco.Text,
                TokenUserNameApi = txtTokenUserNameApi.Text
            };

            return config;

        }


        

    }
}
