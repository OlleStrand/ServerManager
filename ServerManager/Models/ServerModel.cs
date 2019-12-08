using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Models
{
    public class ServerModel
    {
        private const int serverTokenLength = 48;

        public int ServerID { get; set; }
        public string ServerToken { get; set; }
        public string OwnerToken { get; set; }
        public string Game { get; set; }
        public string ServerName { get; set; }

        public byte RamUsage { get; set; } //Used as percentage
        public byte CpuUsage { get; set; }
        public short Players { get; set; }
        public short MaxPlayers { get; set; }

        public string GenerateServerToken(int length = serverTokenLength)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890=?";
            StringBuilder result = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    result.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }
            return result.ToString();
        }

        public void AcuireServerInfo() //Change to Task when implemented
        {
            //RabbitMQ Code Listener
        }

        public string SplitString(string input)
        {
            return "";
        }
    }
}
