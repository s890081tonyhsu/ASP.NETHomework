﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace Web1c
{
    public partial class _Default : System.Web.UI.Page
    {
        DataSet ds = null;
        SqlConnection Conn = null;
        static string Web1cConnectionString = "Data Source=127.0.0.1,9061;Initial Catalog=Web1c;Integrated Security=True";
        public int loginStatus = 0;
        public string msgBoxClass = "ui message row ";
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            Conn = new SqlConnection(Web1cConnectionString);
            loginMessage.Visible = false;
            User_Detail_Tab.Visible = false;
            User_Detail_Seg.Visible = false;
            loginStatusLabel.Text = "";
            Yield_Label.Text = "";
            if (Session["User_Account"] != null || Session["User_Password"] != null)
            {
                Account_Input.Text = Session["User_Account"].ToString();
                Password_Input.Text = Session["User_Password"].ToString();
                Login_Input_Click(sender, e);
            }
        }

        protected void Login_Input_Click(object sender, EventArgs e)
        {
            SqlQuery_User_Details(); //使用sql連線來查詢使用者
            UserDetailView.DataSource = ds.Tables["Users"];
            UserDetailView.DataBind(); // 到這時候detailsview才能拿到資料
            loginStatus = (UserDetailView.DataItemCount == 1) ? 2 : 1;
            Login_Form.Visible = (loginStatus & 2) == 0;
            User_Detail_Tab.Visible = (loginStatus & 2) == 2;
            User_Detail_Seg.Visible = (loginStatus & 2) == 2;
            switch (loginStatus)
            {
                case 2:
                    loginMessage.Visible = true;
                    loginMessage.Attributes["class"] = msgBoxClass + "success";
                    loginStatusLabel.Text = "你已經登入，以下是你的會員資料";
                    Session["User_Account"] = ds.Tables["Users"].Rows[0]["User_Account"];
                    Session["User_Password"] = ds.Tables["Users"].Rows[0]["User_Password"];
                    Session["User_Points"] = ds.Tables["Users"].Rows[0]["User_Points"];
                    break;
                case 1:
                    loginMessage.Visible = true;
                    loginMessage.Attributes["class"] = msgBoxClass + "error";
                    loginStatusLabel.Text = "你輸入了錯誤的帳號或密碼";
                    break;
                default:
                    loginMessage.Visible = false;
                    loginStatusLabel.Text = "";
                    if (Session["User_Account"] != null)
                    {
                        Session.Remove("User_Account");
                        Session.Remove("User_Password");
                        Session.Remove("User_Points");
                    }
                    break;
            }
        }
        protected void SqlQuery_User_Details()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT [User_Account], [User_Password], [User_Points], [User_Email] FROM [Users] WHERE ([User_Account] = @User_Account) AND ([User_Password] = @User_Password) ", Conn);
                ds = new DataSet();
                string account = Account_Input.Text.Length != 0 ? Account_Input.Text : " ";
                string password = Password_Input.Text.Length != 0 ? Password_Input.Text : " ";
                SqlParameter account_query = new SqlParameter("@User_Account", account);
                cmd.Parameters.Add(account_query);
                SqlParameter password_query = new SqlParameter("@User_Password", password);
                cmd.Parameters.Add(password_query);
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Users");
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('無法進行登入')</script>");
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void Logout_Input_Click(object sender, EventArgs e)
        {
            loginStatus = 0;
            Login_Form.Visible = (loginStatus & 2) == 0;
            User_Detail_Tab.Visible = (loginStatus & 2) == 2;
            User_Detail_Seg.Visible = (loginStatus & 2) == 2;
            loginMessage.Visible = true;
            loginMessage.Attributes["class"] = msgBoxClass + "success";
            loginStatusLabel.Text = "恭喜你，你已經成功完成登出";
            if (Session["User_Account"] != null)
            {
                Session.Remove("User_Account");
                Session.Remove("User_Password");
                Session.Remove("User_Points");
            }
        }
    }
}
