Public Class RenewalRecord
    Implements IPresentationObject
    'Public SubscriptionID As Integer
    'Public ExpireDate As DateTime
    'Public AccountCode As String
    'Public PreviousAlisOrder As String
    'Public InvoiceLineItems As List(Of InvoiceLineItem)
    Private _subscription As Subscription
    Sub New()

    End Sub
    Sub New(ByVal subscription As Subscription, ByVal subscriber As Subscriber, ByVal details As IList(Of RenewalRecordDetail))
        _subscription = subscription
        _subscriber = subscriber
        _details = details
    End Sub

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
    Private _subscriber As Subscriber
    Public Property Subscriber() As Subscriber
        Get
            If _subscriber Is Nothing Then
                _subscriber = New Subscriber
            End If
            Return _subscriber
        End Get
        Set(ByVal value As Subscriber)
            _subscriber = value
        End Set
    End Property
  
    Private _details As List(Of RenewalRecordDetail)
    Public Property Details() As IList(Of RenewalRecordDetail)
        Get
            If _details Is Nothing Then
                _details = New List(Of RenewalRecordDetail)
            End If
            Return _details
        End Get
        Set(ByVal value As IList(Of RenewalRecordDetail))
            _details = value
        End Set
    End Property
End Class
