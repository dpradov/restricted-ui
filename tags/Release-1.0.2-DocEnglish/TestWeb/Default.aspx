<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="TestWeb._Default" %>

<%@ Register src="WebUserControl1.ascx" tagname="WebUserControl1" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Página sin título</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="DIV1">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <asp:GridView ID="GridView1" runat="server" 
        DataSourceID="ObjectDataSource1">
        </asp:GridView>
    </div>
        <asp:Panel ID="Panel1" runat="server" Height="112px" Width="272px">
            <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                <asp:ListItem Selected="True" Value="2">Dos</asp:ListItem>
                <asp:ListItem Value="4">Cuatro</asp:ListItem>
                <asp:ListItem Value="8">Ocho</asp:ListItem>
            </asp:CheckBoxList></asp:Panel>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem Value="A">AAAAAAA</asp:ListItem>
            <asp:ListItem Value="B">BBBBBBBB</asp:ListItem>
            <asp:ListItem Value="C">CCCCCCCCC</asp:ListItem>
        </asp:DropDownList>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="getDatos" TypeName="TestWeb.Entity"></asp:ObjectDataSource>
    <uc1:WebUserControl1 ID="WebUserControl11" runat="server" />
    </form>
</body>
</html>
