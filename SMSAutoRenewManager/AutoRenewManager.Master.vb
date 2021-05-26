Imports SMSAutoRenewManager.App
Partial Public Class AutoRenewManager
    Inherits System.Web.UI.MasterPage
    Private _currentUser As AutoRenewUser
    Public Property CurrentUser() As AutoRenewUser
        Get
            Return _currentUser
        End Get
        Set(ByVal value As AutoRenewUser)
            _currentUser = value
        End Set
    End Property
    Private _contentApplicationRights As List(Of ApplicationRight)
    Public Property ContentApplicationRights() As List(Of ApplicationRight)
        Get
            If _contentApplicationRights Is Nothing Then
                Return New List(Of ApplicationRight)
            End If
            Return _contentApplicationRights
        End Get
        Set(ByVal value As List(Of ApplicationRight))
            _contentApplicationRights = value
        End Set
    End Property
    Private _selectedMenuItem As String
    Public Property SelectedMenuItem() As String
        Get
            Return _selectedMenuItem
        End Get
        Set(ByVal value As String)
            _selectedMenuItem = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'TODO: check for active session

        _currentUser = App.CurrentUser

        If _currentUser Is Nothing Then
            FetchUser()
        End If

        LoadUser()

        'verify rights
        For Each appRight In ContentApplicationRights
            If CurrentUser.HasRight(appRight) = False Then
                Response.Redirect("rights.html")
            End If
        Next


    End Sub

    Private Sub LoadUser()

        _loginMessageLabel.Text = _currentUser.Greeting
        _signedInLabel.Text = "Signed in as " & _currentUser.UserName
        _avatarImage.ImageUrl = _currentUser.AvatarImage

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

        Dim menu = <ul>
                       <li>Pending Renewals</li>
                       <ul>
                           <li id="menuExpired"><a href="#">Expired</a></li>
                           <li id="menuWeek"><a href="#">Expire This Week</a></li>
                           <li id="menuMonth"><a href="#">Expire This Month</a></li>
                       </ul>
                       <li><a href="processed.aspx">Processed</a></li>
                   </ul>

        'menubar.InnerHtml = menu.ToString

    End Sub

End Class