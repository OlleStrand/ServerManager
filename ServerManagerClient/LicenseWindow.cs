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
using System.IO.IsolatedStorage;

namespace ServerManagerClient
{
    public partial class LicenseWindow : Form
    {
        public LicenseWindow()
        {
            InitializeComponent();
        }

        private void LicenseWindow_Load(object sender, EventArgs e)
        {
            using (IsolatedStorageFile isolatedStorageFile =
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    using (IsolatedStorageFileStream isolatedStorageFileStream2 =
                        new IsolatedStorageFileStream($"{Authentication.Username}Settings.txt", System.IO.FileMode.Open, isolatedStorageFile))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(isolatedStorageFileStream2))
                        {
                            var activated = Authentication.IsActivated(sr.ReadLine());

                            if (activated)
                            {
                                //GOTO MainWindow
                                MainWindow mw = new MainWindow();
                                Hide();
                                mw.ShowDialog();

                                Close();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        private void ButtonAuth_Click(object sender, EventArgs e)
        {
            List<string> key = new List<string>();

            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    key.Add(item.Text);
                }
            }
            key.Reverse();

            if (Authentication.Activate(string.Join("", key)))
            {
                //GOTO MainWindow
                MainWindow mw = new MainWindow();
                Hide();
                mw.ShowDialog();

                Close();
            }
        }
    }
}
