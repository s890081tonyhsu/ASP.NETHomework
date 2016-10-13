<%@ Page Title="首頁" Language="C#" 
    CodeBehind="Default.aspx.cs" Inherits="Web1c._Default" %>

 <body background="Images/FreeVectorDrinks.jpg" 
    style="background-repeat: no-repeat;background-position: 50% 50%;position:fixed;">
 <div style="position: fixed;top: 0px;left: 0px;width: 100%; height: 100%; background-color: rgba(255, 255, 255, .5);">
    <h2>
        歡迎使用 飲料店
    </h2>

 <form id="form1" runat="server">
 <table style="width: 100%;">
     <tr>
         <td class="style1">
            <asp:Label ID="Account_Label" AssociatedControlId="Account_Input" runat="server" Text="Label">帳號</asp:Label>
         </td>
         <td>
             <asp:TextBox ID="Account_Input" runat="server" style="height: 19px"></asp:TextBox>
             <asp:RegularExpressionValidator ID="regexpAccount" runat="server"         
                                    ErrorMessage="Incorrect Account Format，請不要輸入非英文數字之文字" 
                                    ControlToValidate="Account_Input"         
                                    ValidationExpression="^[\dA-Za-z]*$" />
         </td>
     </tr>
     <tr>
         <td class="style1">
             <asp:Label ID="Password_Label" AssociatedControlId="Password_Input" runat="server" Text="Label">密碼</asp:Label>
         </td>
         <td>
             <asp:TextBox ID="Password_Input" runat="server"></asp:TextBox>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"         
                                    ErrorMessage="Incorrect Password Format，請不要輸入非英文數字之文字" 
                                    ControlToValidate="Password_Input"         
                                    ValidationExpression="^[\dA-Za-z]*$" />
         </td>
     </tr>
 </table>
      <asp:button ID="Login_Input" runat="server" text="登入" onclick="Login_Input_Click" />
     <asp:Button ID="Logout_Input" runat="server" OnClick="Logout_Input_Click" Text="登出" />
     <asp:Label ID="loginStatusLabel" runat="server"></asp:Label>
     <asp:Label ID="Yield_Label" runat="server" Text="WTF"></asp:Label>
     <br />
     <br />
     <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="SqlDataSource1" Height="20px" Width="573px">
         <EmptyDataTemplate>
             尚未登入或帳號密碼錯誤
         </EmptyDataTemplate>
         <Fields>
             <asp:BoundField DataField="Web_Account" HeaderText="Web_Account" SortExpression="Web_Account" />
             <asp:BoundField DataField="Web_Password" HeaderText="Web_Password" SortExpression="Web_Password" />
             <asp:BoundField DataField="Web_Points" HeaderText="Web_Points" SortExpression="Web_Points" />
             <asp:BoundField DataField="Web_Email" HeaderText="Web_Email" SortExpression="Web_Email" />
         </Fields>
     </asp:DetailsView>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Web1cConnectionString %>" SelectCommand="SELECT [Web_Account], [Web_Password], [Web_Points], [Web_Email] FROM [Table]" onselecting="SqlDataSource1_Selecting">
     </asp:SqlDataSource>
</form>

</div>
</body>
