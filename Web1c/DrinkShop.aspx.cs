using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Web1c
{
    public partial class DrinkShop : System.Web.UI.Page
    {
        DataSet ds = null;
        SqlConnection Conn = null;
        static string Web1cConnectionString = "Data Source=127.0.0.1,9061;Initial Catalog=Web1c;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Web_Account"] == null) Response.Redirect("./Default.aspx");
            SessionName.Text = Session["Web_Account"] + "歡迎光臨";
            SessionPoints.Text = "目前你還有" + Session["Web_Points"] + "元";
            Conn = new SqlConnection(Web1cConnectionString);
            Conn.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT drink_id, drink_name, drink_price FROM Drink ORDER BY drink_id", Conn);
            ds = new DataSet();
            da.Fill(ds, "Drink");
            ds.Tables["Drink"].PrimaryKey = new DataColumn[] { ds.Tables["Drink"].Columns["drink_id"] };

            DrinkName_Dropdown.DataValueField = "drink_id";
            DrinkName_Dropdown.DataTextField = "drink_name";

            DrinkName_Dropdown.DataSource = ds.Tables["Drink"].DefaultView;
            if (!IsPostBack)
            {
                DrinkName_Dropdown.DataBind();
                ListItem item = new ListItem("請選擇飲料", "0");
                DrinkName_Dropdown.Items.Insert(0, item);
            }
            Conn.Close();
        }

        protected void DrinkName_Dropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string drink_id = DrinkName_Dropdown.SelectedItem.Value;
            DataRow findSelected = ds.Tables["Drink"].Rows.Find(drink_id);
            if(findSelected != null)
            {
                string drink_name = findSelected["drink_name"].ToString();
                string drink_price = findSelected["drink_price"].ToString();
                DrinkName_Show.Text = "你選擇的是" + drink_name + "，";
                DrinkPrice_Show.Text = "每杯" + drink_price + "元";
                Drink_Image.ImageUrl = "./Images/drinks/" + drink_name + ".jpg";
                Drink_Image.Visible = true;
            }
            else
            {
                DrinkName_Show.Text = "你沒有選擇飲料";
                DrinkPrice_Show.Text = "";
                Drink_Image.Visible = false;
            }
        }

        protected void Order_New_Click(object sender, EventArgs e)
        {
            Conn = new SqlConnection(Web1cConnectionString);
            Conn.Open();
            string order_user_pass = Session["Web_Password"].ToString();
            SqlCommand cmd = new SqlCommand("INSERT INTO [Order]( order_time, order_user_pass) VALUES( GETDATE(), @order_user_pass);", Conn);
            cmd.Parameters.AddWithValue("@order_user_pass", order_user_pass);

            cmd.ExecuteNonQuery();

            SqlDataReader dr;
            cmd = new SqlCommand("SELECT TOP 1 order_id from [Order] order by order_id Desc", Conn); //select出編號最大一筆
            try
            {
                dr = cmd.ExecuteReader(); //執行select
                dr.Read();
                Session["order_id"] = dr["order_id"];
                Order_New_Msg.Text = "訂單已建立，這是第" + dr["order_id"] + "號訂單";
                Order_New.Enabled = false;
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('訂單建立失敗')</script>");
            }
        }
    }
}