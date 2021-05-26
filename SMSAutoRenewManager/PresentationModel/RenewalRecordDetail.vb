Public Class RenewalRecordDetail

    Public AlisSale As AlisSale
    Public RenewalProduct As RenewalProduct
    Public HonorLastPrice As Boolean
    Public CarrierName As String
    Public CarrierTerm As String
    Public OrderType As String
    Public Subscriber As Subscriber
    Public PaymentMethod As String
    Public ExcludeFromInvoice As Boolean
    Sub New()
        AlisSale = New AlisSale
        RenewalProduct = New RenewalProduct
        Subscriber = New Subscriber
    End Sub
End Class
