<%@ Page Title="歡迎來到飲料店 - 飲料店店面" Language="C#" 
    CodeBehind="DrinkShop.aspx.cs" MasterPageFile="~/Site.master" Inherits="Web1c.DrinkShop" %>
<asp:Content runat="server" ContentPlaceHolderId="MainContent">
    <form id="form1" runat="server">
        <h1 class="ui centered header">歡迎來到飲料店</h1>
        <div ID="User_Detail_Tab" class="ui secondary pointing menu" runat="server">
            <asp:Button ID="BackToDefault" class="item" runat="server" PostBackUrl="~/Default.aspx" Text="個人頁面" />
            <a class="item active">
                進入商店
            </a>
            <div class="right menu">
            </div>
        </div>
        <div class="ui fluid steps">
            <div class="step">
                <i class="user icon"></i>
                <div class="content">
                    <div class="title">你好</div>
                    <asp:Label ID="SessionName" class="description" runat="server" Text="使用者"></asp:Label>
                </div>
            </div>
            <div class="active step">
                <i class="money icon"></i>
                <div class="content">
                    <div class="title">餘額</div>
                    <asp:Label ID="SessionPoints" class="description" runat="server" Text="未知"></asp:Label>
                </div>
            </div>
        </div>
        <asp:Panel ID="OrderDrinks_SelectDrinks_Panel" class="ui two column stackable grid segment" runat="server">
            <div class="column">
                <div class="ui fluid image">
                    <div class="ui black ribbon label">
                        <i class="dollar icon"></i> <asp:Label ID="DrinkPrice_Show" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="ui huge bottom right attached label">
                        <asp:Label ID="DrinkName_Show" runat="server" Text=""></asp:Label>
                    </div>
                    <asp:Image ID="Drink_Image" runat="server" onerror="this.src = './Images/drinks/no-image-icon-6.png'" />
                </div>
                <hr>
            </div>
            <div class="column ui form">
                <div class="field">
                    <div class="ui fluid dropdown selection" tabindex="0">
                        <asp:DropDownList ID="DrinkName_Dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrinkName_Dropdown_SelectedIndexChanged">
                        </asp:DropDownList>
                        <i class="dropdown icon"></i>
                        <div class="default text">請選擇飲料</div>
                        <div ID="DrinkName_Dropdown_menu" class="menu transition hidden" runat="server" tabindex="-1">
                        </div>
                    </div>
                </div>
                <div class="field">
                    <asp:LinkButton ID="Order_New" class="ui fluid red button" runat="server" OnClick="Order_New_Click">訂購此飲料</asp:LinkButton>
                    <asp:Label class="ui basic red pointing label" ID="Order_New_Msg" runat="server"></asp:Label>
                </div>
                <div ID="OrderDrinks_Order_Panel" runat="server">
                    <div class="field">
                        <div class="ui dropdown selection" tabindex="0">
                            <asp:DropDownList ID="OrderDrinks_no_dropdown" runat="server">
                                <asp:ListItem Selected="True">0</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                            <i class="dropdown icon"></i>
                            <div class="default text">數量</div>
                            <div class="menu transition hidden" tabindex="-1">
                                <div class="item" data-value="0">0</div>
                                <div class="item" data-value="1">1</div>
                                <div class="item" data-value="2">2</div>
                                <div class="item" data-value="3">3</div>
                                <div class="item" data-value="4">4</div>
                                <div class="item" data-value="5">5</div>
                                <div class="item" data-value="6">6</div>
                                <div class="item" data-value="7">7</div>
                                <div class="item" data-value="8">8</div>
                                <div class="item" data-value="9">9</div>
                                <div class="item" data-value="10">10</div>
                            </div>
                        </div>
                    </div>
                    <div class="field">
                        <div class="ui dropdown selection" tabindex="0">
                            <asp:DropDownList ID="OrderDrinks_sweet_dropdown" runat="server">
                                <asp:ListItem Value="-1">未知</asp:ListItem>
                                <asp:ListItem Value="0">無糖</asp:ListItem>
                                <asp:ListItem Value="1">1分甜</asp:ListItem>
                                <asp:ListItem Value="2">3分甜</asp:ListItem>
                                <asp:ListItem Value="3">半糖</asp:ListItem>
                                <asp:ListItem Value="4">7分甜</asp:ListItem>
                                <asp:ListItem Value="5">全糖</asp:ListItem>
                            </asp:DropDownList>
                            <i class="dropdown icon"></i>
                            <div class="default text">甜度</div>
                            <div class="menu transition hidden" tabindex="-1">
                                <div class="item" data-value="-1">未知</div>
                                <div class="item" data-value="0">無糖</div>
                                <div class="item" data-value="1">1分甜</div>
                                <div class="item" data-value="2">3分甜</div>
                                <div class="item" data-value="3">半糖</div>
                                <div class="item" data-value="4">7分甜</div>
                                <div class="item" data-value="5">全糖</div>
                            </div>
                        </div>
                    </div>
                    <div class="field">
                        <div class="ui dropdown selection" tabindex="0">
                            <asp:DropDownList ID="OrderDrinks_ice_dropdown" runat="server">
                                <asp:ListItem Value="-1">未知</asp:ListItem>
                                <asp:ListItem Value="0">去冰</asp:ListItem>
                                <asp:ListItem Value="1">少冰</asp:ListItem>
                                <asp:ListItem Value="2">加冰</asp:ListItem>
                            </asp:DropDownList>
                            <i class="dropdown icon"></i>
                            <div class="default text">冰塊</div>
                            <div class="menu transition hidden" tabindex="-1">
                                <div class="item" data-value="-1">未知</div>
                                <div class="item" data-value="0">去冰</div>
                                <div class="item" data-value="1">少冰</div>
                                <div class="item" data-value="2">加冰</div>
                            </div>
                        </div>
                    </div>
                    <div class="field">
                        <asp:Button ID="orderdrink_Item_Insert_Button" class="ui fluid green button" runat="server" Text="插入此項目" OnClick="OrderDrinks_Item_Insert_Button_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="OrderDrinks_Porperty_Panel" class="ui segment" runat="server">
            <asp:GridView ID="OrderDrinks_GridView" class="ui celled table" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="OrderDrinks_Gridview_RowCancelingEdit" OnRowDataBound="OrderDrinks_Gridview_RowDataBound" OnRowDeleting="OrderDrinks_Gridview_RowDeleting" OnRowEditing="OrderDrinks_Gridview_RowEditing" OnRowUpdating="OrderDrinks_Gridview_RowUpdating" OnRowDeleted="OrderDrinks_Gridview_RowDeleted" GridLines="None">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="rowDelete" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex%>' runat="server" CssClass="ui icon red button">
                                <i class="remove icon"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="rowEdit" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex%>' runat="server" CssClass="ui icon blue button">
                                <i class="edit icon"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="rowUpdate" CommandName="Update" CommandArgument='<%# Container.DataItemIndex%>' runat="server" CssClass="ui icon green button">
                                <i class="save icon"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="rowCancelEdit" CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex%>' runat="server" CssClass="ui icon teal button">
                                <i class="stop icon"></i>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="名稱" SortExpression="drink_name">
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_drink_id_label" runat="server" Text='<%# FormatColumnData("name", Convert.ToInt32(Eval("orderdrink_drink_id"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="數量" SortExpression="orderdrink_no">
                        <EditItemTemplate>
                            <div class="ui dropdown selection" tabindex="0">
                                <asp:DropDownList ID="OrderDrink_Template_no_dropdown" runat="server" SelectedValue='<%# Bind("orderdrink_no") %>'>
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>6</asp:ListItem>
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>8</asp:ListItem>
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                </asp:DropDownList>
                                <i class="dropdown icon"></i>
                                <div class="default text">數量</div>
                                <div class="menu transition hidden" tabindex="-1">
                                    <div class="item" data-value="0">0</div>
                                    <div class="item" data-value="1">1</div>
                                    <div class="item" data-value="2">2</div>
                                    <div class="item" data-value="3">3</div>
                                    <div class="item" data-value="4">4</div>
                                    <div class="item" data-value="5">5</div>
                                    <div class="item" data-value="6">6</div>
                                    <div class="item" data-value="7">7</div>
                                    <div class="item" data-value="8">8</div>
                                    <div class="item" data-value="9">9</div>
                                    <div class="item" data-value="10">10</div>
                                </div>
                            </div>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_no_label" runat="server" Text='<%# Eval("orderdrink_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="甜度" SortExpression="orderdrink_sweet">
                        <EditItemTemplate>
                            <div class="ui dropdown selection" tabindex="0">
                                <asp:DropDownList ID="OrderDrink_Template_sweet_dropdown" runat="server" SelectedValue='<%# Bind("orderdrink_sweet") %>'>
                                    <asp:ListItem Value="-1">未知</asp:ListItem>
                                    <asp:ListItem Value="0">無糖</asp:ListItem>
                                    <asp:ListItem Value="1">1分甜</asp:ListItem>
                                    <asp:ListItem Value="2">3分甜</asp:ListItem>
                                    <asp:ListItem Value="3">半糖</asp:ListItem>
                                    <asp:ListItem Value="4">7分甜</asp:ListItem>
                                    <asp:ListItem Value="5">全糖</asp:ListItem>
                                </asp:DropDownList>
                                <i class="dropdown icon"></i>
                                <div class="default text">甜度</div>
                                <div class="menu transition hidden" tabindex="-1">
                                    <div class="item" data-value="-1">未知</div>
                                    <div class="item" data-value="0">無糖</div>
                                    <div class="item" data-value="1">1分甜</div>
                                    <div class="item" data-value="2">3分甜</div>
                                    <div class="item" data-value="3">半糖</div>
                                    <div class="item" data-value="4">7分甜</div>
                                    <div class="item" data-value="5">全糖</div>
                                </div>
                            </div>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_sweet_label" runat="server" Text='<%# FormatColumnData("sweet", Convert.ToInt32(Eval("orderdrink_sweet"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="冰度" SortExpression="orderdrink_ice">
                        <EditItemTemplate>
                            <div class="ui dropdown selection" tabindex="0">
                                <asp:DropDownList ID="OrderDrink_Template_ice_dropdown" runat="server" SelectedValue='<%# Eval("orderdrink_ice") %>'>
                                    <asp:ListItem Value="-1">未知</asp:ListItem>
                                    <asp:ListItem Value="0">去冰</asp:ListItem>
                                    <asp:ListItem Value="1">少冰</asp:ListItem>
                                    <asp:ListItem Value="2">加冰</asp:ListItem>
                                </asp:DropDownList>
                                <i class="dropdown icon"></i>
                                <div class="default text">冰塊</div>
                                <div class="menu transition hidden" tabindex="-1">
                                    <div class="item" data-value="-1">未知</div>
                                    <div class="item" data-value="0">去冰</div>
                                    <div class="item" data-value="1">少冰</div>
                                    <div class="item" data-value="2">加冰</div>
                                </div>
                            </div>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_ice_label" runat="server" Text='<%# FormatColumnData("ice", Convert.ToInt32(Eval("orderdrink_ice"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="orderdrink_item_price" HeaderText="總價" SortExpression="orderdrink_item_price" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="OrderDrinks_total_label" class="ui message" runat="server" Text="請完成你的訂購"></asp:Label>
            <div class="ui buttons">
              <asp:Button ID="CancelBuy_Button" class="ui button" runat="server" Text="取消訂單" OnClick="CancelBuy_Button_Click" />
              <div class="or"></div>
              <asp:Button ID="CheckBuy_Button" class="ui positive button" runat="server" OnClick="CheckBuy_Button_Click" Text="送交訂單" />
            </div>
        </asp:Panel>
        <asp:Label ID="Order_status_label" class="ui message" runat="server"></asp:Label>
    </form>
</asp:Content>
