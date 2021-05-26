<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="SMSAutoRenewManager.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Label ID="_messageLabel" runat="server"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text="ALIS UserName" Font-Bold="True"></asp:Label><br />
            <asp:TextBox ID="_userNameTextBox" runat="server"></asp:TextBox><br />
            <asp:Label ID="Label2" runat="server" Text="ALIS Password" Font-Bold="True"></asp:Label><br />
            <asp:TextBox ID="_passwordTextBox" runat="server" TextMode="Password"></asp:TextBox><br /><br />
            <asp:Button ID="_loginButton" runat="server" Text="Login" />
    </div>
 
    
    </form>
    
</body>
</html>
