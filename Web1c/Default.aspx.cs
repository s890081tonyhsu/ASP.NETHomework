using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Web1c
{
    public partial class _Default : System.Web.UI.Page
    {
        DataSet ds = null;
        SqlConnection Conn = null;
        static string Web1cConnectionString = "Data Source=127.0.0.1,9061;Initial Catalog=Web1c;Integrated Security=True";
        public int loginStatus = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Conn = new SqlConnection(Web1cConnectionString);
            loginStatusLabel.Text = "";
            UserDetailView.Visible = false;
            Logout_Input.Visible = false;
            Yield_Label.Text = "";
            if (Session["Web_Account"] != null || Session["Web_Password"] != null)
            {
                Account_Input.Text = Session["Web_Account"].ToString();
                Password_Input.Text = Session["Web_Password"].ToString();
                Login_Input_Click(sender, e);
            }
        }

        protected void Login_Input_Click(object sender, EventArgs e)
        {
            SqlQuery_User_Details(); //使用sql連線來查詢使用者
            UserDetailView.DataSource = ds.Tables["Table"];
            UserDetailView.DataBind(); // 到這時候detailsview才能拿到資料
            loginStatus = (UserDetailView.DataItemCount == 1) ? 2 : 1;
            UserDetailView.Visible = (loginStatus & 2) == 2;
            Login_Input.Visible = (loginStatus & 2) == 0;
            Logout_Input.Visible = (loginStatus & 2) == 2;
            EnterStore_Input.Visible = (loginStatus & 2) == 2;
            switch (loginStatus)
            {
                case 2:
                    loginStatusLabel.Text = "你已經登入，以下是你的會員資料";
                    Session["Web_Account"] = ds.Tables["Table"].Rows[0]["Web_Account"];
                    Session["Web_Password"] = ds.Tables["Table"].Rows[0]["Web_Password"];
                    Session["Web_Points"] = ds.Tables["Table"].Rows[0]["Web_Points"];
                    break;
                case 1:
                    loginStatusLabel.Text = "你輸入了錯誤的帳號或密碼";
                    break;
                default:
                    loginStatusLabel.Text = "";
                    if (Session["Web_Account"] != null)
                    {
                        Session.Remove("Web_Account");
                        Session.Remove("Web_Points");
                    }
                    break;
            }
        }
        protected void SqlQuery_User_Details()
        {
            SqlCommand cmd = new SqlCommand("SELECT [Web_Account], [Web_Password], [Web_Points], [Web_Email] FROM [Table] WHERE ([Web_Account] = @Web_Account) AND ([Web_Password] = @Web_Password) ", Conn);
            ds = new DataSet();
            string account = Account_Input.Text.Length != 0 ? Account_Input.Text : " ";
            string password = Password_Input.Text.Length != 0 ? Password_Input.Text : " ";
            SqlParameter account_query = new SqlParameter("@Web_Account", account);
            cmd.Parameters.Add(account_query);
            SqlParameter password_query = new SqlParameter("@Web_Password", password);
            cmd.Parameters.Add(password_query);
            Conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Table");
            cmd.ExecuteNonQuery();
            Conn.Close();
            
        }

        protected void Logout_Input_Click(object sender, EventArgs e)
        {
            loginStatus = 0;
            Login_Input.Visible = true;
        }
    }
}
