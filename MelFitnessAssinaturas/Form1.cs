using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MundiAPI.PCL;
using MundiAPI.PCL.Models;

namespace MelFitnessAssinaturas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string basicAuthUserName = "sk_test_J3xVZJPUBTWOnj6v";

            // Senha em branco. Passando apenas a secret key
            string basicAuthPassword = "";

            var client = new MundiAPIClient(basicAuthUserName, basicAuthPassword);

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

            var request = new CreateCustomerRequest
            {
                Name = "Carlos Andre de Souza Viana",
                Email = "viana.mail@gmail.com",
                Type = "individual",
                Document = "07069543786",
                Gender = "male",
                Code = "MY_CUSTOMER_002",
                Phones = phones,
                Address = address,
                Metadata = metadata
            };

            var response = client.Customers.CreateCustomer(request);

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
                Console.WriteLine(string.Format("Id: {0} -- {1}", item.Id, item.Code));
            }
        }
    }
}
