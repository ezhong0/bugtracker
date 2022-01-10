﻿//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.DirectoryServices.Protocols;
//using System.Web;
//using Microsoft.Owin;

//namespace BugTracker.Security
//{
//    public class Authenticate
//    {

//        public static LoginResult AttemptLogin(IOwinContext owinContext, string username, string password)
//        {
//            LoginResult result = new LoginResult();

//            bool authenticated = check_password(username, password);

//            if (authenticated)
//            {
//                SQLString sql = new SQLString("select us_id, us_username, us_org from users where us_username = @us");
//                sql = sql.AddParameterWithValue("us", username);
//                DataRow dr = DbUtil.get_datarow(sql);
//                if (dr != null)
//                {
//                    Security.SignIn(owinContext, username);
//                    result.Success = true;
//                    result.ErrorMessage = string.Empty;
//                }
//                else
//                {
//                    // How could this happen?  If someday the authentication
//                    // method uses, say LDAP, then check_password could return
//                    // true, even though there's no user in the database";
//                    result.Success = false;
//                    result.ErrorMessage = "User not found in database";
//                }
//            }
//            else
//            {
//                result.Success = false;
//                result.ErrorMessage = "Invalid User or Password.";
//            }

//            return result;
//        }

//        public static bool check_password(string username, string password)
//        {

//            var sql = new SQLString(@"
//select us_username, us_id, us_password, isnull(us_salt,0) us_salt, us_active
//from users
//where us_username = @username");

//            sql = sql.AddParameterWithValue("username", username);

//            DataRow dr = btnet.DbUtil.get_datarow(sql);

//            if (dr == null)
//            {
//                Util.write_to_log("Unknown user " + username + " attempted to login.");
//                return false;
//            }

//            int us_active = (int)dr["us_active"];

//            if (us_active == 0)
//            {
//                Util.write_to_log("Inactive user " + username + " attempted to login.");
//                return false;
//            }

//            bool authenticated = false;
//            LinkedList<DateTime> failed_attempts = null;

//            // Too many failed attempts?
//            // We'll only allow N in the last N minutes.
//            failed_attempts = (LinkedList<DateTime>)HttpRuntime.Cache[username];

//            if (failed_attempts != null)
//            {
//                // Don't count attempts older than N minutes ago.
//                int minutes_ago = Convert.ToInt32(btnet.Util.get_setting("FailedLoginAttemptsMinutes", "10"));
//                int failed_attempts_allowed = Convert.ToInt32(btnet.Util.get_setting("FailedLoginAttemptsAllowed", "10"));

//                DateTime n_minutes_ago = DateTime.Now.AddMinutes(-1 * minutes_ago);
//                while (true)
//                {
//                    if (failed_attempts.Count > 0)
//                    {
//                        if (failed_attempts.First.Value < n_minutes_ago)
//                        {
//                            Util.write_to_log("removing stale failed attempt for " + username);
//                            failed_attempts.RemoveFirst();
//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }
//                    else
//                    {
//                        break;
//                    }
//                }

//                // how many failed attempts in last N minutes?
//                Util.write_to_log("failed attempt count for " + username + ":" + Convert.ToString(failed_attempts.Count));

//                if (failed_attempts.Count > failed_attempts_allowed)
//                {
//                    Util.write_to_log("Too many failed login attempts in too short a time period: " + username);
//                    return false;
//                }

//                // Save the list of attempts
//                HttpRuntime.Cache[username] = failed_attempts;
//            }

//            if (btnet.Util.get_setting("AuthenticateUsingLdap", "0") == "1")
//            {
//                authenticated = check_password_with_ldap(username, password);
//            }
//            else
//            {

//                authenticated = check_password_with_db(username, password, dr);
//            }

//            if (authenticated)
//            {
//                // clear list of failed attempts
//                if (failed_attempts != null)
//                {
//                    failed_attempts.Clear();
//                    HttpRuntime.Cache[username] = failed_attempts;
//                }

//                btnet.Util.update_most_recent_login_datetime((int)dr["us_id"]);
//                return true;
//            }
//            else
//            {
//                if (failed_attempts == null)
//                {
//                    failed_attempts = new LinkedList<DateTime>();
//                }

//                // Record a failed login attempt.
//                failed_attempts.AddLast(DateTime.Now);
//                HttpRuntime.Cache[username] = failed_attempts;

//                return false;
//            }
//        }

//        public static bool check_password_with_ldap(string username, string password)
//        {
//            // allow multiple, seperated by a pipe character
//            string dns = btnet.Util.get_setting(
//                "LdapUserDistinguishedName",
//                "uid=$REPLACE_WITH_USERNAME$,ou=people,dc=mycompany,dc=com");

//            string[] dn_array = dns.Split('|');

//            string ldap_server = btnet.Util.get_setting(
//                "LdapServer",
//                "127.0.0.1");

//            using (LdapConnection ldap = new LdapConnection(ldap_server))
//            {

//                for (int i = 0; i < dn_array.Length; i++)
//                {
//                    string dn = dn_array[i].Replace("$REPLACE_WITH_USERNAME$", username);

//                    System.Net.NetworkCredential cred = new System.Net.NetworkCredential(dn, password);

//                    ldap.AuthType = (System.DirectoryServices.Protocols.AuthType)System.Enum.Parse
//                        (typeof(System.DirectoryServices.Protocols.AuthType),
//                        Util.get_setting("LdapAuthType", "Basic"));

//                    try
//                    {
//                        ldap.Bind(cred);
//                        btnet.Util.write_to_log("LDAP authentication ok using " + dn + " for username: " + username);
//                        return true;
//                    }
//                    catch (Exception e)
//                    {
//                        string exception_msg = e.Message;

//                        if (e.InnerException != null)
//                        {
//                            exception_msg += "\n";
//                            exception_msg += e.InnerException.Message;
//                        }

//                        btnet.Util.write_to_log("LDAP authentication failed using " + dn + ": " + exception_msg);
//                    }
//                }
//            }

//            return false;
//        }

//        public static bool check_password_with_db(string username, string enteredPassword, DataRow dr)
//        {
//            Util.update_user_password(1, "admin");
//            string salt = (string)dr["us_salt"];
//            string hashedEnteredPassword = Util.HashString(enteredPassword, salt.ToString());
//            string databasePassword = (string)dr["us_password"];

//            if (hashedEnteredPassword == databasePassword)
//            {
//                return true;
//            }
//            else
//            {
//                Util.write_to_log(String.Format("User {0} entered an incorrect password.", username));
//                return false;
//            }
//        }
//    }

//}