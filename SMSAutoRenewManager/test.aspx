<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="test.aspx.vb" Inherits="SMSAutoRenewManager.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SMS Auto-Renewal Queue</title>
    
    <link href="Content/main.css" rel="stylesheet" type="text/css" />
    <link href="Content/pistachio.css" rel="stylesheet" type="text/css" />
    <link href="Content/site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Content/jquery-1.4.1.min.js"></script>
    

    <script type="text/javascript" >
        $(document).ready(function(){
        
        //var obj = {"product":{ "name":"IMO0009" }};
        
$.ajax({
        url: 'ajax/queuehtmltable.aspx',
	    data: 'date1=1/1/1980&date2=1/1/2010',
        beforeSend: function() { $('#queueThrobber').show(); },
        complete: function() { $('#queueThrobber').hide(); },
        success: function(html){
            $('#renewals').append(html);
            }
    });
            
            
        
                
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
                                    <img src="Images/avatar_default.png" width="46" height="46" class="avatar" />
                                </td>
                                <td>
                                    <asp:Label ID="_loginMessageLabel" runat="server" Text="Hello, David" CssClass="loginMessage"></asp:Label><br />
                                    <asp:Label ID="_signedInLabel" runat="server" Text="Signed in as DAVID H"></asp:Label>
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
                <li class="selected">Invoices <img src="Images/menubar_more_pistachio.png" /></li>
                    <ul>
                        <li><a href="#">Unprocessed</a></li>
                        <li><a href="#">Processed</a></li>
                     </ul>
                 <li><a href="#">Tax Exceptions</a></li>
                 <li><a href="#">Open Orders</a></li>
            </ul>
        
        </div> 
        
        <div class="body">
            <div class="toolbar">
                <div style="float: left">
                    <img src="Images/toolbar_lborder_pistachio.png" height="44" width="6" class="toolbarLeftBorder"/>
                    <div class="toolbarContent">
                        <asp:Label ID="Label1" runat="server" Text="Start Date"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="toolbarTextBox"></asp:TextBox>
                    </div>
                </div>
                <img src="Images/toolbar_rborder_pistachio.png" height="44" width="6" class="toolbarRightBorder" />
            </div>
            
            <div class="innerBody">
                <asp:Label ID="_pageHeading" runat="server" Text="Pending Renewals" CssClass="pageHeading"></asp:Label>
                
                <div id="queueThrobber"><img src="Images/loading64.gif" /></div>
                <div id="renewals"></div>

            </div>

        </div>
    </div>
    </form>
    </body>
</html>

