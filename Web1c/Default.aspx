<%@ Page Title="歡迎來到飲料店 - 用戶頁面" Language="C#" 
    CodeBehind="Default.aspx.cs" MasterPageFile="~/Site.master" Inherits="Web1c._Default" %>
<asp:Content runat="server" ContentPlaceHolderId="MainContent">
 <form id="form1" class="ui large form" runat="server">
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
     <asp:Button ID="Logout_Input" class="ui fluid large red submit button" runat="server" OnClick="Logout_Input_Click" Text="登出" />
     <asp:Button ID="EnterStore_Input" class="ui fluid large teal submit button" runat="server" PostBackUrl="~/DrinkShop.aspx" Text="進入商店" Visible="False"></asp:Button>
 </div>
<div class="ui error message">
</div>
     <br>
     <asp:Label ID="loginStatusLabel" runat="server"></asp:Label>
     <asp:Label ID="Yield_Label" runat="server" Text="WTF"></asp:Label>
     <br />
     <br />
     <asp:DetailsView ID="UserDetailView" runat="server" AutoGenerateRows="False" Height="20px" Width="573px">
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
</form>
</asp:Content>