using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using UserPlatform.Models;
using System.Globalization;

namespace UserPlatform.Classes.Database
{
    public class DBConnect
    {
        private MySqlConnection _connection;

        public DBConnect(string connVar)
        {
            Initialize(connVar);
        }

        private void Initialize(string connVar)
        {
            string connstring = ConfigurationManager.ConnectionStrings[connVar].ToString();
            _connection = new MySqlConnection(connstring);
        }

        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        // Cannot connect to server.
                        break;

                    case 1045:
                        // Invalid user name and/or password.
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //ex.Message
                return false;
            }
        }

        /*
         *  METHODS FOR GETTING DATA
         *
         */

        public List<UserModel> GetAllUsers()
        {
            string query = "SELECT * FROM users";
            List<UserModel> list = new List<UserModel>();

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    bool banned = false;

                    if (Convert.ToByte(dataReader["banned"]) == 1)
                        banned = true;

                    UserModel user = new UserModel
                    {
                        Name = dataReader["name"].ToString(),
                        Email = dataReader["email"].ToString(),
                        UserName = dataReader["username"].ToString(),
                        Password = dataReader["password"].ToString(),
                        PhoneNumber = dataReader["phone_number"].ToString(),
                        AdminLevel = Convert.ToByte(dataReader["adminlevel"]),
                        IsBanned = banned
                    };

                    list.Add(user);
                }
                dataReader.Close();
                CloseConnection();

                return list;
            }
            else
            {
                return list;
            }
        }

        public void CreateUser(UserModel user)
        {

            string query = $"INSERT INTO users (name, email, username, password, phone_number) VALUES('{user.Name}', '{user.Email}', '{user.UserName}', '{user.Password}', '{user.PhoneNumber}')";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }
    }
}