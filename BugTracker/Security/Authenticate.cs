using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.DirectoryServices.Protocols;
using System.Web;
using Microsoft.Owin;
using MySql.Data.MySqlClient;

namespace BugTracker.Security
{
    public class Authenticate
    {

        public static LoginResult AttemptLogin(string email, string password)
        {
            LoginResult result = new();

            string connection_string = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";
            //SqlConnection conn = new SqlConnection(connection_string);
            //conn.Open();


            string sql = "SELECT * FROM USER WHERE EMAIL='" + email + "'";

            DataTable ds = new();
            using (MySqlConnection conn = new(connection_string))
            {
                conn.Open();
                using MySqlDataAdapter da = new();
                da.SelectCommand = new MySqlCommand(sql, conn);
                da.Fill(ds);
            }

            if (ds.Rows.Count != 1)
            {
                result.Success = false;
                result.ErrorMessage = "cant find email";
            }
            else
            {
                if ((string)((ds.Rows[0])["password"]) == password)
                {
                    result.Success = true;
                    result.FirstName = (string)((ds.Rows[0])["firstname"]);
                    result.LastName = (string)((ds.Rows[0])["lastname"]);
                    result.Email = (string)((ds.Rows[0])["email"]);
                    result.UserId = (int)((ds.Rows[0])["UserId"]);
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "wrong password";
                }
            }

            return result;
        }

        public static Boolean CreateAccount(string email, string firstname, string lastname, string password)
        {
            //CreateAccountResult result = new();

            string connection_string = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";

            string sqlemail = "SELECT * FROM USER WHERE EMAIL='" + email + "'";
            string sql = "INSERT INTO USER (roleid, email, firstname, lastname, password) VALUES (-1, '" + email + "', '" + firstname + "', '" + lastname + "', '" + password + "')" ;

            using MySqlConnection conn = new(connection_string);
            DataTable ds = new();
            using (MySqlDataAdapter da = new())
            {
                da.SelectCommand = new MySqlCommand(sqlemail, conn);
                da.Fill(ds);

            }
            conn.Open();
            if (ds.Rows.Count >= 1)
            {
                return false;
                //result.ErrorMessage = "already has account";
            }
            else
            {
                MySqlCommand com = new(sql, conn);
                com.ExecuteNonQuery();
                return true;
                //result.FirstName = firstname;
                //result.LastName = lastname;
                //result.Email = email;
            }
        }
    }
}