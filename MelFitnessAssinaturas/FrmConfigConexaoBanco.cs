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
using Microsoft.SqlServer.Management.Smo;

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
            CarregaComboServidores();
            CarregaComboUsuarios();
        }
        

        private void btnTestarConexao_Click(object sender, EventArgs e)
        {

            var testeConnectionStringController = new TesteConnectionStringController();

            var servidor = (string)cbServidores.SelectedValue;
            var porta = nupPorta.Text;
            var usuario = cbUsuarios.SelectedText;
            var senha = txtSenha.Text;
            var banco = txtBanco.Text;


            var connectionString = $@"Server={servidor},{porta}" +
                                   ";Database=" + banco +
                                   "Integrated Security=true" + 
                                   ";User ID=" + usuario +
                                   $@";Password={senha};";

            MessageBox.Show(testeConnectionStringController.TestConnectionString(connectionString)
                ? @"Conexão realizada com sucesso !!"
                : @"Falha na conexão.");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CarregaComboServidores()
        {

            try
            {
                var dtInstancias = SmoApplication.EnumAvailableSqlServers();
                dtInstancias.TableName = "Instancias";

                cbServidores.ValueMember = "Name";
                cbServidores.DataSource = dtInstancias;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Erro: " + ex.Message,@"Pesquisa de Servidores",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }

        private void CarregaComboUsuarios()
        {
            foreach (var s in Properties.Settings.Default.Usuario)
            {
                cbUsuarios.Items.Add(s);
            }
        }







    }
}
