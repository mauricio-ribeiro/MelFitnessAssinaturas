using CryptoDll;
using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using System;
using System.Windows.Forms;
using MelFitnessAssinaturas.Singletons;

namespace MelFitnessAssinaturas
{
    public partial class FrmLogin : Form
    {

        public bool entrarOk;
        public bool sairOk;
        private Usuario _usuario;

        public FrmLogin()
        {
            InitializeComponent();
        }
        
        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var data_hora = DateTime.Now;
            tssLabelData.Text = data_hora.ToLongDateString();
        }

        private void txtCodUsuario_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodUsuario.Text))
            {
                LimparCampos();
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (SenhaValida())
            {
                Close();
            }
            else
            {
                MessageBox.Show(@"Senha incorreta",@"Atenção!!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (SenhaValida())
            {
                Close();
            }
            else
            {
                MessageBox.Show(@"Senha incorreta", @"Atenção!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void PreencherDadosDoUsuario()
        {
            UsuarioSingleton.Instancia.Id = _usuario.Id;
            UsuarioSingleton.Instancia.Nome = _usuario.Nome;
            UsuarioSingleton.Instancia.SenhaApi = _usuario.SenhaApi;
            UsuarioSingleton.Instancia.EhUsuario = _usuario.EhUsuario;
            UsuarioSingleton.Instancia.Ativo = _usuario.Ativo;
            UsuarioSingleton.Instancia.Admin = _usuario.Admin;

            txtUsuario.Text = _usuario.Nome;

        }

        private bool SenhaValida()
        {
            bool senhaValida = false;

            if (!string.IsNullOrEmpty(txtSenha.Text))
            {
                var secureString = StringUtil.ConvertToSecureString(Crypto.Cifra(txtSenha.Text));

                if (StringUtil.ConvertToSecureString(UsuarioSingleton.Instancia.SenhaApi).CompareSecuryString(secureString))
                {
                    senhaValida = true;
                    entrarOk = true;
                    sairOk = true;
                }

            }

            return senhaValida;

        }


        private void LimparCampos()
        {
            txtCodUsuario.ResetText();
            txtUsuario.ResetText();
            txtSenha.ResetText();
        }

        private void txtCodUsuario_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
            if (e.KeyData == Keys.Enter || e.KeyData == Keys.Tab)
            {
                int id;
                int.TryParse(txtCodUsuario.Text, out id);

                if (id > 0)
                {

                    var usuarioController = new UsuarioController();

                    try
                    {

                        _usuario = usuarioController.ObterUsuarioPorId(id);

                        if (_usuario != null && _usuario.Id > 0)
                        {
                            PreencherDadosDoUsuario();
                        }
                        else
                        {
                            MessageBox.Show(@"Usuário não encontrado!", @"Usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Data["MensagemCustomizada"] + Environment.NewLine + Environment.NewLine +
                                        @"Mensagem original: " + ex.Message, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void txtCodUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
