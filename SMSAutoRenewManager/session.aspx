<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="session.aspx.vb" Inherits="SMSAutoRenewManager.session" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Session Timeout :(</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>Session has been inactive for at least 20 minutes and has timed out.</p>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RenewalQueue.aspx">Start New Session</asp:HyperLink>
    </div>
    </form>
</body>
</html>
