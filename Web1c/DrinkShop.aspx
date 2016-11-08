﻿<%@ Page Title="歡迎來到飲料店 - 飲料店店面" Language="C#" 
    CodeBehind="DrinkShop.aspx.cs" MasterPageFile="~/Site.master" Inherits="Web1c.DrinkShop" %>
<asp:Content runat="server" ContentPlaceHolderId="MainContent">
    <form id="form1" runat="server">
        <div>

            <asp:Label ID="SessionName" runat="server" Text="讀取你的名字中..."></asp:Label>
            <br>
            <asp:Label ID="SessionPoints" runat="server" Text="讀取你的點數中..."></asp:Label>

            <asp:Button ID="BackToDefault" runat="server" Style="float: right;" PostBackUrl="~/Default.aspx" Text="返回個人頁面" />

        </div>
        <asp:Panel ID="OrderDrinks_SelectDrinks_Panel" runat="server">
            <asp:DropDownList ID="DrinkName_Dropdown" runat="server" AutoPostBack="True"
                Height="30px" Width="167px"
                OnSelectedIndexChanged="DrinkName_Dropdown_SelectedIndexChanged"
                Font-Names="微軟正黑體" Font-Size="15px">
            </asp:DropDownList>
            <br>
            <asp:Label ID="DrinkName_Show" runat="server" Text=""></asp:Label>
            <asp:Label ID="DrinkPrice_Show" runat="server" Text=""></asp:Label>
            <br>
            <asp:Image ID="Drink_Image" runat="server" Height="200px" Width="200px" onerror="this.src = './Images/drinks/no-image-icon-6.png'" />
            <br />
            <asp:Button ID="Order_New" runat="server" OnClick="Order_New_Click" Text="訂購此飲料" />
            <asp:Label ID="Order_New_Msg" runat="server"></asp:Label>
            <br />
            <br />
        </asp:Panel>
        <asp:Panel ID="OrderDrinks_Porperty_Panel" runat="server" Visible="False">
            要訂購
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
            杯，甜度

     
        <asp:DropDownList ID="OrderDrinks_sweet_dropdown" runat="server">
            <asp:ListItem Value="-1">未知</asp:ListItem>
            <asp:ListItem Value="0">無糖</asp:ListItem>
            <asp:ListItem Value="1">1分甜</asp:ListItem>
            <asp:ListItem Value="2">3分甜</asp:ListItem>
            <asp:ListItem Value="3">半糖</asp:ListItem>
            <asp:ListItem Value="4">7分甜</asp:ListItem>
            <asp:ListItem Value="5">全糖</asp:ListItem>
        </asp:DropDownList>
            ，冰塊
     
        <asp:DropDownList ID="OrderDrinks_ice_dropdown" runat="server">
            <asp:ListItem Value="-1">未知</asp:ListItem>
            <asp:ListItem Value="0">去冰</asp:ListItem>
            <asp:ListItem Value="1">少冰</asp:ListItem>
            <asp:ListItem Value="2">加冰</asp:ListItem>
        </asp:DropDownList>
            。
        <asp:Button ID="orderdrink_Item_Insert_Button" runat="server" Text="插入此項目" OnClick="OrderDrinks_Item_Insert_Button_Click" />
            <br />
            <br />
            <br />
            <asp:GridView ID="OrderDrinks_GridView" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" OnRowCancelingEdit="OrderDrinks_Gridview_RowCancelingEdit" OnRowDataBound="OrderDrinks_Gridview_RowDataBound" OnRowDeleting="OrderDrinks_Gridview_RowDeleting" OnRowEditing="OrderDrinks_Gridview_RowEditing" OnRowUpdating="OrderDrinks_Gridview_RowUpdating" OnRowDeleted="OrderDrinks_Gridview_RowDeleted">
                <Columns>
                    <asp:TemplateField HeaderText="名稱" SortExpression="drink_name">
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_drink_id_label" runat="server" Text='<%# formatColumnData("name", Convert.ToInt32(Eval("orderdrink_drink_id"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="數量" SortExpression="orderdrink_no">
                        <EditItemTemplate>
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
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_no_label" runat="server" Text='<%# Eval("orderdrink_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="甜度" SortExpression="orderdrink_sweet">
                        <EditItemTemplate>
                            <asp:DropDownList ID="OrderDrink_Template_sweet_dropdown" runat="server" SelectedValue='<%# Bind("orderdrink_sweet") %>'>
                                <asp:ListItem Value="-1">未知</asp:ListItem>
                                <asp:ListItem Value="0">無糖</asp:ListItem>
                                <asp:ListItem Value="1">1分甜</asp:ListItem>
                                <asp:ListItem Value="2">3分甜</asp:ListItem>
                                <asp:ListItem Value="3">半糖</asp:ListItem>
                                <asp:ListItem Value="4">7分甜</asp:ListItem>
                                <asp:ListItem Value="5">全糖</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_sweet_label" runat="server" Text='<%# formatColumnData("sweet", Convert.ToInt32(Eval("orderdrink_sweet"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="冰度" SortExpression="orderdrink_ice">
                        <EditItemTemplate>
                            <asp:DropDownList ID="OrderDrink_Template_ice_dropdown" runat="server" SelectedValue='<%# Eval("orderdrink_ice") %>'>
                                <asp:ListItem Value="-1">未知</asp:ListItem>
                                <asp:ListItem Value="0">去冰</asp:ListItem>
                                <asp:ListItem Value="1">少冰</asp:ListItem>
                                <asp:ListItem Value="2">加冰</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="OrderDrink_Template_ice_label" runat="server" Text='<%# formatColumnData("ice", Convert.ToInt32(Eval("orderdrink_ice"))) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="orderdrink_item_price" HeaderText="總價" SortExpression="orderdrink_item_price" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="OrderDrinks_total_label" runat="server"></asp:Label>
            <br />
            <asp:Button ID="CheckBuy_Button" runat="server" OnClick="CheckBuy_Button_Click" Text="送交訂單" />
            <asp:Button ID="CancelBuy_Button" runat="server" Text="取消訂單" OnClick="CancelBuy_Button_Click" />
        </asp:Panel>
        <asp:Label ID="Order_status_label" runat="server"></asp:Label>
    </form>
</asp:Content>
