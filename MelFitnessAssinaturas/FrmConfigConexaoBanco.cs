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
using MelFitnessAssinaturas.Util;

namespace MelFitnessAssinaturas
{
    public partial class FrmConfigConexaoBanco : Form
    {
        private string servidor { get; set; }
        private string instancia { get; set; }
        private string porta { get; set; }
        private string usuario { get; set; }
        private string senha { get; set; }
        private string banco { get; set; }

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
            txtInstancia.Text = ConfigIniUtil.Read("SERVIDOR", "instancia");
            nupPorta.Text = ConfigIniUtil.Read("SERVIDOR", "porta");
            txtUsuario.Text = ConfigIniUtil.Read("SERVIDOR", "usuario");
            txtSenha.Text = ConfigIniUtil.Read("SERVIDOR", "senha");
            txtBanco.Text = ConfigIniUtil.Read("SERVIDOR", "banco");
            txtTokenApi.Text = ConfigIniUtil.Read("MUNDIPAGG", "basicAuthUserName");
        }


        private void btnTestarConexao_Click(object sender, EventArgs e)
        {

            var testeConnectionStringController = new TesteConnectionStringController();

            servidor = txtServidor.Text;
            instancia = txtInstancia.Text;
            porta = nupPorta.Text;
            usuario = txtUsuario.Text;
            senha = txtSenha.Text;
            banco = txtBanco.Text;

            // QUANDO FOR INTANCIA
            // Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
            // SERVIDOR noRMAL
            // Server=myServerName,myPortNumber;Database=myDataBase;User Id=myUsername;Password=myPassword;

            var server = new StringBuilder();
            server.Append(servidor);

            if (string.IsNullOrEmpty(instancia))
            {
                server.Append(",");
                server.Append(porta);
            }
            else
            {
                server.Append(@"\");
                    server.Append(instancia);
            }

            var connectionString = $@"Server={server}" + ";" +
                                   "Database=" + banco + ";" +
                                   "User ID=" + usuario + ";" +
                                   $@"Password={senha}" +
                                   ";";

            MessageBox.Show(testeConnectionStringController.TestConnectionString(connectionString)
                ? @"Conexão realizada com sucesso !!"
                : @"Falha na conexão.");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            ConfigIniUtil.Write("SERVIDOR", "servidor", txtServidor.Text);
            ConfigIniUtil.Write("SERVIDOR", "instancia", txtInstancia.Text);
            ConfigIniUtil.Write("SERVIDOR", "porta", nupPorta.Text);
            ConfigIniUtil.Write("SERVIDOR", "usuario", txtUsuario.Text);
            ConfigIniUtil.Write("SERVIDOR", "senha", txtSenha.Text);
            ConfigIniUtil.Write("SERVIDOR", "banco", txtBanco.Text);
            ConfigIniUtil.Write("MUNDIPAGG", "basicAuthUserName", txtTokenApi.Text);

            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
