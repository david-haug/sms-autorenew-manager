Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Response.Write("This guy is logged in: " & System.Security.Principal.WindowsIdentity.GetCurrent().Name())
        Response.Redirect("renewalqueue.aspx")
    End Sub

End Class