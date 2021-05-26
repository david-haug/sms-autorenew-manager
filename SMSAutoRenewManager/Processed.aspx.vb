Imports SMSAutoRenewManager.App
Partial Public Class Processed
    Inherits AutoRenewManagerPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadUser()

        If CurrentUser.HasRight(ApplicationRight.SMSAutoRenewManager_View) = False Then
            Response.Redirect("rights.html")
        End If

        LoadProcessed()

    End Sub

    Private Sub LoadProcessed()
        Dim processed As New List(Of ProcessedRenewal)

        Dim repository As New SqlProcessedRenewalRepository
        processed = repository.FetchAll

        Dim sortedProcessed = From p In processed _
                              Order By p.DateProcessed Descending _
                              Select p


        _processedGridView.DataSource = sortedProcessed.ToList
        _processedGridView.DataBind()
    End Sub
    Private Sub LoadUser()

        _loginMessageLabel.Text = CurrentUser.Greeting
        _signedInLabel.Text = "Signed in as " & CurrentUser.UserName
        _avatarImage.ImageUrl = CurrentUser.AvatarImage '"Images/avatar_default.png"

    End Sub

   
    Protected Friend Sub _processedGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles _processedGridView.PageIndexChanging
        _processedGridView.PageIndex = e.NewPageIndex
        _processedGridView.DataBind()
    End Sub

End Class