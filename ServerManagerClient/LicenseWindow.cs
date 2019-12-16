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
                        new IsolatedStorageFileStream($"{Authentication.User.Username}Settings.txt", System.IO.FileMode.Open, isolatedStorageFile))
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(isolatedStorageFileStream2))
                        {
                            string license = sr.ReadLine();
                            var activated = Authentication.IsActivated(license);

                            if (activated)
                            {
                                Authentication.User.License = license;
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

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            foreach (Control c in Controls)
            {
                if (c is TextBox)
                {
                    c.Text = string.Empty;
                }
            }
        }

        #region AutomaticFocus
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.MaxLength == textBox1.TextLength)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.MaxLength == textBox2.TextLength)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.MaxLength == textBox3.TextLength)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.MaxLength == textBox4.TextLength)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.MaxLength == textBox5.TextLength)
            {
                SelectNextControl((Control)sender, true, true, true, true);
            }
        }

        #endregion
    }
}
