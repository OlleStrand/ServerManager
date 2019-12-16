using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.ComponentModel;
using ServerManagerClient.Classes;

namespace ServerManagerClient.Classes.Database
{
    class DBConnect
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
         */

        public void UpdateSerialKey(string key, string hwid)
        {
            string query = $"UPDATE authentication SET activated=1, hwid='{hwid}' WHERE serialKey='{key}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public bool IsSerialKeyActivated(string key)
        {
            string query = $"SELECT * FROM authentication WHERE serialKey='{key}' AND activated=0";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    return false;
                }
                dataReader.Close();
                CloseConnection();
                return true;
            }
            else
                return false;
        }

        public bool IsSerialKeyInDB(string key)
        {
            string query = $"SELECT * FROM authentication WHERE serialKey='{key}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    return true;
                }
                dataReader.Close();
                CloseConnection();
                return false;
            }
            else
                return false;
        }

        public bool Login(string Username, string Password)
        {
            string query = $"SELECT password FROM users WHERE username = '{Username}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string savedPasswordHash = dataReader["password"].ToString();
                    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

                    Authentication.PasswordHash pHash = new Authentication.PasswordHash(hashBytes);

                    return !pHash.Verify(Password) ? false : true;
                }
                return false;
            }
            else
                return false;
        }

        public void GetUser(string Username)
        {
            string query = $"SELECT name, email, adminlevel, ownerToken FROM users WHERE username='{Username}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Authentication.User.Name = dataReader["name"].ToString();
                    Authentication.User.Email = dataReader["email"].ToString();
                    Authentication.User.AdminLevel = Convert.ToByte(dataReader["adminlevel"]);
                    Authentication.User.OwnerToken = dataReader["ownerToken"].ToString();
                }
                dataReader.Close();
                CloseConnection();
            }
        }
    }
}
