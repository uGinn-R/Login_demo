using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Login_demo
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private SqlConnection sqlConnection = null;
        protected async void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync();
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, string> > db = new Dictionary<string, Dictionary<string, string> >();
            
            SqlCommand getUsersCredentials = new SqlCommand("SELECT [login], [password], [status] FROM [Users]", sqlConnection);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await getUsersCredentials.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    db.Add(Convert.ToString(sqlReader["login"]), new Dictionary<string, string>());
                    db[Convert.ToString(sqlReader["login"])].Add(Convert.ToString(sqlReader["password"]), Convert.ToString(sqlReader["status"]));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }
            if (loginTb.Text != "" || passTb.Text != "")
            {
                try
                {
                    if (db[loginTb.Text].ContainsKey(passTb.Text)) // if login-pass pair found
                    {
                        if (db[loginTb.Text].ContainsValue("BLOCKED"))
                        {
                            ShowJsAlert("Your account is BLOCKED!"); // show alert message 
                            loginTb.Text = "";
                            passTb.Text = "";
                            return;
                        }
                        HttpCookie login = new HttpCookie("login", loginTb.Text);
                        HttpCookie pass = new HttpCookie("pass", passTb.Text);
                        
                        Response.Cookies.Add(login);
                        Response.Cookies.Add(pass);
                        
                        SqlCommand updateLoginDate = new SqlCommand("UPDATE Users SET lastlogin = @Date WHERE login = @Login", sqlConnection);

                        updateLoginDate.Parameters.AddWithValue("Date", DateTime.Now);
                        updateLoginDate.Parameters.AddWithValue("Login", loginTb.Text);

                        await updateLoginDate.ExecuteNonQueryAsync();
                        
                        Response.Redirect("UserPage.aspx", false);
                    }
                    else
                    {
                        ShowJsAlert("Incorrect password!");
                    }
                }
                catch (Exception)
                {
                    ShowJsAlert("Incorrect login or password!");
                }
            }
            else
            {
                ShowJsAlert("Enter login and password first!");
            }            
        }
        private void ShowJsAlert(string message)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "MessageBox", $"alert('{message}')" , true); // show alert message 

        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }
    }
}