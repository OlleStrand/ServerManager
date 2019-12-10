using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

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


    }
}
