Imports System.Xml
Imports System.Linq

Public Class AutoRenewUserService
    Private _alisService As AlisServices.SecurityUserService
    Sub New()
        _alisService = New AlisServices.SecurityUserService
    End Sub

    Public Function FetchCurrentUserByWindowsLogin() As AutoRenewUser
        Return FetchCurrentUser(WindowsUserName)
    End Function
    Public Function FetchCurrentUserByAlisLogin(ByVal userName As String, ByVal password As String) As AutoRenewUser

        If _alisService.ValidatePassword(userName, password) Then
            Return FetchCurrentUser(userName)
        Else
            Throw New ApplicationException("Password is incorrect for supplied user name.")
        End If

    End Function
    Public Function FetchAlisUserFullName(ByVal alisUserID As Integer) As String

        Dim alisSecurityUser As New AlisServices.AlisSecurityUser
        alisSecurityUser = _alisService.FetchUserById(alisUserID)
        Return alisSecurityUser.FirstName & " " & alisSecurityUser.LastName

    End Function

    Private Function FetchCurrentUser(ByVal userName As String) As AutoRenewUser

        Dim alisSecurityUser As New AlisServices.AlisSecurityUser
        alisSecurityUser = _alisService.FetchUserByUserName(userName)

        Dim user As New AutoRenewUser With { _
            .UserName = alisSecurityUser.UserName, _
            .UserID = alisSecurityUser.UserID, _
            .FirstName = alisSecurityUser.FirstName, _
            .AvatarImage = "images/avatar_default.png", _
            .Greeting = FetchGreeting() & " " & alisSecurityUser.FirstName}

        'get rights related to this application
        Dim rights = From r In alisSecurityUser.SecurityRights _
                     Where r.Path = "SMSAutoRenewManager" _
                     Select r

        For Each r In rights
            user.Rights.Add(New AutoRenewUser.SecurityRight(r.Name, r.Path))
        Next

        Return user

    End Function

    Private Function WindowsUserName() As String
        Dim currentIdentity As String
        ' Get the user's login ID. This will be of the form "domainname\loginName". 
        currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent().Name()
        ' Chop off at the domain separator. 
        Return Mid(currentIdentity, InStr(1, currentIdentity, "\", CompareMethod.Text) + 1).ToUpper
    End Function

    Private Function FetchGreeting()

        'Dim doc As XDocument


        'here we can pull data from xml file...

        Dim x = DateTime.Now.ToLocalTime.Hour
        If DateTime.Now.ToLocalTime.Hour < 12 Then
            Return "Good Morning"
        Else
            Return "Hello"
        End If


    End Function

End Class
