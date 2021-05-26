Imports SMSAutoRenewManager.App

Partial Public Class RenewalQueue
    Inherits System.Web.UI.Page
    Private _filterStart As DateTime = CDate("1/1/2000")
    Private _filterEnd As DateTime = Now

    Private _currentUser As AutoRenewUser
    Private _validationErrors As ArrayList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'check for active session

        _currentUser = App.CurrentUser

        If _currentUser Is Nothing Then
            FetchUser()
        End If

        LoadUser()

        If _currentUser.HasRight(ApplicationRight.SMSAutoRenewManager_View) = False Then
            Response.Redirect("rights.html")
        End If

        PopulateDetailSelects()

        If Not IsPostBack Then
            _startDateFilterTextBox.Text = "1/1/2000"
            _endDateFilterTextBox.Text = Now.ToShortDateString

            If Request.QueryString("startdate") <> "" Then
                _startDateFilterTextBox.Text = Request.QueryString("startdate")
            End If
            If Request.QueryString("enddate") <> "" Then
                _endDateFilterTextBox.Text = Request.QueryString("enddate")
            End If

        End If


    End Sub
    Private Sub CheckSessionState()
        If Not Context.Session Is Nothing Then

        End If
    End Sub

    Private Sub _postLinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _postLinkButton.Click
        If checkedIds.Value <> "" Then

            Dim checked = checkedIds.Value.Split(",")

            'using hashset to remove duplicate subscriptions....
            'if a sub is Marked for Posting, unchecked, and then marked again there will be duplicate in checkedIds
            Dim hset As New HashSet(Of String)(checked)
            Dim result(hset.Count) As String
            hset.CopyTo(result)

            Dim i As Integer
            For i = 0 To result.Length - 1
                If result(i) <> "" Then
                    CreateAlisOrder(result(i))
                End If

            Next i

            'reload table
            Dim queueScript As String = "sortQueue(1);"
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "queueScript", queueScript, True)

            'reset chekced
            checkedIds.Value = ""

        End If

    End Sub

    Private Sub PopulateDetailSelects()

        Dim selectService As New SelectListsService

        'order type
        _orderTypeSelect.Items.Clear()
        selectService.PopulateOrderTypeSelect(_orderTypeSelect)

        'carrier
        _carrierSelect.Items.Clear()
        selectService.PopulateCarrierSelect(_carrierSelect)

        'carrier terms
        _termsSelect.Items.Clear()
        selectService.PopulateCarrierTermSelect(_termsSelect)


    End Sub

    Private Sub CreateAlisOrder(ByVal subscriptionID As Integer)

        Try
            Dim orderFactory As New AlisOrderFactory
            Dim orderToPost = orderFactory.CreateOrder(subscriptionID)

            'post order
            Dim service = New OrderService()

            _validationErrors = New ArrayList
            service.Validate(orderToPost, _validationErrors)

            If _validationErrors.Count > 0 Then
                Dim errorMsg As String = String.Empty
                Throw New SMSAutoRenewException(errorMsg)
            End If

            'XDocument.Parse(orderToPost.ToXml).Save("d:\orderxml\testorder_" & subscriptionID & ".xml")
            service.PostToAlis(orderToPost, subscriptionID)

            DisplayMessage(MessageType.Success, "Renewal(s) processed and posted to ALIS.")

        Catch ex As SMSAutoRenewException
            DisplayMessage(MessageType.Error, "Unable to post order for SubID " & subscriptionID & ". " & ex.Message)
        End Try


    End Sub
    'move to a base page class
    Private Sub DisplayMessage(ByVal messageType As MessageType, ByVal text As String, Optional ByVal errors As ArrayList = Nothing)

        Dim html As XElement

        Select Case messageType
            Case RenewalQueue.MessageType.Success
                html = <div class="msgSuccess"><img src="images/message_success.png" class="msgIcon"/><%= text %><a href="javascript: closeMessage();">[close]</a></div>

            Case RenewalQueue.MessageType.Information
                html = <div class="msgInfo"><img src="images/message_info.png" class="msgIcon"/><%= text %><a href="javascript: closeMessage();">[close]</a></div>

            Case RenewalQueue.MessageType.Warning
                html = <div class="msgWarning"><img src="images/message_warning.png" class="msgIcon"/><%= text %><a href="javascript: closeMessage();">[close]</a></div>

            Case RenewalQueue.MessageType.Error
                html = <div class="msgError"><img src="images/message_error.png" class="msgIcon"/><span><%= text %></span><a href="javascript: closeMessage();">[close]</a></div>
                'add each validation error
                Dim ul As XElement = Nothing

                If _validationErrors.Count > 0 Then
                    ul = <ul></ul>

                    For Each e In _validationErrors
                        ul.Add(<li><%= e.ToString %></li>)
                    Next

                End If

                If Not ul Is Nothing Then
                    html.<span>.FirstOrDefault.AddAfterSelf(ul)
                End If


            Case Else
                html = <div class="msgInfo"><img src="images/message_info.png"/><span class="msgText"><%= text %></span></div>

        End Select


        _messageDiv.InnerHtml = html.ToString & "<br />"

    End Sub

    Private Enum MessageType
        Success
        Information
        Warning
        [Error]
    End Enum
    Private Sub LoadUser()

        _loginMessageLabel.Text = _currentUser.Greeting
        _signedInLabel.Text = "Signed in as " & _currentUser.UserName
        _avatarImage.ImageUrl = _currentUser.AvatarImage '"Images/avatar_default.png"

    End Sub
    Private Sub FetchUser()

        'try to get user from Windows login
        Dim service As New AutoRenewUserService
        _currentUser = service.FetchCurrentUserByWindowsLogin
        App.CurrentUser = _currentUser
        If _currentUser Is Nothing Then
            Response.Redirect("login.aspx")
        End If

    End Sub

    Private Sub _loginLinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _loginLinkButton.Click
        'logout
        App.CurrentUser = Nothing
        Response.Redirect("login.aspx")
    End Sub

    Private Sub CreateMenu()



    End Sub

    Private Sub _removeLinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _removeLinkButton.Click

        If IsNumeric(selectedSubID.Value) Then
            RemoveRenewal(selectedSubID.Value)
        End If

    End Sub

    Private Sub RemoveRenewal(ByVal subscriptionID As Integer)
        Try

            Dim removalService As New RemoveRenewalService
            removalService.Remove(subscriptionID, CurrentUser.UserID)

            'need to clear the hidden field that holds ids of subs to post, otherwise a checked renewal
            'that was then removed could still be posted
            Dim checked = checkedIds.Value
            checked = checked.Replace(subscriptionID & ",", "")
            checkedIds.Value = checked

            DisplayMessage(MessageType.Success, "Subscription (SubID: " & subscriptionID & ") changed to non-autorenew and removed from the renewal queue.")
            'clear renewal detail?

        Catch ex As Exception
            DisplayMessage(MessageType.Error, "Unable to remove SubID " & subscriptionID & " from the renewal queue. " & ex.Message)

        End Try
    End Sub
End Class