

<%@ Page Title="歡迎來到飲料店 - 用戶頁面" Language="C#" 
    CodeBehind="Default.aspx.cs" MasterPageFile="~/Site.master" Inherits="Web1c._Default" %>
<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderId="MainContent">
    <form id="form1" runat="server">
        <h1 class="ui centered header">歡迎來到飲料店</h1>
        <div class="ui two column centered grid">
            <div ID="Login_Form" class="ui large form" runat="server">
                <div class="ui stacked segment">
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="user icon"></i>
                            <asp:TextBox ID="Account_Input" runat="server" placeholder="請輸入帳號"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="lock icon"></i>
                            <asp:TextBox ID="Password_Input" runat="server" TextMode="Password" placeholder="請輸入密碼"></asp:TextBox>
                        </div>
                    </div>
                    <asp:button ID="Login_Input" class="ui fluid large teal submit button" runat="server" text="登入" onclick="Login_Input_Click" />
                </div>
                <div class="ui error message">
                </div>
            </div>
        </div>
        <div ID="loginMessage" class="ui message" runat="server">
            <i class="close icon"></i>
            <asp:Label ID="loginStatusLabel" runat="server"></asp:Label>
            <asp:Label ID="Yield_Label" class="" runat="server" Text=""></asp:Label>
        </div> 
        <div ID="User_Detail_Tab" class="ui secondary pointing menu" runat="server">
            <a class="item active">
                個人資料
            </a>
            <asp:Button ID="EnterStore_Input" class="item" runat="server" PostBackUrl="~/DrinkShop.aspx" Text="進入商店" />
            <div class="right menu">
                <asp:Button ID="Logout_Input" class="ui item" runat="server" OnClick="Logout_Input_Click" Text="登出" />
            </div>
        </div>
        <div ID="User_Detail_Seg" class="ui segment" runat="server">
            <asp:DetailsView ID="UserDetailView" runat="server" AutoGenerateRows="False" CssClass="ui very basic table" BorderStyle="None" GridLines="None">
                <EmptyDataTemplate>
                    尚未登入或帳號密碼錯誤
                </EmptyDataTemplate>
                <Fields>
                    <asp:BoundField DataField="User_Account" HeaderText="帳號名稱" SortExpression="User_Account" />
                    <asp:BoundField DataField="User_Password" HeaderText="密碼" SortExpression="User_Password" Visible="False" />
                    <asp:BoundField DataField="User_Points" HeaderText="點數" SortExpression="User_Points" />
                    <asp:BoundField DataField="User_Email" HeaderText="E-Mail地址" SortExpression="User_Email" />
                </Fields>
            </asp:DetailsView>
        </div>
    </form>
</asp:Content>

