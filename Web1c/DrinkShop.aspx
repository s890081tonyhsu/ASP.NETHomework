<%@ Page Title="首頁" Language="C#" 
    CodeBehind="DrinkShop.aspx.cs" Inherits="Web1c.DrinkShop" %>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:Label ID="SessionName" runat="server" Text="讀取你的名字中..."></asp:Label>
        <br>
        <asp:Label ID="SessionPoints" runat="server" Text="讀取你的點數中..."></asp:Label> 

        <asp:Button ID="BackToDefault" runat="server" style="float: right;" PostBackUrl="~/Default.aspx" Text="返回個人頁面" />

    </div>
    <div>
        <asp:DropDownList ID="DrinkName_Dropdown" runat="server" AutoPostBack="True" 
            Height="30px" Width="167px" 
            OnSelectedIndexChanged="DrinkName_Dropdown_SelectedIndexChanged" 
            Font-Names="微軟正黑體" Font-Size="15px">
        </asp:DropDownList>
        <br>
        <asp:Label ID="DrinkName_Show" runat="server" Text=""></asp:Label>
        <asp:Label ID="DrinkPrice_Show" runat="server" Text=""></asp:Label>
        <br>
        <asp:Image ID="Drink_Image" runat="server"  Height="200px" Width="200px" onerror="this.src = './Images/drinks/no-image-icon-6.png'"/>
        <br />
        <asp:Button ID="Order_New" runat="server" OnClick="Order_New_Click" Text="訂購此飲料" />
        <asp:Label ID="Order_New_Msg" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
