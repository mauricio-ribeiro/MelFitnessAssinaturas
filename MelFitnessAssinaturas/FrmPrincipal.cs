using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.Enums;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;


namespace MelFitnessAssinaturas
{
    public partial class FrmPrincipal : Form
    {

        private Timer _timer = new Timer();
        private bool _allowVisible;
        private bool _allowClose;
        

        public FrmPrincipal()
        {
            InitializeComponent();
            notifyIcon1.ContextMenu = new ContextMenu();
            notifyIcon1.ContextMenu.MenuItems.Add("Abrir", abrir_Click);
            notifyIcon1.ContextMenu.MenuItems.Add("Sair", sair_Click);
            //ExecutarTarefas();
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!_allowVisible)
            {
                value = false;
                if (!IsHandleCreated) CreateHandle();
            }

            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose)
            {
                Hide();
                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
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


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dgvDadosLog.AutoGenerateColumns = false;
                CarregaComboTipo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data["MensagemCustomizada"] + Environment.NewLine + Environment.NewLine +
                                @"Mensagem original: " + ex.Message, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void abrir_Click(object sender, EventArgs e)
        {
            var frmLogin = new FrmLogin();
            frmLogin.ShowDialog();

            if (frmLogin.entrarOk)
            {
                _allowVisible = true;
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
            }
            
        }

        private void sair_Click(object sender, EventArgs e)
        {
            var frmLogin = new FrmLogin();
            frmLogin.ShowDialog();

            if (frmLogin.sairOk)
            {
                _allowClose = true;
                Application.Exit();
            }

        }


        
        private void CarregaComboTipo()
        {

            cbTipo.DisplayMember = "Description";
            cbTipo.ValueMember = "Value";
            cbTipo.DataSource = Enum.GetValues(typeof(TipoLogEnum))
                .Cast<TipoLogEnum>()
                .Where(x=>x != TipoLogEnum.Cl)
                .Select(value => new
                    {
                        (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                        value
                    }
                )
                .OrderBy(item => item.value)
                .ToList();

        }
        
        private void btnPesquisaLog_Click(object sender, EventArgs e)
        {

            var datasValida = RotinasDataUtil.CompareDatas(dtInicial.Value, dtFinal.Value);
            var logApiMundipaggController = new LogSyncController();

            object[] parametros = { txtNomeCliente.Text, dtInicial.Value, dtFinal.Value, (TipoLogEnum)cbTipo.SelectedValue };

            if (datasValida)
            {

                try
                {

                    var logApiMundipaggs = logApiMundipaggController.ObterTodos(parametros);
                    dgvDadosLog.DataSource = null;
                    dgvDadosLog.DataSource = logApiMundipaggs.ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Data["MensagemCustomizada"] + Environment.NewLine + Environment.NewLine +
                                    @"Mensagem original: " + ex.Message, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else
            {
                MessageBox.Show(@"A data final tem que ser maior ou igual a data inicial.",
                    @"Atenção !!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void dgvDadosLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {

                if (dgvDadosLog.Rows[e.RowIndex].DataBoundItem != null && (dgvDadosLog.Columns[e.ColumnIndex].DataPropertyName.Contains(".")))
                {
                    e.Value = BindProperty(dgvDadosLog.Rows[e.RowIndex].DataBoundItem, dgvDadosLog.Columns[e.ColumnIndex].DataPropertyName);
                }

            }
            catch
            { }
        }

        private string BindProperty(object property, string propertyName)
        {
            string retValue = "";

            if (propertyName.Contains("."))
            {
                PropertyInfo[] arrayProperties;
                string leftPropertyName;

                leftPropertyName = propertyName.Substring(0, propertyName.IndexOf("."));
                arrayProperties = property.GetType().GetProperties();

                foreach (PropertyInfo propertyInfo in arrayProperties)
                {
                    if (propertyInfo.Name == leftPropertyName)
                    {
                        retValue = BindProperty(
                            propertyInfo.GetValue(property, null),
                            propertyName.Substring(propertyName.IndexOf(".") + 1));
                        break;
                    }
                }
            }
            else
            {
                Type propertyType;
                PropertyInfo propertyInfo;

                propertyType = property.GetType();
                propertyInfo = propertyType.GetProperty(propertyName);
                retValue = propertyInfo.GetValue(property, null).ToString();
            }

            return retValue;
        }

        private void menuItemConfiguracao_Click(object sender, EventArgs e)
        {
            new FrmConfigConexaoBanco().ShowDialog();
        }

        private void FrmPrincipal_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
