<%@ Page Title="首頁" Language="C#" 
    CodeBehind="DrinkShop.aspx.cs" Inherits="Web1c.DrinkShop" %>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:Label ID="SessionName" runat="server" Text="讀取你的名字中..."></asp:Label>
        <br>
        <asp:Label ID="SessionPoints" runat="server" Text="讀取你的點數中..."></asp:Label> 

    </div>
    <div>
        <asp:DropDownList ID="DrinkName_Dropdown" runat="server" AutoPostBack="True" 
            Height="30px" Width="167px" 
            OnSelectedIndexChanged="DrinkName_Dropdown_SelectedIndexChanged" 
            Font-Names="微軟正黑體" Font-Size="15px">
        </asp:DropDownList>
        <asp:Button ID="BackToDefault" runat="server" PostBackUrl="~/Default.aspx" Text="返回個人頁面" />
        <br>
        <asp:Label ID="DrinkName_Show" runat="server" Text=""></asp:Label>
        <asp:Label ID="DrinkPrice_Show" runat="server" Text=""></asp:Label>
        <br>
        <asp:Image ID="Drink_Image" runat="server"  Height="200px" Width="200px" onerror="this.src = './Images/drinks/no-image-icon-6.png'"/>
    </div>
    </form>
</body>
</html>
