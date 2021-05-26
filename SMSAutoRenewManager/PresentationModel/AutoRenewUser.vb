Public Class AutoRenewUser

    Private _userName As String
    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property
    Private _userID As Integer
    Public Property UserID() As Integer
        Get
            Return _userID
        End Get
        Set(ByVal value As Integer)
            _userID = value
        End Set
    End Property
    Private _firstName As String
    Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property
    Private _avatarImage As String
    Public Property AvatarImage() As String
        Get
            Return _avatarImage
        End Get
        Set(ByVal value As String)
            _avatarImage = value
        End Set
    End Property
    Private _greeting As String
    Public Property Greeting() As String
        Get
            Return _greeting
        End Get
        Set(ByVal value As String)
            _greeting = value
        End Set
    End Property
    Private _rights As New List(Of SecurityRight)
    Public Property Rights() As List(Of SecurityRight)
        Get
            Return _rights
        End Get
        Set(ByVal value As List(Of SecurityRight))
            _rights = value
        End Set
    End Property

    Public Function HasRight(ByVal appRight As App.ApplicationRight) As Boolean

        For Each r In Me.Rights
            If r.Path & "_" & r.Name = appRight.ToString Then
                Return True
            End If
        Next

        Return False

    End Function


    Public Class SecurityRight
        Sub New(ByVal name As String, ByVal path As String)
            Me.Name = name
            Me.Path = path
        End Sub
        Public ReadOnly Name As String
        Public ReadOnly Path As String
    End Class
End Class
