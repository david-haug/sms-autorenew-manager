Public Partial Class queuehtmltable
    Inherits System.Web.UI.Page

    Private _renewalRecords As New List(Of RenewalRecord)
    Private _date1 As DateTime
    Private _date2 As DateTime
    Private _sortColumn As SortMethod
    Private _selectedId As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("date1") <> "" And Request.QueryString("date1") <> "undefined" Then
            _date1 = Request.QueryString("date1")
        Else
            _date1 = "1/1/2000"
        End If

        If Request.QueryString("date2") <> "" And Request.QueryString("date2") <> "undefined" Then
            _date2 = Request.QueryString("date2")
        Else
            _date2 = Now
        End If

        If Request.QueryString("id") <> "" Then
            _selectedId = Request.QueryString("id")
        End If

        If Request.QueryString("col") <> "" Then
            _sortColumn = CInt(Request.QueryString("col"))
        End If

        'Dim repository As New RenewalRecordRepository
        Dim repository As New SqlRenewalRecordRepository
        _renewalRecords = repository.FetchAll(_date1, _date2)

        CreateHtmlTable()
    End Sub
    Private _sortedIndex As Integer
    Private _arrowImage As String
    Private _sortedColumnText As String
    Private _upArrow As String = "images/sort_up.gif"
    Private _downArrow As String = "images/sort_down.gif"

    Private Sub CreateHtmlTable()

        'sort renewal records
        SortRenewals()

        Dim queueTable As XElement = <table id="queueDataTable" cellspacing="0" cellpadding="5" width="95%">
                                         <tr class="dataTableCaption">
                                             <th>Process</th>
                                             <th onclick=<%= _sortScript(0) %>>Subscription ID</th>
                                             <th onclick=<%= _sortScript(1) %>>End Date</th>
                                             <th onclick=<%= _sortScript(2) %>>Account</th>
                                             <th onclick=<%= _sortScript(3) %>>Invoice Number</th>
                                         </tr>
                                     </table>


        'TODO: come up with cleaner way to sort columns
        queueTable.<tr>.<th>.ElementAtOrDefault(_sortedIndex).SetValue("")
        queueTable.<tr>.<th>.ElementAtOrDefault(_sortedIndex).Add(<img src=<%= _arrowImage %> class="dataTableArrow"/>)
        queueTable.<tr>.<th>.ElementAtOrDefault(_sortedIndex).Add(<span><%= _sortedColumnText %></span>)

        'initialize alternateRow variable
        Dim altRow As Boolean = False


        Dim firstRow = True
        For Each r In _renewalRecords

            Dim cssClass As String = "dataTableRow"

            If altRow Then
                cssClass = "dataTableRowAlternating"
            End If

            'If r.Subscription.SubscriptionId = _selectedId Or firstRow Then
            '    cssClass = "dataTableRowSelected"
            'End If



            Dim row As XElement = <tr onclick=<%= "javascript:  getRenewalDetail(" & r.Subscription.SubscriptionId & ")" %> class=<%= cssClass %> ID=<%= "row" & r.Subscription.SubscriptionId %>>
                                      <td><input name=<%= "chkPost_" & r.Subscription.SubscriptionId %> type="checkbox" id=<%= "chkPost_" & r.Subscription.SubscriptionId %> value=<%= r.Subscription.SubscriptionId %> disabled="disabled" class="queueCheck"/></td>
                                      <td><%= r.Subscription.SubscriptionId %></td>
                                      <td><%= r.Subscription.EndDate.ToShortDateString %></td>
                                      <td><%= r.Subscriber.AccountCode %></td>
                                      <td><%= r.Subscription.AlisInvoiceNumber %></td>
                                  </tr>

            altRow = Not altRow
            firstRow = False

            'add to table element
            queueTable.Add(row)

        Next

        Response.Write("<div style=""height: 430px; overflow: auto;"">" & queueTable.ToString & "</div>")

    End Sub
    Private _sortScript As New ArrayList
    Private Sub SortRenewals()

        'initialize with ascending sort values...
        _sortScript.Add("javascript: sortQueue(" & SortMethod.SubscriptionID_Asc & ")")
        _sortScript.Add("javascript: sortQueue(" & SortMethod.EndDate_Asc & ")")
        _sortScript.Add("javascript: sortQueue(" & SortMethod.AccountCode_Asc & ")")
        _sortScript.Add("javascript: sortQueue(" & SortMethod.AlisInvoiceNumber_Asc & ")")

        Select Case _sortColumn
            Case SortMethod.SubscriptionID_Asc
                _renewalRecords = _renewalRecords.OrderBy(Function(r) r.Subscription.SubscriptionId).ToList
                _sortScript.RemoveAt(0)
                _sortScript.Insert(0, "javascript: sortQueue(" & SortMethod.SubscriptionID_Dsc & ")")
                _sortedColumnText = "Subscription ID"
                _sortedIndex = 1
                _arrowImage = _upArrow

            Case SortMethod.SubscriptionID_Dsc
                _renewalRecords = _renewalRecords.OrderByDescending(Function(r) r.Subscription.SubscriptionId).ToList
                _sortScript.RemoveAt(0)
                _sortScript.Insert(0, "javascript: sortQueue(" & SortMethod.SubscriptionID_Asc & ")")
                _sortedColumnText = "Subscription ID"
                _sortedIndex = 1
                _arrowImage = _downArrow

            Case SortMethod.EndDate_Asc
                _renewalRecords = _renewalRecords.OrderBy(Function(r) r.Subscription.EndDate).ToList
                _sortScript.RemoveAt(1)
                _sortScript.Insert(1, "javascript: sortQueue(" & SortMethod.EndDate_Dsc & ")")
                _sortedColumnText = "End Date"
                _sortedIndex = 2
                _arrowImage = _upArrow


            Case SortMethod.EndDate_Dsc
                _renewalRecords = _renewalRecords.OrderByDescending(Function(r) r.Subscription.EndDate).ToList
                _sortScript.RemoveAt(1)
                _sortScript.Insert(1, "javascript: sortQueue(" & SortMethod.EndDate_Asc & ")")
                _sortedColumnText = "End Date"
                _sortedIndex = 2
                _arrowImage = _downArrow

            Case SortMethod.AccountCode_Asc
                _renewalRecords = _renewalRecords.OrderBy(Function(r) r.Subscriber.AccountCode).ToList
                _sortScript.RemoveAt(2)
                _sortScript.Insert(2, "javascript: sortQueue(" & SortMethod.AccountCode_Dsc & ")")
                _sortedColumnText = "Account"
                _sortedIndex = 3
                _arrowImage = _upArrow

            Case SortMethod.AccountCode_Dsc
                _renewalRecords = _renewalRecords.OrderByDescending(Function(r) r.Subscriber.AccountCode).ToList
                _sortScript.RemoveAt(2)
                _sortScript.Insert(2, "javascript: sortQueue(" & SortMethod.AccountCode_Asc & ")")
                _sortedColumnText = "Account"
                _sortedIndex = 3
                _arrowImage = _downArrow

            Case SortMethod.AlisInvoiceNumber_Asc
                _renewalRecords = _renewalRecords.OrderBy(Function(r) r.Subscription.AlisInvoiceNumber).ToList
                _sortScript.RemoveAt(3)
                _sortScript.Insert(3, "javascript: sortQueue(" & SortMethod.AlisInvoiceNumber_Dsc & ")")
                _sortedColumnText = "Invoice Number"
                _sortedIndex = 4
                _arrowImage = _upArrow

            Case SortMethod.AlisInvoiceNumber_Dsc
                _renewalRecords = _renewalRecords.OrderByDescending(Function(r) r.Subscription.AlisInvoiceNumber).ToList
                _sortScript.RemoveAt(3)
                _sortScript.Insert(3, "javascript: sortQueue(" & SortMethod.AlisInvoiceNumber_Asc & ")")
                _sortedColumnText = "Invoice Number"
                _sortedIndex = 4
                _arrowImage = _downArrow

        End Select



    End Sub

    Private Enum SortMethod
        SubscriptionID_Asc = 1
        EndDate_Asc = 2
        AccountCode_Asc = 3
        AlisInvoiceNumber_Asc = 4
        SubscriptionID_Dsc = 5
        EndDate_Dsc = 6
        AccountCode_Dsc = 7
        AlisInvoiceNumber_Dsc = 8
    End Enum

End Class