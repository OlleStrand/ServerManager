using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerManagerClient.Classes;
using ServerManagerClient.Classes.Database;
using System.Diagnostics;

namespace ServerManagerClient
{
    public partial class MainWindow : Form
    {
        public static Process _SPID;
        private static Server _server;

        public MainWindow()
        {
            InitializeComponent();

            new DBConnect("UserMySQL").GetUser(Authentication.User.Username);
            _server = new Server();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            string[] args = { "" };
            _SPID = _server.StartServer("cmd.exe", args);
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            //Get SPID from DB
            _server.StopServer(_SPID);
        }
    }
}
