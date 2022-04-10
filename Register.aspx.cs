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
    public partial class WebForm2 : System.Web.UI.Page
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
            Dictionary<string, string> db = new Dictionary<string, string>();
            SqlCommand getUsersCredentials = new SqlCommand("SELECT [login], [password] FROM [Users]", sqlConnection);

            SqlDataReader sqlReader = null;

            try
            {
                sqlReader = await getUsersCredentials.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    db.Add(Convert.ToString(sqlReader["login"]), Convert.ToString(sqlReader["password"]));
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
            if (!(db.Count>0)) // if bd is empty - reset id 
            {
                SqlCommand resetId = new SqlCommand("DBCC CHECKIDENT(Users, RESEED, 0)", sqlConnection);
                resetId.ExecuteNonQuery();
            }
            if (!db.Keys.Contains(loginTb.Text)) // if login is unregistered
            {
                SqlCommand registerUser = new SqlCommand("INSERT INTO [Users](login,password,regDate)VALUES(@Login,@Password,@regDate)", sqlConnection);
                registerUser.Parameters.AddWithValue("Login", loginTb.Text);
                registerUser.Parameters.AddWithValue("Password", passTb.Text);
                registerUser.Parameters.AddWithValue("regDate", DateTime.Now);

                await registerUser.ExecuteNonQueryAsync();

                ClientScript.RegisterClientScriptBlock(this.GetType(), "MessageBox", "Success! Now you can login with your credentials!", true); // you can read this if you're slow :)
                Response.Redirect("Login.aspx");
            }
            else
            {
                string js = "alert('This login is already registered!')";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MessageBox", js, true); // show alert message
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (sqlConnection!=null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }
    }
     
}