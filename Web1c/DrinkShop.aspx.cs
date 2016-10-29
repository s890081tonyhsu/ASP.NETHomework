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
            if(Session["User_Account"] == null) Response.Redirect("./Default.aspx");
            SessionName.Text = Session["User_Account"] + "歡迎光臨";
            SessionPoints.Text = "目前你還有" + Session["User_Points"] + "元";
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT drink_id, drink_name, drink_price FROM Drinks ORDER BY drink_id", Conn);
                ds = new DataSet();
                da.Fill(ds, "Drinks");
                ds.Tables["Drinks"].PrimaryKey = new DataColumn[] { ds.Tables["Drinks"].Columns["drink_id"] };

                DrinkName_Dropdown.DataValueField = "drink_id";
                DrinkName_Dropdown.DataTextField = "drink_name";

                DrinkName_Dropdown.DataSource = ds.Tables["Drinks"].DefaultView;
                if (!IsPostBack)
                {
                    DrinkName_Dropdown.DataBind();
                    ListItem item = new ListItem("請選擇飲料", "0");
                    DrinkName_Dropdown.Items.Insert(0, item);
                }
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('飲料清單讀取失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }  
        }

        protected void DrinkName_Dropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string drink_id = DrinkName_Dropdown.SelectedItem.Value;
            DataRow findSelected = ds.Tables["Drinks"].Rows.Find(drink_id);
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
            Order_New.Enabled = true;
            Order_New_Msg.Text = "";
        }

        protected void Order_New_Click(object sender, EventArgs e)
        {
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [Orders]( order_time, order_user_pass) VALUES( GETDATE(), @order_user_pass);", Conn);
                cmd.Parameters.AddWithValue("@order_user_pass", Session["User_Password"].ToString());

                cmd.ExecuteNonQuery();

                SqlDataReader dr;
                cmd = new SqlCommand("SELECT TOP 1 order_id from [Orders] order by order_id Desc", Conn); //select出編號最大一筆

                dr = cmd.ExecuteReader(); //執行select
                dr.Read();
                Session["order_id"] = dr["order_id"];
                Order_New_Msg.Text = "訂單已建立，這是第" + dr["order_id"] + "號訂單";
                Session["order_id"] = dr["order_id"];
                Order_New.Enabled = false;
                OrderDrinks_Porperty_Panel.Visible = true;
                Order_status_label.Text = "";
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('訂單建立失敗')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_View();
            }
        }
        protected void OrderDrinks_Item_View()
        {
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT Drinks.drink_name, OrderDrinks.orderdrink_no, OrderDrinks.orderdrink_sweet, OrderDrinks.orderdrink_ice, Drinks.drink_price * OrderDrinks.orderdrink_no AS orderdrink_total_price FROM Drinks INNER JOIN OrderDrinks ON Drinks.drink_id = OrderDrinks.orderdrink_drink_id WHERE OrderDrinks.orderdrink_order_id = @orderdrink_order_id", Conn); // 從OrderDrinks和Drinks抓資料來組合
                da.SelectCommand.Parameters.AddWithValue("@orderdrink_order_id", Session["order_id"]);
                ds = new DataSet();

                da.Fill(ds, "OrderDrinks_with_Price"); //執行select
                OrderDrinks_total_label.Text = "共有" + ds.Tables["OrderDrinks_with_Price"].Rows.Count + "筆資料";
                OrderDrinks_GridView.DataSource = ds;
                OrderDrinks_GridView.DataMember = "OrderDrinks_with_Price";
                OrderDrinks_GridView.DataBind();
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('項目查詢失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }
        }
        protected void OrderDrinks_Item_Insert_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO [OrderDrinks](orderdrink_order_id, orderdrink_no, orderdrink_drink_id, orderdrink_sweet, orderdrink_ice) VALUES (@orderdrink_order_id, @orderdrink_no, @orderdrink_drink_id, @orderdrink_sweet, @orderdrink_ice)", Conn);
                cmd.Parameters.AddWithValue("@orderdrink_order_id", Session["order_id"]);
                cmd.Parameters.AddWithValue("@orderdrink_no", OrderDrinks_no_dropdown.SelectedValue);
                cmd.Parameters.AddWithValue("@orderdrink_drink_id", DrinkName_Dropdown.SelectedValue);
                cmd.Parameters.AddWithValue("@orderdrink_sweet", OrderDrinks_sweet_dropdown.SelectedValue);
                cmd.Parameters.AddWithValue("@orderdrink_ice", OrderDrinks_ice_dropdown.SelectedValue);

                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('項目新增失敗 "+sqlE+"')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_View();
            }
        }
        protected string formatColumnData(string type, int n)
        {
            string res = "未知";
            string[] sweet = { "1分甜", "3分甜", "半糖", "7分甜", "全糖" };
            string[] ice = { "去冰", "少冰", "加冰" };
            if (n == -1) return res;
            if (type == "sweet") return sweet[n];
            if (type == "ice") return ice[n];
            return res;
        }

        protected void CheckBuy_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET order_status = 2 WHERE order_id = @order_id AND order_user_pass = @order_user_pass", Conn);
                cmd.Parameters.AddWithValue("@order_id", Session["order_id"].ToString());
                cmd.Parameters.AddWithValue("@order_user_pass", Session["User_Password"].ToString());

                cmd.ExecuteNonQuery();
                Order_status_label.Text = "你的訂單已經送出";
                Order_New.Enabled = true;
                OrderDrinks_Porperty_Panel.Visible = false;
                Session.Remove("order_id");
                Order_New_Msg.Text = "";
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('訂單確認失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void CancelBuy_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Conn = new SqlConnection(Web1cConnectionString);
                Conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET order_status = 0 WHERE order_id = @order_id AND order_user_pass = @order_user_pass", Conn);
                cmd.Parameters.AddWithValue("@order_id", Session["order_id"].ToString());
                cmd.Parameters.AddWithValue("@order_user_pass", Session["User_Password"].ToString());

                cmd.ExecuteNonQuery();
                Order_status_label.Text = "你的訂單已經取消";
                Order_New.Enabled = true;
                OrderDrinks_Porperty_Panel.Visible = false;
                Session.Remove("order_id");
                Order_New_Msg.Text = "";
            }
            catch (SqlException sqlE)
            {
                Response.Write("<script>window.alert('訂單取消失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}