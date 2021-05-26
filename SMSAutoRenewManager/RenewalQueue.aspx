<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RenewalQueue.aspx.vb" Inherits="SMSAutoRenewManager.RenewalQueue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SMS Auto-Renewal Queue</title>
    
    <link href="Content/main.css" rel="stylesheet" type="text/css" />
    <link href="Content/pistachio.css" rel="stylesheet" type="text/css" />
    <link href="Content/site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Content/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Content/renewalqueue.js"></script>   
       
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
                <li class="selected">Pending Renewals</li>
                     <ul>
                        <li id="menuExpired"><a href="#">Expired</a></li>
                        <li id="menuWeek"><a href="#">Expire This Week</a></li>
                        <li id="menuMonth"><a href="#">Expire This Month</a></li>
                     </ul>
                <li><a href="processed.aspx">Processed</a></li>
                <li><a href="removed.aspx">Removed Renewals</a></li>
            </ul>
        
        </div> 
        
        <div class="body">
            <div class="toolbar">
                <div style="float: left">
                    <img src="Images/toolbar_lborder_pistachio.png" height="44" width="6" class="toolbarLeftBorder"/>
                    <div class="toolbarContent">
                        <asp:Label ID="Label1" runat="server" Text="Filter by Expire Date"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label9" runat="server" Text="From" CssClass="toolbarField"></asp:Label>
                        <asp:TextBox ID="_startDateFilterTextBox" runat="server" CssClass="toolbarTextBox"></asp:TextBox>
                        <asp:Label ID="Label8" runat="server" Text="To" CssClass="toolbarField"></asp:Label>
                        <asp:TextBox ID="_endDateFilterTextBox" runat="server" CssClass="toolbarTextBox"></asp:TextBox>
                        <a id="_filterLinkButton" class="toolbarLinkButton" href="javascript:sortQueue(1);">Filter</a>
                    </div>
                </div>
                <img src="Images/toolbar_rborder_pistachio.png" height="44" width="6" class="toolbarRightBorder" />
            </div>
            
            <div class="innerBody">
        
            <!--message display here-->
            <div id="_messageDiv" runat="server">
            <asp:PlaceHolder ID="_messagePlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
 
            <table id="contentTable" width="100%" border="0" cellspacing="0" cellpadding="1">
            <tr>
            
            <td width="500" valign="top">
            <div class="queueInnerBody">
                <asp:Label ID="_pageHeading" runat="server" Text="Pending Renewals" CssClass="pageHeading queueHeading"></asp:Label>

                <div class="queueTable">
                 <div id="queueThrobber">
                    <img src="Images/throbber64.gif" /><br /><br />loading
                    </div>
                    <div id="renewals">
                        <asp:PlaceHolder ID="_queuePlaceHolder" runat="server"></asp:PlaceHolder>
                        
                    </div>
                   
                </div>
                <br />
                <div class="queueInnerBodyToolBar">
                <asp:LinkButton ID="_postLinkButton" runat="server" CssClass="queueLinkButton" OnClientClick="return confirm('Post all checked renewals to ALIS?');">Post Checked</asp:LinkButton>
                </div>
            </div>
            </td>
            <td valign="top">
            <div class="queueDetailBox">
               <asp:Label ID="Label2" runat="server" Text="Selected Renewal Details" CssClass="detailHeading queueHeading"></asp:Label>
               <div id="detailThrobberHolder">
                    <div id="detailThrobber">
                        <img src="Images/throbber_detail.gif" />&nbsp;&nbsp;loading
                    </div>
               </div>        
                
 
                <div class="queueDetailInner">
                <table class="renewalDetailHeader">
                    <tr>
                        <td><asp:Label ID="Label6" runat="server" Text="Subscription ID" CssClass="detailItemHeader"></asp:Label></td>
                        <td><asp:Label ID="_subscriptionIDLabel" runat="server"></asp:Label></td>
                   </tr>
                   <tr>
                        <td><asp:Label ID="Label11" runat="server" Text="Name" CssClass="detailItemHeader"></asp:Label></td>
                        <td><asp:Label ID="_subscriberFullNameLabel" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label7" runat="server" Text="Company" CssClass="detailItemHeader"></asp:Label></td>
                        <td><asp:Label ID="_subscriberLocationLabel" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label3" runat="server" Text="Order Type" CssClass="detailItemHeader"></asp:Label></td>
                        <td> <select id="_orderTypeSelect" runat="server" class="detailSelect"></select></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label4" runat="server" Text="Carrier" CssClass="detailItemHeader"></asp:Label></td>
                        <td><select id="_carrierSelect" runat="server" class="detailSelect"></select></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label5" runat="server" Text="Carrier Term" CssClass="detailItemHeader"></asp:Label></td>
                        <td><select id="_termsSelect" runat="server" class="detailSelect"></select></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label10" runat="server" Text="Payment Method" CssClass="detailItemHeader"></asp:Label></td>
                        <td><asp:Label ID="_paymentMethodLabel" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<a href="javascript: changePaymentMethod();" id="changePaymentLink" class="textLinkSmall">[Change to Purchase Order]</a></td>
                    </tr>
                </table>
                
                <div id="detail">
                </div>
                
                </div>
       
                
                
                
                <div class="queueDetailToolBar">
                    <div style="float: left; padding: 5px 5px 10px 5px;">
                    <a href="javascript:saveRenewalDetail()" id="_detailSaveButton" class="detailLinkButton">Mark to Post</a>
                    <br />
                    </div>
                    <div style="float: right; 	padding: 5px 5px 10px 5px;">
                    <asp:LinkButton ID="_removeLinkButton" runat="server" CssClass="detailLinkButton" OnClientClick="return confirm('Remove this renewal from the queue?  This action will mark the subscription as non-autorenew.');">Remove from Queue</asp:LinkButton>
                    <br />
                    </div>
                    <br />
                </div>
            </div>
            </td>
            </tr>
            </table>
            </div>
            
            
        </div>
    </div>
    <input type="hidden" id="checkedIds" runat="server" />
    <input type="hidden" id="selectedSubID" runat="server" />
     </form>
    </body>
</html>
