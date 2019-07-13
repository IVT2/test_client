using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.API.Builder;
using InstaSharper.Logger;

namespace client
{
    public partial class Form1 : Form
    {
        private static UserSessionData user;
        private static IInstaApi api;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string[] text = textBox2.Text.Split(' ');
            string userName = textBox1.Text;
            string password = textBox2.Text;

            const int port = 8888;
            const string address = "127.0.0.1";
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();
                int i = 0;
                while (i < 1)
                {

                    string message = String.Format("{0}, {1}", userName, password);

                    byte[] userData = Encoding.Unicode.GetBytes(userName);
                    byte[] passwordData = Encoding.Unicode.GetBytes(password);
                    stream.Write(userData, 0, userData.Length);
                    stream.Write(passwordData, 0, passwordData.Length);
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    message = builder.ToString();
                    i++;
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            user = new UserSessionData();
            user.UserName = textBox1.Text;
            user.Password = textBox2.Text;
            Login();
        }

        public async static void Login()
        {
            label3.Text = "Connecting";
            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 3))
                .Build();

            var loginReguest = await api.LoginAsync();
            if (loginReguest.Succeeded)
                label3.Text = "ok";
            else
                label3.Text = "!ok " + loginReguest.Info.Message;
        }
    }
}
