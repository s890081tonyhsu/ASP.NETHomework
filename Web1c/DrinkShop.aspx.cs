using System;
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
    public partial class DrinkShop : System.Web.UI.Page
    {
        const string Web1cConnectionString = "Data Source=127.0.0.1,9061;Initial Catalog=Web1c;Integrated Security=True";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(); // for OrderDrinks
        SqlConnection Conn = new SqlConnection(Web1cConnectionString);
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["User_Account"] == null) Response.Redirect("./Default.aspx");
            SessionName.Text = Session["User_Account"] + "歡迎光臨";
            SessionPoints.Text = "目前你還有" + Session["User_Points"] + "元";
            try
            {
                Conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT drink_id, drink_name, drink_price FROM Drinks ORDER BY drink_id", Conn);
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
                logger.Error(sqlE);
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
                logger.Error(sqlE);
                Response.Write("<script>window.alert('訂單建立失敗')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_Dataset();
            }
        }
        protected void OrderDrinks_Item_Dataset()
        {
            try
            {
                Conn.Open();
                da.SelectCommand = new SqlCommand("SELECT OrderDrinks.orderdrink_count, OrderDrinks.orderdrink_drink_id, OrderDrinks.orderdrink_no, OrderDrinks.orderdrink_sweet, OrderDrinks.orderdrink_ice, Drinks.drink_price * OrderDrinks.orderdrink_no AS orderdrink_item_price FROM Drinks INNER JOIN OrderDrinks ON Drinks.drink_id = OrderDrinks.orderdrink_drink_id WHERE OrderDrinks.orderdrink_order_id = @orderdrink_order_id", Conn);
                da.SelectCommand.Parameters.AddWithValue("@orderdrink_order_id", Session["order_id"]);
                da.Fill(ds, "OrderDrinks_with_Price"); //執行select
                ds.Tables["OrderDrinks_with_Price"].PrimaryKey = new DataColumn[] { ds.Tables["OrderDrinks_with_Price"].Columns["orderdrink_count"] };
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('項目查詢失敗')</script>");
            }
            finally
            {
                Conn.Close();                
            }
        }
        protected void OrderDrinks_Item_GridViewSet()
        {
            OrderDrinks_GridView.DataSource = ds;
            OrderDrinks_GridView.DataMember = "OrderDrinks_with_Price";
            OrderDrinks_GridView.DataBind();            
        }
        protected void OrderDrinks_Item_Insert_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();
                da.InsertCommand = new SqlCommand("INSERT INTO [OrderDrinks](orderdrink_order_id, orderdrink_no, orderdrink_drink_id, orderdrink_sweet, orderdrink_ice) VALUES (@orderdrink_order_id, @orderdrink_no, @orderdrink_drink_id, @orderdrink_sweet, @orderdrink_ice)", Conn);
                da.InsertCommand.Parameters.AddWithValue("@orderdrink_order_id", Session["order_id"]);
                da.InsertCommand.Parameters.AddWithValue("@orderdrink_no", OrderDrinks_no_dropdown.SelectedValue);
                da.InsertCommand.Parameters.AddWithValue("@orderdrink_drink_id", DrinkName_Dropdown.SelectedValue);
                da.InsertCommand.Parameters.AddWithValue("@orderdrink_sweet", OrderDrinks_sweet_dropdown.SelectedValue);
                da.InsertCommand.Parameters.AddWithValue("@orderdrink_ice", OrderDrinks_ice_dropdown.SelectedValue);

                da.InsertCommand.ExecuteNonQuery();
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('項目新增失敗 "+sqlE+"')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_Dataset();
                OrderDrinks_Item_GridViewSet();
            }
        }
        protected string formatColumnData(string type, int n)
        {
            string res = "未知";
            string[] sweet = { "無糖", "1分甜", "3分甜", "半糖", "7分甜", "全糖" };
            string[] ice = { "去冰", "少冰", "加冰" };
            if (n == -1) return res;
            if (type == "sweet") return sweet[n];
            if (type == "ice") return ice[n];
            if (type == "name")
            {
                DataRow rowFind = ds.Tables["Drinks"].Rows.Find(n);
                return rowFind.Field<string>("drink_name");
            }
            return res;
        }

        protected void CheckBuy_Button_Click(object sender, EventArgs e)
        {
            try
            {
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
                logger.Error(sqlE);
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
                logger.Error(sqlE);
                Response.Write("<script>window.alert('訂單取消失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void OrderDrinks_Gridview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                OrderDrinks_Item_Dataset();
                logger.Info("Index at: "+Convert.ToInt32(e.RowIndex));
                string orderdrink_count = ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_count"].ToString();
                Conn.Open();
                da.DeleteCommand = new SqlCommand("DELETE FROM OrderDrinks WHERE orderdrink_count = @orderdrink_count", Conn);
                da.DeleteCommand.Parameters.AddWithValue("@orderdrink_count", orderdrink_count);

                ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex].Delete();
                da.Update(ds, "OrderDrinks_with_Price");
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('項目刪除失敗')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_GridViewSet();
                logger.Info("At deleting, OrderDrinks_with_Price has " + ds.Tables["OrderDrinks_with_Price"].Rows.Count + " rows.");
            }
        }

        protected void OrderDrinks_Gridview_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            logger.Info("At deleted, OrderDrinks_with_Price has " + ds.Tables["OrderDrinks_with_Price"].Rows.Count + " rows.");
            OrderDrinks_Item_Dataset();
            OrderDrinks_Item_GridViewSet();
        }

        protected void OrderDrinks_Gridview_RowEditing(object sender, GridViewEditEventArgs e)
        {
            OrderDrinks_Item_Dataset();
            OrderDrinks_GridView.EditIndex = e.NewEditIndex;
            OrderDrinks_Item_GridViewSet();
        }

        protected void OrderDrinks_Gridview_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            OrderDrinks_Item_Dataset();
            OrderDrinks_GridView.EditIndex = -1;
            OrderDrinks_Item_GridViewSet();
        }

        protected void OrderDrinks_Gridview_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = OrderDrinks_GridView.Rows[e.RowIndex];
                DropDownList orderdrink_no, orderdrink_sweet, orderdrink_ice;
                orderdrink_no = (row.FindControl("OrderDrink_Template_no_dropdown") as DropDownList);
                orderdrink_sweet = (row.FindControl("OrderDrink_Template_sweet_dropdown") as DropDownList);
                orderdrink_ice = (row.FindControl("OrderDrink_Template_ice_dropdown") as DropDownList);

                OrderDrinks_Item_Dataset();
                logger.Info("Index at: "+Convert.ToInt32(e.RowIndex));
                string orderdrink_count = ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_count"].ToString();
                Conn.Open();
                da.UpdateCommand = new SqlCommand("UPDATE OrderDrinks SET orderdrink_no = @orderdrink_no, orderdrink_sweet = @orderdrink_sweet, orderdrink_ice = @orderdrink_ice WHERE orderdrink_count = @orderdrink_count", Conn);
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_count", orderdrink_count);
                //da.UpdateCommand.Parameters.AddWithValue("@orderdrink_drink_id", orderdrink_drink_id.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_no", orderdrink_no.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_sweet", orderdrink_sweet.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_ice", orderdrink_ice.SelectedValue.ToString());

                //ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_drink_id"] = orderdrink_drink_id.SelectedValue.ToString();
                ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_no"] = orderdrink_no.SelectedValue.ToString();
                ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_sweet"] = orderdrink_sweet.SelectedValue.ToString();
                ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_ice"] = orderdrink_ice.SelectedValue.ToString();

                da.Update(ds, "OrderDrinks_with_Price");
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('選項更改失敗')</script>");
            }
            finally
            {
                Conn.Close();
                OrderDrinks_Item_Dataset();
                OrderDrinks_GridView.EditIndex = -1;
                OrderDrinks_Item_GridViewSet();
            }
        }

        protected void OrderDrinks_Gridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            int sum = 0;

            for (int i = 0; i < ds.Tables["OrderDrinks_with_Price"].Rows.Count; i++)
            {
                sum += Convert.ToInt32(ds.Tables["OrderDrinks_with_Price"].Rows[i]["orderdrink_item_price"].ToString());
            }
            OrderDrinks_total_label.Text = "共有" + ds.Tables["OrderDrinks_with_Price"].Rows.Count + "筆項目，總共 " + sum + " 元";
        }
    }
}