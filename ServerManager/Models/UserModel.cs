using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using ServerManager.Classes.Database;
using ServerManager.Classes;
using System.Text;

namespace ServerManager.Models
{
    public class UserModel
    {
        private const int ownerTokenLength = 48;

        public int UserID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Email Adress")]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string OwnerToken { get; set; }
         
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Admin Level")]
        public byte AdminLevel { get; set; } = 0;

        [Display(Name = "Banned")]
        public bool IsBanned { get; set; } = false;

        public List<UserModel> GetUsers() => new DBConnect("UserMySQL").GetAllUsers();
        public UserModel GetUserById(int userId) => new DBConnect("UserMySQL").GetUser(userId) ?? null;
        public UserModel GetUser() => new DBConnect("UserMySQL").GetUser(Username) ?? null;
        public void UpdateUser() => new DBConnect("UserMySQL").UpdateUser(this);

        public bool CreateUser()
        {
            DBConnect dB = new DBConnect("UserMySQL");

            if (dB.ValidUser(this))
            {
                PasswordHash pHash = new PasswordHash(Password);
                byte[] hashBytes = pHash.ToArray();
                Password = Convert.ToBase64String(hashBytes);
                OwnerToken = GenerateOwnerToken();
                dB.CreateUser(this);
                return true;
            }
            return false;
        }
        public bool Login() => new DBConnect("UserMySQL").Login(Username, Password);

        public void InsertNewOwnerToken()
        {
            DBConnect dB = new DBConnect("UserMySQL");
            OwnerToken = GenerateOwnerToken();
            dB.InsertNewOwnerToken(this);
        }

        public string GenerateOwnerToken(int length = ownerTokenLength)
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