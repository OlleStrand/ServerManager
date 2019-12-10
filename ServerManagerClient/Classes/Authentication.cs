using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Management;
using System.Security.Cryptography;
using ServerManagerClient.Classes.Database;
using System.Windows.Forms;
using System.IO.IsolatedStorage;

namespace ServerManagerClient.Classes
{
    sealed class Authentication
    {
        private static bool Activated { get; set; }

        public static bool SignedIn { get; set; } = true;
        public static string Username { get; set; }

        public static async Task<string> GetHWID()
        {
            byte[] bytes;
            byte[] hashedBytes;
            StringBuilder sb = new StringBuilder();

            Task t1 = Task.Run(() =>
            {
                sb.Append(HWID.ProcessorID());
                sb.Append(HWID.DiskID());
                sb.Append(HWID.MotherBoardID());
            });
            Task.WaitAll(t1);
            bytes = Encoding.UTF8.GetBytes(sb.ToString());
            hashedBytes = SHA256.Create().ComputeHash(bytes);

            return await Task.FromResult(Convert.ToBase64String(hashedBytes));
        }

        public static bool Activate(string key)
        {
            if (!SignedIn)
                return false;

            if (new DBConnect("UserMySQL").IsSerialKeyInDB(key))
            {
                if (!IsActivated(key))
                {
                    UpdateActivation(key);

                    using (IsolatedStorageFile isolatedStorageFile = 
                        IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream = 
                            new IsolatedStorageFileStream($"{Username}Settings.txt", System.IO.FileMode.Create, isolatedStorageFile))
                        {
                            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(isolatedStorageFileStream))
                            {
                                streamWriter.WriteLine(key);
                            }
                        }
                    }

                    Activated = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("Key is already activated", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Activated = false;
                    return false;
                }
            } else
            {
                MessageBox.Show("Key does not exist", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Activated = false;
                return false;
            }
        }

        public async static void UpdateActivation(string key)
        {
            DBConnect dBConnect = new DBConnect("UserMySQL");
            string hwid = await GetHWID();

            dBConnect.UpdateSerialKey(key, hwid);

            MessageBox.Show("Your Software Key has been activated", "Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool IsActivated(string key) => new DBConnect("UserMySQL").IsSerialKeyActivated(key);

        public static bool Login(string username, string password) => new DBConnect("UserMySQL").Login(username, password);

        private class HWID
        {
            public static string ProcessorID()
            {
                ManagementObjectCollection mbsList = null;
                ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                mbsList = mbs.Get();
                string id = "";
                foreach (ManagementObject mo in mbsList)
                {
                    id = mo["ProcessorID"].ToString();
                }
                return id;
            }

            public static string DiskID()
            {
                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                dsk.Get();
                return dsk["VolumeSerialNumber"].ToString();
            }

            public static string MotherBoardID()
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = mos.Get();
                string serial = "";
                foreach (ManagementObject mo in moc)
                {
                    serial = (string)mo["SerialNumber"];
                }
                return serial;
            }
        }

        public sealed class PasswordHash
        {
            private const int SaltSize = 16, HashSize = 20, HashIter = 10000;
            private readonly byte[] _salt, _hash;

            public PasswordHash(string password)
            {
                new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
                _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            }

            public PasswordHash(byte[] hashBytes)
            {
                Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
                Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
            }

            public PasswordHash(byte[] salt, byte[] hash)
            {
                Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
                Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
            }

            public byte[] ToArray()
            {
                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
                return hashBytes;
            }

            public byte[] Salt { get { return (byte[])_salt.Clone(); } }
            public byte[] Hash { get { return (byte[])_hash.Clone(); } }

            public bool Verify(string password)
            {
                byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                    if (test[i] != _hash[i])
                        return false;
                return true;
            }
        }
    }
}
