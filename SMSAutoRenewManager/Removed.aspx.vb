Imports SMSAutoRenewManager.App
Partial Public Class Removed
    Inherits AutoRenewManagerPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadUser()

        If CurrentUser.HasRight(ApplicationRight.SMSAutoRenewManager_View) = False Then
            Response.Redirect("rights.html")
        End If

        LoadRemoved()

    End Sub

    Private Sub LoadRemoved()
        Dim removed As New List(Of RemovedRenewal)

        Dim repository As New SqlRemovedRenewalRepository
        removed = repository.FetchAll

        Dim sortedProcessed = From r In removed _
                              Order By r.DateProcessed Descending _
                              Select r


        _removedGridView.DataSource = sortedProcessed.ToList
        _removedGridView.DataBind()
    End Sub
    Private Sub LoadUser()

        _loginMessageLabel.Text = CurrentUser.Greeting
        _signedInLabel.Text = "Signed in as " & CurrentUser.UserName
        _avatarImage.ImageUrl = CurrentUser.AvatarImage '"Images/avatar_default.png"

    End Sub


    Protected Friend Sub _removedGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles _removedGridView.PageIndexChanging
        _removedGridView.PageIndex = e.NewPageIndex
        _removedGridView.DataBind()
    End Sub

End Class