Public Class ProcessedRenewal
    Private _autoRenewalLogID As Integer
    Public Property AutoRenewLogID() As Integer
        Get
            Return _autoRenewalLogID
        End Get
        Set(ByVal value As Integer)
            _autoRenewalLogID = value
        End Set
    End Property
    Private _processedBy As String
    Public Property ProcessedByUserName() As String
        Get
            Return _processedBy
        End Get
        Set(ByVal value As String)
            _processedBy = value
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
    Private _alisOrderRef As String
    Public Property AlisOrderRef() As String
        Get
            Return _alisOrderRef
        End Get
        Set(ByVal value As String)
            _alisOrderRef = value
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
