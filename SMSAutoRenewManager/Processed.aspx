<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Processed.aspx.vb" Inherits="SMSAutoRenewManager.Processed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SMS Processed Auto-Renewals</title>
    
    <link href="Content/main.css" rel="stylesheet" type="text/css" />
    <link href="Content/pistachio.css" rel="stylesheet" type="text/css" />
    <link href="Content/site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Content/jquery-1.4.1.min.js"></script>
    
    <script type="text/javascript">
       $(document).ready(function() {
          //load menu options
          var currentDate = new Date();

          var endWeek = new Date();
          endWeek.setDate(currentDate.getDate()+7);
          var endMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
          
          var expiredEndDate = currentDate.getMonth()+1 + '/' + currentDate.getDate() + '/' + currentDate.getFullYear();
          var weekEndDate = endWeek.getMonth()+1 + '/' + endWeek.getDate() + '/' + endWeek.getFullYear();
          var monthEndDate = endMonth.getMonth()+1 + '/' + endMonth.getDate() + '/' + endMonth.getFullYear();

          
          $('#menuExpired').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + expiredEndDate + '">Expired</a>');
          $('#menuWeek').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + weekEndDate + '">Expire This Week</a>');
          $('#menuMonth').html('<a href="renewalqueue.aspx?startdate=1/1/2000&enddate=' + monthEndDate + '">Expire This Month</a>');
        });
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <div class="header">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <img alt="Labelmaster" longdesc="Labelmaster logo" src="Images/logo_lmmini.png" 
                            style="width: 132px; height: 25px" class="lmlogo" /><br />
                        <h2 class="appTitle">SMS Auto-Renewal Queue</h2>
                    </td>
                    <td>
                        <div class="login">
                            <table>
                            <tr>
                                <td width="50">
                                    <asp:Image ID="_avatarImage" runat="server" CssClass="avatar" Width="45" Height="46" />
                                </td>
                                <td>
                                    <asp:Label ID="_loginMessageLabel" runat="server" CssClass="loginMessage"></asp:Label><br />
                                    <asp:Label ID="_signedInLabel" runat="server"></asp:Label>
                                    <asp:LinkButton ID="_loginLinkButton" runat="server" class="loginButton">Sign Out</asp:LinkButton>
                                </td>
                            </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="menubar">
            <ul>
                <li>Pending Renewals</li>
                     <ul>
                        <li id="menuExpired"><a href="renewalqueue.aspx">Expired</a></li>
                        <li id="menuWeek"><a href="renewalqueue.aspx">Expire This Week</a></li>
                        <li id="menuMonth"><a href="renewalqueue.aspx">Expire This Month</a></li>
                     </ul>
                <li class="selected">Processed</li>
                <li><a href="removed.aspx">Removed Renewals</a></li>
            </ul>
        </div> 
        
        <div class="innerBody">
                <asp:Label ID="_pageHeading" runat="server" Text="Processed Renewals" CssClass="pageHeading"></asp:Label>
                <asp:GridView ID="_processedGridView" runat="server" GridLines="None" HeaderStyle-CssClass="dataTableCaption" AlternatingRowStyle-CssClass="dataTableRowAlternating" RowStyle-CssClass="dataTableRow" 
                    AutoGenerateColumns="false" AllowPaging="true" PageSize="25" OnPageIndexChanging="_processedGridView_PageIndexChanging" PagerStyle-CssClass="dataTablePager">


                    <Columns>
                        <asp:BoundField DataField="SubscriptionID" HeaderText="Subscription ID" />
                        <asp:BoundField DataField="DateProcessed" HeaderText="Date Processed" />
                        <asp:BoundField DataField="ProcessedByUserName" HeaderText = "Processed By" />
                        <asp:BoundField DataField="AlisOrderRef" HeaderText="ALIS Order Number" />
                        
                    </Columns>
   

<HeaderStyle CssClass="dataTableCaption"></HeaderStyle>

<AlternatingRowStyle CssClass="dataTableRowAlternating"></AlternatingRowStyle>
                </asp:GridView>
        </div>
        
        
    
    </div>
    </form>
</body>
</html>
