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
    public partial class WebForm3 : System.Web.UI.Page
    {
        private SqlConnection sqlConnection = null;
        public string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        HttpCookie login_cookie = null;
        HttpCookie pass_cookie = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            login_cookie = Request.Cookies["login"];
            pass_cookie = Request.Cookies["pass"];
            
            if (login_cookie != null && pass_cookie != null)
            {
                if (!IsPostBack)
                {
                    BindTable();
                    return;
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        private void BindTable()
        {
            sqlConnection = new SqlConnection(connectionString);
            string query = "SELECT * from Users";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                GridView1.DataSource = reader;
                GridView1.DataBind();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MessageBox", "alert('Database is empty!')", true); // show alert message 
            }
        }
        protected void ChkEmpty_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkstatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkstatus.NamingContainer;
        }

        protected void ChkHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkHeader = (CheckBox)GridView1.HeaderRow.FindControl("ChkHeader");
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chkRow = (CheckBox)row.FindControl("ChkEmpty");

                if (chkHeader.Checked == true)
                {
                    chkRow.Checked = true;
                }
                else
                {
                    chkRow.Checked = false;
                }
            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox chkdelete = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkEmpty");
                if (chkdelete.Checked)
                {
                    int id = int.Parse(GridView1.Rows[i].Cells[1].Text);
                    string current_login = GridView1.Rows[i].Cells[2].Text;
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand("DELETE from Users WHERE id='"+id+"'", sqlConnection);
                        cmd.ExecuteNonQuery();
                        GridView1.EditIndex = -1;
                        sqlConnection.Close();
                    }
                    if (current_login.Equals(login_cookie.Value))
                    {
                        Response.Redirect("Logout.aspx");
                    }
                }
            }
            BindTable();
        }

        protected void btnBlock_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox chkblock = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkEmpty");
                if (chkblock.Checked)
                {
                    int id = int.Parse(GridView1.Rows[i].Cells[1].Text);
                    string current_login = GridView1.Rows[i].Cells[2].Text;
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand updateData = new SqlCommand("UPDATE Users SET status = 'BLOCKED' WHERE id='"+id+"'", sqlConnection);
                        updateData.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                    if (current_login.Equals(login_cookie.Value))
                    {
                        Response.Redirect("Logout.aspx");
                    }
                }
               
            }
            BindTable();
        }

        protected void btnUnblock_Click(object sender, ImageClickEventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                CheckBox chkunblock = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkEmpty");
                if (chkunblock.Checked)
                {
                    int id = int.Parse(GridView1.Rows[i].Cells[1].Text);
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();

                        SqlCommand updateData = new SqlCommand("UPDATE Users SET status = '' WHERE id='" + id + "'", sqlConnection);
                        updateData.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }
            }
            BindTable();
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