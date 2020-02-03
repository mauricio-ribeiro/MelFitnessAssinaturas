using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MelFitnessAssinaturas.Controllers;
using MelFitnessAssinaturas.Enums;
using MelFitnessAssinaturas.DAL;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using MelFitnessAssinaturas.Util;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas
{
    public partial class Form1 : Form
    {

        private Timer _timer = new Timer();
        private bool _allowVisible;
        private bool _allowClose;


        public Form1()
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
            dgvDadosLog.AutoGenerateColumns = false;
            CarregaComboTipo();

            _timer.Start();
            

            //var sql = new StringBuilder();
            //var ds = new DataSet();

            //sql.Append("SELECT * FROM cad_clientes;");

            //using (var conn = ConexaoBd.GetConnection())
            //{
            //    using (var cmd = new SqlCommand(sql.ToString(),conn))
            //    {
            //        using (var da = new SqlDataAdapter(cmd))
            //        {
            //            da.TableMappings.Add("Table","Clientes");
            //            da.Fill(ds);
            //        }
            //    }
            //}

            var logApiMundipaggController = new LogApiMundipaggController();

            var logApiMundipagg = new LogApiMundipagg
            {
                DtEvento = DateTime.Now,
                Tipo = TipoEnum.Cl,
                Descricao = "Novo cadastro de cliente",
                CodCliente = "12",
                IdApi = "xxxx",
                Valor = 120.00M,
                DtDocumento = DateTime.Now
            };

            try
            {

                logApiMundipaggController.Incluir(logApiMundipagg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data["MensagemCustomizada"] + Environment.NewLine + Environment.NewLine +
                                @"Mensagem original: " + ex.Message, @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void abrir_Click(object sender, EventArgs e)
        {
            _allowVisible = true;
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void sair_Click(object sender, EventArgs e)
        {
            _allowClose = true;
            Application.Exit();
        }


        private void ExecutarTarefas()
        {

            var interval = TimeSpan.FromSeconds(10);
            var timer = new System.Timers.Timer(interval.TotalMilliseconds) { AutoReset = false };

            timer.Elapsed += (sender, eventArgs) =>
            {
                var start = DateTime.Now;
                try
                {
                    Task task1 = Task.Factory.StartNew(() => Tarefa1());
                    Task task2 = Task.Factory.StartNew(() => Tarefa2());
                    Task.WaitAll(task1, task2);
                }
                finally
                {
                    var elapsed = DateTime.Now - start;
                    if (elapsed < interval)
                        timer.Interval = (interval - elapsed).TotalMilliseconds;
                    else
                        timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
                    timer.Start();
                }
            };

            timer.Start();

        }
        

        private async Task Tarefa1()
        {
            notifyIcon1.ShowBalloonTip(20, @"Atenção !!", @"Tarefa 1.", ToolTipIcon.Info);
        }

        private async Task Tarefa2()
        {
            notifyIcon1.ShowBalloonTip(20, @"Atenção !!", @"Tarefa 2.", ToolTipIcon.Info);
        }

        private void CarregaComboTipo()
        {

            cbTipo.DisplayMember = "Description";
            cbTipo.ValueMember = "Value";
            cbTipo.DataSource = Enum.GetValues(typeof(TipoEnum))
                .Cast<Enum>()
                .Select(value => new
                    {
                        (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                        value
                    }
                )
                .OrderBy(item => item.value)
                .ToList();

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Secret key fornecida pela Mundipagg
            string basicAuthUserName = ConfigIniUtil.Read("MUNDIPAGG", "basicAuthUserName");

            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            var clienteDAL = new ClienteDal();
            var listaClientes = clienteDAL.ListaClientes("N");

            foreach (var customer in listaClientes)
            {
                var response = client.Customers.CreateCustomer(customer);
                Console.WriteLine(String.Format("Cliente {0} registrado", response.Name));
                clienteDAL.ClienteGravado(response.Code, response.Id);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Secret key fornecida pela Mundipagg
            string basicAuthUserName = "sk_test_J3xVZJPUBTWOnj6v";
            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);
            string customerId = "cus_dVrMpGKURuao2qkA";

            var response = client.Customers.GetCustomer(customerId);

            Console.WriteLine(response.Name);

            //return View();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Secret key fornecida pela Mundipagg
            string basicAuthUserName = ConfigIniUtil.Read("MUNDIPAGG","basicAuthUserName");
            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            var response = client.Customers.GetCustomers();

            foreach (GetCustomerResponse item in response.Data) {
                Console.WriteLine(item.Name);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Secret key fornecida pela Mundipagg
            string basicAuthUserName = "sk_test_J3xVZJPUBTWOnj6v";
            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            string customerId = "cus_dVrMpGKURuao2qkA";

            var request = new CreateAccessTokenRequest
            {
                ExpiresIn = 30
            };

            var response = client.Customers.CreateAccessToken(customerId, request);

            Console.WriteLine(response.Code);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Secret key fornecida pela Mundipagg
            string basicAuthUserName = "sk_test_J3xVZJPUBTWOnj6v";
            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

            string customerId = "cus_dVrMpGKURuao2qkA";

            var response = client.Customers.GetAccessTokens(customerId);

            foreach (GetAccessTokenResponse item in response.Data)
            {
                Console.WriteLine($@"Id: {item.Id} -- {item.Code}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var clienteDAL = new ClienteDal();
            var listaClientes = clienteDAL.ListaClientes("A");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Scheduler tarefa = new Scheduler(tempo =>
           {
               Console.WriteLine("Rodando...");
           });

            tarefa.ID = "xptoTeste";
            tarefa.Frequencia = new TimeSpan(0, 0, 10);
            tarefa.StartWithDelay(null, new TimeSpan(0, 0, 10));
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



    }
}
