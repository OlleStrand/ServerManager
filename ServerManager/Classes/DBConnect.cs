using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using ServerManager.Models;
using ServerManager.Classes;
using System.Globalization;

namespace ServerManager.Classes.Database
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
                        UserID = Convert.ToInt32(dataReader["id"]),
                        Name = dataReader["name"].ToString(),
                        Email = dataReader["email"].ToString(),
                        Username = dataReader["username"].ToString(),
                        Password = dataReader["password"].ToString(),
                        PhoneNumber = dataReader["phone_number"].ToString(),
                        AdminLevel = Convert.ToByte(dataReader["adminlevel"]),
                        IsBanned = banned,
                        OwnerToken = dataReader["ownerToken"].ToString()
                    };

                    list.Add(user);
                }
                dataReader.Close();
                CloseConnection();

                return list;
            }
            else
                return list;
        }

        public List<ServerModel> GetUserServers(UserModel user)
        {
            string query = $"SELECT * FROM servers WHERE ownerToken = '{user.OwnerToken}'";
            List<ServerModel> list = new List<ServerModel>();

            if (user.OwnerToken == "" || user.OwnerToken == null)
                return list;

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ServerModel server = new ServerModel
                    {
                        ServerID = Convert.ToInt32(dataReader["serverId"]),
                        ServerToken = dataReader["serverToken"].ToString(),
                        ServerName = dataReader["name"].ToString()
                    };

                    list.Add(server);
                }
                dataReader.Close();
                CloseConnection();

                return list;
            }
            else
                return list;
        }

        public void CreateUser(UserModel user)
        {
            string query = $"INSERT INTO users (name, email, username, password, phone_number, ownerToken) VALUES('{user.Name}', '{user.Email}', '{user.Username}', '{user.Password}', '{user.PhoneNumber}', '{user.OwnerToken}')";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public bool ValidUser(UserModel user)
        {
            string query = $"SELECT * FROM users WHERE username = '{user.Username}'";

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

        public bool Login(string username, string Password)
        {
            string query = $"SELECT password FROM users WHERE username = '{username}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string savedPasswordHash = dataReader["password"].ToString();
                    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

                    PasswordHash pHash = new PasswordHash(hashBytes);

                    return !pHash.Verify(Password) ? false : true;
                }
                return false;
            }
            else
                return false;
        }

        public UserModel GetUser(int id)
        {
            string query = $"SELECT * FROM users WHERE id={id}";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                UserModel user = new UserModel();
                while (dataReader.Read())
                {
                    bool banned = false;
                    if (Convert.ToByte(dataReader["banned"]) == 1)
                        banned = true;

                    user = new UserModel
                    {
                        UserID = Convert.ToInt32(dataReader["id"]),
                        Name = dataReader["name"].ToString(),
                        Email = dataReader["email"].ToString(),
                        Username = dataReader["username"].ToString(),
                        Password = dataReader["password"].ToString(),
                        PhoneNumber = dataReader["phone_number"].ToString(),
                        AdminLevel = Convert.ToByte(dataReader["adminlevel"]),
                        IsBanned = banned,
                        OwnerToken = dataReader["ownerToken"].ToString() ?? new UserModel().GenerateOwnerToken()
                    };
                }
                
                dataReader.Close();
                CloseConnection();

                return user;
            }
            else
                return null;
        }

        public UserModel GetUser(string name)
        {
            string query = $"SELECT * FROM users WHERE username='{name}'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                UserModel user = new UserModel();
                while (dataReader.Read())
                {
                    bool banned = false;
                    if (Convert.ToByte(dataReader["banned"]) == 1)
                        banned = true;

                    user = new UserModel
                    {
                        UserID = Convert.ToInt32(dataReader["id"]),
                        Name = dataReader["name"].ToString(),
                        Email = dataReader["email"].ToString(),
                        Username = dataReader["username"].ToString(),
                        Password = dataReader["password"].ToString(),
                        PhoneNumber = dataReader["phone_number"].ToString(),
                        AdminLevel = Convert.ToByte(dataReader["adminlevel"]),
                        IsBanned = banned,
                        OwnerToken = dataReader["ownerToken"].ToString()
                    };
                }
                
                
                dataReader.Close();
                CloseConnection();

                return user;
            }
            else
                return null;
        }

        public void UpdateUser(UserModel user)
        {
            string query = $"UPDATE users SET name='{user.Name}', email='{user.Email}', username='{user.Username}', phone_number='{user.PhoneNumber}', adminlevel={user.AdminLevel}, banned={user.IsBanned} WHERE id={user.UserID}";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public void InsertNewOwnerToken(UserModel user)
        {
            string query = $"UPDATE users SET ownerToken='{user.OwnerToken}' WHERE id={user.UserID}";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }
    }
}