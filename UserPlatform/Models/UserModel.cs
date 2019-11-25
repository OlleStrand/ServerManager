using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using UserPlatform.Classes.Database;

namespace UserPlatform.Models
{
    public class UserModel
    {
        public int userID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public byte AdminLevel { get; set; } = 0;
        public bool IsBanned { get; set; } = false;

        public List<UserModel> GetUsers() => new DBConnect("UserMySQL").GetAllUsers();

        public bool CreateUser(UserModel user)
        {
            DBConnect dB = new DBConnect("UserMySQL");

            if (dB.ValidUser(user))
            {
                user.Password = HashPassword(user.Password);
                dB.CreateUser(user);
                return true;
            }
            return false;
        }

        //https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}