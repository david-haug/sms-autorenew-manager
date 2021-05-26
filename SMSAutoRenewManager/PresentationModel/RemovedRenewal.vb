Public Class RemovedRenewal

    Private _processedBy As String
    Public Property ProcessedByUserName() As String
        Get
            Return _processedBy
        End Get
        Set(ByVal value As String)
            _processedBy = value
        End Set
    End Property
    Private _accountCode As String
    Public Property AccountCode() As String
        Get
            Return _accountCode
        End Get
        Set(ByVal value As String)
            _accountCode = value
        End Set
    End Property
    Private _invoiceNumber As String
    Public Property InvoiceNumber() As String
        Get
            Return _invoiceNumber
        End Get
        Set(ByVal value As String)
            _invoiceNumber = value
        End Set
    End Property
    Private _companyName As String
    Public Property CompanyName() As String
        Get
            Return _companyName
        End Get
        Set(ByVal value As String)
            _companyName = value
        End Set
    End Property

    Private _dateProcessed As DateTime
    Public Property DateProcessed() As DateTime
        Get
            Return _dateProcessed
        End Get
        Set(ByVal value As DateTime)
            _dateProcessed = value
        End Set
    End Property
    Private _subscription As Subscription
    Public Property Subscription() As Subscription
        Get
            If _subscription Is Nothing Then
                _subscription = New Subscription
            End If
            Return _subscription
        End Get
        Set(ByVal value As Subscription)
            _subscription = value
        End Set
    End Property
    Public ReadOnly Property SubscriptionID() As Integer
        Get
            Return _subscription.SubscriptionId
        End Get
    End Property
End Class
