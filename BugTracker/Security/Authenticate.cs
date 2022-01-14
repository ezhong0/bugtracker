using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.Protocols;
using System.Web;
using Microsoft.Owin;
using MySql.Data.MySqlClient;

namespace BugTracker.Security
{
    public class Authenticate
    {

        public static LoginResult AttemptLogin(string username, string password)
        {
            LoginResult result = new LoginResult();

            string connection_string = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";
            //SqlConnection conn = new SqlConnection(connection_string);
            //conn.Open();


            string sql = "SELECT NAME FROM USER WHERE USERNAME='" + username + "'";

            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(connection_string))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter())
                {
                    da.SelectCommand = new MySqlCommand(sql, conn);
                    da.Fill(ds);
                }
            }

            if (ds.Tables[0].Rows.Count != 1)
            {
                result.Success = false;
                result.ErrorMessage = "cant find username";
            }
            else
            {
                if ((string)ds.Tables[0].Rows[0]["password"] == password)
                {
                    result.Success = true;
                    result.ErrorMessage = string.Empty;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "wrong password";
                }
            }

            return result;
        }
    }

}