using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ServerManager.Models
{
    public class ServerModel
    {
        private const int serverTokenLength = 48;

        public string ServerToken { get; set; }
        public string OwnerToken { get; set; }
        public byte RamUsage { get; set; }
        public byte CpuUsage { get; set; }
        public short Players { get; set; }
        public short MaxPlayers { get; set; }

        public string Game { get; set; }
        public string ServerName { get; set; }

        public string GenerateServerToken(int length = serverTokenLength)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890=?";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }
            return res.ToString();
        }
    }
}
