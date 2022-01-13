using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using BugTracker.Security;

namespace BugTracker.Views.Home
{
    public partial class Index
    {

        ///////////////////////////////////////////////////////////////////////
        public void Page_Load(Object sender, EventArgs e)
        {
            string connection_string = "server=localhost;database=bugtrackerdb;user=root;password=Bl@ckpink";
            SqlConnection conn = new SqlConnection(connection_string);


            string auth_mode = Util.get_setting("WindowsAuthentication", "0");
            HttpCookie username_cookie = Request.Cookies["user"];
            string previous_auth_mode = "0";
            if (username_cookie != null)
            {
                previous_auth_mode = username_cookie["NTLM"];
            }
        }
    }
}
