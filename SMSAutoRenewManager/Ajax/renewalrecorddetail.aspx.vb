
Partial Public Class renewalrecorddetail1
    Inherits System.Web.UI.Page
    Private _subscriptionID As Integer
    Private _details As List(Of RenewalRecordDetail)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("id") <> "" Then
            _subscriptionID = Request.QueryString("id")
        End If

        If Request.QueryString("save") = "true" Then
            'verify renewal detail
            Dim detail = CreateDetailFromJson(Request.Form)
            'ValidateRenewalDetail(detail) 'TODO:
            SaveRenewalDetail(detail)
            Exit Sub
        End If

        Dim repository = New RenewalRecordRepository
        _details = repository.FetchDetails(_subscriptionID)

        ReturnJson(_details)
    End Sub

    Private Sub ReturnJson(ByVal details As IList(Of RenewalRecordDetail))

        Dim json As String

        json = "{""details"": [" & vbCrLf
        For Each d In details
            'Format:
            '{"alissale": {"name":"NAME","quantity": "QUANTITY","price":"PRICE"},
            '"renewalproduct": {"name":"NAME","quantity": "QUANTITY","price":"PRICE"},
            '"honorlastprice":"HONORLASTPRICE"}

            Dim djson As String
            'using <> instead of proper JSON brackets {} for string.format
            djson = String.Format("<""alissale"": <""name"":""{0}"",""quantity"": ""{1}"",""price"":""{2}"",""transactiondetailid"":""{3}"">,", d.AlisSale.ProductName, d.AlisSale.QuantityOrdered, d.AlisSale.UnitPrice, d.AlisSale.TransactionDetailId)
            djson += String.Format("""renewalproduct"": <""name"":""{0}"",""price"":""{1}"">,", d.RenewalProduct.Name, d.RenewalProduct.Price)
            djson += String.Format("""subscriber"": <""locationname"":""{0}"",""fullname"":""{1}"">,", d.Subscriber.LocationName, d.Subscriber.FirstName & " " & d.Subscriber.LastName)

            djson += String.Format("""excludefrominvoice"": ""{0}"",", d.ExcludeFromInvoice)
            djson += String.Format("""honorlastprice"": ""{0}"",", d.HonorLastPrice)
            djson += String.Format("""carriername"": ""{0}"",", d.CarrierName)
            djson += String.Format("""carrierterm"": ""{0}"",", d.CarrierTerm)
            djson += String.Format("""ordertype"": ""{0}"",", d.OrderType)
            djson += String.Format("""paymentmethod"": ""{0}"">,", d.PaymentMethod)

            'formatting done, replace "<" and ">"
            djson = djson.Replace("<", "{")
            djson = djson.Replace(">", "}")

            'add the detail to the string representing the details array
            json += djson
        Next

        'we have a trailing comma on the last detail, remove it
        json = json.Remove(json.Length - 1, 1)

        'close it up 
        json += "]}" & vbCrLf

        Response.ContentType = "application/json"
        Response.Write(json)

    End Sub
    Private Function CreateDetailFromJson(ByVal formCollection As NameValueCollection) As RenewalRecordDetail

        Dim detail As New RenewalRecordDetail
        detail.CarrierName = formCollection.Item("carriername")
        detail.CarrierTerm = formCollection.Item("carrierterm")
        detail.RenewalProduct.Name = formCollection.Item("productname")
        detail.RenewalProduct.Price = formCollection.Item("price")
        detail.RenewalProduct.Quantity = formCollection.Item("quantity")
        detail.AlisSale.TransactionDetailId = formCollection.Item("transactiondetailID")
        detail.OrderType = formCollection.Item("ordertype")
        detail.PaymentMethod = formCollection.Item("paymentmethod")
        detail.ExcludeFromInvoice = formCollection.Item("excludefrominvoice")

        Return detail

    End Function
    Private Function ValidateRenewalDetail(ByVal detail As RenewalRecordDetail) As Boolean

        'validate line items
        Dim orderService As New OrderService

        Dim lineItem = New Labelmaster.AlisOrder.BusinessObjects.LineItem With { _
                            .Product = New Labelmaster.AlisOrder.BusinessObjects.Product With {.Name = detail.RenewalProduct.Name}, _
                            .QuantityOrdered = detail.RenewalProduct.Quantity, _
                            .Price = detail.RenewalProduct.Price}
        Dim errors As New ArrayList
        orderService.Validate(lineItem, errors)

        If errors.Count = 0 Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Sub SaveRenewalDetail(ByVal detail As RenewalRecordDetail)

        Dim repository = New RenewalRecordRepository
        If repository.SaveWorkAutoRenewQueueDetail(detail) Then
            Response.Write("{""success"":""true""}")
        Else
            Response.Write("{""success"":""false""}")
        End If


    End Sub
End Class