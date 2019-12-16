using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerManagerClient.Classes;

namespace ServerManagerClient
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            using (IsolatedStorageFile isolatedStorageFile =
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                try
                {
                    using (IsolatedStorageFileStream isolatedStorageFileStream =
                        new IsolatedStorageFileStream("LogInSettings.txt", System.IO.FileMode.Open, isolatedStorageFile))
                    {
                        using (System.IO.StreamReader streamReader = new System.IO.StreamReader(isolatedStorageFileStream))
                        {
                            if (streamReader.ReadLine() == "true")
                            {
                                Authentication.User.Username = streamReader.ReadLine();
                                //GOTO LicenseWindow
                                LicenseWindow lw = new LicenseWindow();
                                Hide();
                                lw.ShowDialog();

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

        private void ButtonSignIn_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Password or Username not entered", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(Authentication.Login(textBoxUsername.Text, textBoxPassword.Text))
            {
                MessageBox.Show("You successfully logged in", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Authentication.User.Username = textBoxUsername.Text;
                Authentication.SignedIn = true;

                if (checkBoxRemember.Checked)
                {
                    using (IsolatedStorageFile isolatedStorageFile =
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream =
                            new IsolatedStorageFileStream($"LogInSettings.txt", System.IO.FileMode.Create, isolatedStorageFile))
                        {
                            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(isolatedStorageFileStream))
                            {
                                streamWriter.WriteLine("true");
                                streamWriter.WriteLine(Authentication.User.Username);
                            }
                        }
                    }
                } else
                {
                    using (IsolatedStorageFile isolatedStorageFile =
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream =
                            new IsolatedStorageFileStream($"LogInSettings.txt", System.IO.FileMode.Create, isolatedStorageFile))
                        {
                            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(isolatedStorageFileStream))
                            {
                                streamWriter.WriteLine("false");
                            }
                        }
                    }
                }

                //GOTO LicenseWindow
                LicenseWindow lw = new LicenseWindow();
                Hide();
                lw.ShowDialog();

                Close();
            } else
            {
                MessageBox.Show("Wrong Password or Username", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
