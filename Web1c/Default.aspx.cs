﻿using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web1c
{
    public partial class _Default : System.Web.UI.Page
    {
        public int loginStatus = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            loginStatusLabel.Text = "";
            DetailsView1.Visible = false;
            Logout_Input.Visible = false;
            Yield_Label.Text = "";
        }

        protected void Login_Input_Click(object sender, EventArgs e)
        {
            DetailsView1.DataBind(); //更新detailView
            loginStatus = (DetailsView1.DataItemCount == 1) ? 2 : 1;
            DetailsView1.Visible = (loginStatus & 2) == 2;
            Login_Input.Visible = (loginStatus & 2) == 0;
            Logout_Input.Visible = (loginStatus & 2) == 2;
            EnterStore_Input.Visible = (loginStatus & 2) == 2;
            switch (loginStatus)
            {
                case 2:
                    loginStatusLabel.Text = "你已經登入，以下是你的會員資料";
                    Session["Web_Account"] = DetailsView1.Rows[0].Cells[1].Text;
                    Session["Web_Password"] = DetailsView1.Rows[1].Cells[1].Text;
                    Session["Web_Points"] = DetailsView1.Rows[2].Cells[1].Text;
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
        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string account = Account_Input.Text.Length != 0 ? Account_Input.Text : " ";
            string password = Password_Input.Text.Length != 0 ? Password_Input.Text : " ";
            // if the user trying to login , we add "where" to the select query 
            // modify the command to add where 
            e.Command.CommandText += " WHERE ([Web_Account] = @Web_Account) AND ([Web_Password] = @Web_Password) ";
            SqlParameter account_query = new SqlParameter("@Web_Account", account);
            e.Command.Parameters.Add(account_query);
            SqlParameter password_query = new SqlParameter("@Web_Password", password);  
            e.Command.Parameters.Add(password_query);
        }

        protected void Logout_Input_Click(object sender, EventArgs e)
        {
            loginStatus = 0;
            Login_Input.Visible = true;
        }
    }
}
