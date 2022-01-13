using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.Protocols;
using System.Web;
using Microsoft.Owin;

namespace BugTracker.Security
{
    public class Authenticate
    {

        public static LoginResult AttemptLogin(IOwinContext owinContext, string username, string password)
        {
            LoginResult result = new LoginResult();

            string connection_string = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";
            //SqlConnection conn = new SqlConnection(connection_string);
            //conn.Open();


            string sql = "SELECT NAME FROM USERS WHERE USERNAME = " + username;

            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand(sql, conn);
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