﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MelFitnessAssinaturas.InfraEstruturas;
using MelFitnessAssinaturas.Models;
using Microsoft.SqlServer.Server;
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
            _timer.Start();


            var sql = new StringBuilder();
            var ds = new DataSet();

            sql.Append("SELECT * FROM cad_clientes;");

            using (var conn = ConexaoBd.GetConnection())
            {
                using (var cmd = new SqlCommand(sql.ToString(),conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.TableMappings.Add("Table","Clientes");
                        da.Fill(ds);
                    }
                }
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
        
        private void button1_Click(object sender, EventArgs e)
        {
           
            var metadata = new Dictionary<string, string>();
            metadata.Add("id", "my_customer_id");

            var address = new CreateAddressRequest
            {
                Line1 = "Rua Olimpio Guimaraes Venâncio, 156",
                Line2 = "casa 1",
                ZipCode = "28022740",
                City = "Campos dos Goytacazes",
                State = "RJ",
                Country = "BR"
            };

            var phones = new CreatePhonesRequest
            {
                HomePhone = new CreatePhoneRequest
                {
                    AreaCode = "22",
                    CountryCode = "55",
                    Number = "27234577"
                },
                MobilePhone = new CreatePhoneRequest
                {
                    AreaCode = "22",
                    CountryCode = "55",
                    Number = "999100201"
                },
            };

            var cliente = new Cliente("Maurício Ribeiro", "mrexsolucoes@hotmail.com", "individual",
                "00000000000","male","XXXX",metadata,address,phones);
            

            // Senha em branco. Passando apenas a secret key
           
            
            //var request = new CreateCustomerRequest
            //{
            //    Name = "Carlos Andre de Souza Viana",
            //    Email = "viana.mail@gmail.com",
            //    Type = "individual",
            //    Document = "07069543786",
            //    Gender = "male",
            //    Code = "MY_CUSTOMER_002",
            //    Phones = phones,
            //    Address = address,
            //    Metadata = metadata
            //};
            

            //return View();

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
            string basicAuthUserName = "sk_test_J3xVZJPUBTWOnj6v";
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

       
    }
}