Public Class App

    Public Enum ApplicationRight
        [SMSAutoRenewManager_View]
        [SMSAutoRenewManager_Post]
    End Enum

    Public Shared Property CurrentUser() As AutoRenewUser
        Get
            If System.Web.HttpContext.Current.Session("CurrentUser") Is Nothing Then
                Return Nothing
            End If

            Return CType(System.Web.HttpContext.Current.Session("CurrentUser"), AutoRenewUser)
        End Get
        Set(ByVal Value As AutoRenewUser)
            System.Web.HttpContext.Current.Session("CurrentUser") = Value
        End Set
    End Property
End Class
