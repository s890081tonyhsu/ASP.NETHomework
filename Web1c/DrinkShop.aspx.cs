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
        public bool order_session;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["User_Account"] == null) Response.Redirect("./Default.aspx");
            SessionName.Text = Session["User_Account"].ToString();
            SessionPoints.Text = Session["User_Points"] + "元";
            order_session = (Session["order_id"] != null);
            logger.Warn("order_session = " + order_session.ToString());
            Drinks_Item_Dataset();

            DrinkName_Dropdown.DataValueField = "drink_id";
            DrinkName_Dropdown.DataTextField = "drink_name";

            DrinkName_Dropdown.DataSource = ds;
            DrinkName_Dropdown.DataMember = "Drinks";

            if (!IsPostBack)
            {
                DrinkName_Dropdown.DataBind();
                DrinkName_Dropdown.Items.Insert(0, new ListItem("請選擇飲料", ""));
            }
            string DrinkName_Dropdown_menu_list = "";
            foreach(ListItem item in DrinkName_Dropdown.Items)
            {
                if(item.Value != "")DrinkName_Dropdown_menu_list += "<div class='item' data-value="+item.Value+">"+item.Text+"</div>";
            }
            DrinkName_Dropdown_menu.InnerHtml = DrinkName_Dropdown_menu_list;
            ChangeVisibleState();
        }
        protected void Drinks_Item_Dataset()
        {
            try
            {
                Conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT drink_id, drink_name, drink_price, drink_qt FROM Drinks ORDER BY drink_id", Conn);
                da.Fill(ds, "Drinks");
                ds.Tables["Drinks"].PrimaryKey = new DataColumn[] { ds.Tables["Drinks"].Columns["drink_id"] };
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
                DrinkName_Show.Text = drink_name;
                DrinkPrice_Show.Text = drink_price + "元/杯";
                Drink_Image.ImageUrl = "./Images/drinks/" + drink_name + ".jpg";
                Drink_Image.Visible = true;
            }
            else
            {
                DrinkName_Show.Text = "你尚未選擇飲料";
                DrinkPrice_Show.Text = "";
                Drink_Image.Visible = false;
            }
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
                order_session = true;
                Order_New_Msg.Text = "第" + Session["order_id"] + "號訂單";
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
                ChangeVisibleState();
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
        protected string FormatColumnData(string type, int n)
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
            if(User_Money_and_Drink_Qt_Check()) SuccessBuy_Command();
            else CancelBuy_Button_Click(sender, e);
        }
        protected bool User_Money_and_Drink_Qt_Check()
        {
            bool qtyTest = true;
            Dictionary<int, int> DrinkQtyDict = new Dictionary<int, int>();
            int drink_id, drink_qt;
            Drinks_Item_Dataset();
            OrderDrinks_Item_Dataset();
            for (int i = 0; i < ds.Tables["Drinks"].Rows.Count; i++) DrinkQtyDict.Add(Convert.ToInt32(ds.Tables["Drinks"].Rows[i]["drink_id"]), Convert.ToInt32(ds.Tables["Drinks"].Rows[i]["drink_qt"]));

            int sum = 0, user_points = Convert.ToInt32(Session["User_Points"]);

            for (int i = 0; i < ds.Tables["OrderDrinks_with_Price"].Rows.Count; i++)
            {
                drink_id = Convert.ToInt32(ds.Tables["OrderDrinks_with_Price"].Rows[i]["orderdrink_drink_id"].ToString());
                drink_qt = Convert.ToInt32(ds.Tables["OrderDrinks_with_Price"].Rows[i]["orderdrink_no"]);
                sum += Convert.ToInt32(ds.Tables["OrderDrinks_with_Price"].Rows[i]["orderdrink_item_price"].ToString());
                DrinkQtyDict[drink_id] -= drink_qt;
                if(DrinkQtyDict[drink_id] < 0) qtyTest = false;
            }

            if(user_points < sum) return false;
            if (!qtyTest) return false;

            user_points -= sum;
            Session["User_Points"] = user_points.ToString();
            SessionPoints.Text = Session["User_Points"] + "元";

            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET User_Points = @User_Points WHERE (User_Account = @User_Account) AND (User_Password = @User_Password)", Conn);
                SqlParameter account_query = new SqlParameter("@User_Account", Session["User_Account"]);
                cmd.Parameters.Add(account_query);
                SqlParameter password_query = new SqlParameter("@User_Password", Session["User_Password"]);
                cmd.Parameters.Add(password_query);
                SqlParameter point_query = new SqlParameter("@User_Points", user_points);
                cmd.Parameters.Add(point_query);
                cmd.ExecuteNonQuery();

                foreach (KeyValuePair<int, int> entry in DrinkQtyDict)
                {
                    cmd = new SqlCommand("UPDATE Drinks SET drink_qt = @drink_qt WHERE drink_id = @drink_id", Conn);
                    cmd.Parameters.AddWithValue("@drink_id", entry.Key);
                    cmd.Parameters.AddWithValue("@drink_qt", entry.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlE)
            {
                logger.Error(sqlE);
                Response.Write("<script>window.alert('扣款失敗')</script>");
            }
            finally
            {
                Conn.Close();
            }
            return true;
        }
        protected void SuccessBuy_Command()
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET order_status = 2 WHERE order_id = @order_id AND order_user_pass = @order_user_pass", Conn);
                cmd.Parameters.AddWithValue("@order_id", Session["order_id"].ToString());
                cmd.Parameters.AddWithValue("@order_user_pass", Session["User_Password"].ToString());

                cmd.ExecuteNonQuery();
                Session.Remove("order_id");
                order_session = false;
                Order_status_label.Text = "你的訂單已經送出";
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
                ChangeVisibleState();
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
                Session.Remove("order_id");
                order_session = false;
                Order_status_label.Text = "你的訂單已經取消";
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
                ChangeVisibleState();
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
                DropDownList orderdrink_drink_id, orderdrink_no, orderdrink_sweet, orderdrink_ice;
                orderdrink_drink_id = (row.FindControl("OrderDrink_Template_drink_id_dropdown") as DropDownList);
                orderdrink_no = (row.FindControl("OrderDrink_Template_no_dropdown") as DropDownList);
                orderdrink_sweet = (row.FindControl("OrderDrink_Template_sweet_dropdown") as DropDownList);
                orderdrink_ice = (row.FindControl("OrderDrink_Template_ice_dropdown") as DropDownList);

                OrderDrinks_Item_Dataset();
                logger.Info("Index at: "+Convert.ToInt32(e.RowIndex));
                string orderdrink_count = ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_count"].ToString();
                Conn.Open();
                da.UpdateCommand = new SqlCommand("UPDATE OrderDrinks SET orderdrink_drink_id = @orderdrink_drink_id, orderdrink_no = @orderdrink_no, orderdrink_sweet = @orderdrink_sweet, orderdrink_ice = @orderdrink_ice WHERE orderdrink_count = @orderdrink_count", Conn);
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_count", orderdrink_count);
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_drink_id", orderdrink_drink_id.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_no", orderdrink_no.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_sweet", orderdrink_sweet.SelectedValue.ToString());
                da.UpdateCommand.Parameters.AddWithValue("@orderdrink_ice", orderdrink_ice.SelectedValue.ToString());

                ds.Tables["OrderDrinks_with_Price"].Rows[e.RowIndex]["orderdrink_drink_id"] = orderdrink_drink_id.SelectedValue.ToString();
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
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                if((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList drink_id_list = (DropDownList)e.Row.FindControl("OrderDrink_Template_drink_id_dropdown");

                    Drinks_Item_Dataset();
                    drink_id_list.DataSource = ds.Tables["drinks"];
                    drink_id_list.DataTextField = "drink_name";
                    drink_id_list.DataValueField = "drink_id";
                    drink_id_list.DataBind();

                    DataRowView dr = e.Row.DataItem as DataRowView;
                    drink_id_list.SelectedValue = dr["orderdrink_drink_id"].ToString();
                }
            }
            int sum = 0;

            for (int i = 0; i < ds.Tables["OrderDrinks_with_Price"].Rows.Count; i++)
            {
                sum += Convert.ToInt32(ds.Tables["OrderDrinks_with_Price"].Rows[i]["orderdrink_item_price"].ToString());
            }
            OrderDrinks_total_label.Text = "共有" + ds.Tables["OrderDrinks_with_Price"].Rows.Count + "筆項目，總共 " + sum + " 元";
        }

        protected void ChangeVisibleState()
        {
            Order_New.CssClass = order_session ? "ui fluid red disabled button":"ui fluid red button";
            Order_New_Msg.Visible = order_session & IsPostBack;
            OrderDrinks_Order_Panel.Visible = order_session;
            OrderDrinks_Porperty_Panel.Visible = order_session;
            Order_status_label.Visible = !order_session & IsPostBack;
        }
    }
}