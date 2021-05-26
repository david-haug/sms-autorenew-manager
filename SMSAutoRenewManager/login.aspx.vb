Public Partial Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub _loginButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _loginButton.Click
        Try
            Dim service As New AutoRenewUserService
            App.CurrentUser = service.FetchCurrentUserByAlisLogin(_userNameTextBox.Text, _passwordTextBox.Text)

            Response.Redirect("renewalqueue.aspx")
        Catch ex As Exception
            _messageLabel.Text = ex.Message
        End Try

    End Sub
End Class