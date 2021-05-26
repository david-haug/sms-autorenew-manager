Public MustInherit Class AutoRenewEvent

    Private _subID As Integer
    Private _userID As Integer
    Private _dateProcessed As DateTime
    Private _eventType As AutoRenewEventTypes.EventType

    Sub New(ByVal subscriptionID As Integer, ByVal userID As Integer, ByVal eventType As AutoRenewEventTypes.EventType)

        _dateProcessed = Now
        _subID = subscriptionID
        _userID = userID
        _eventType = eventType

    End Sub

    Public ReadOnly Property SubscriptionID() As Integer
        Get
            Return _subID
        End Get
    End Property
    Public ReadOnly Property UserID() As Integer
        Get
            Return _userID
        End Get
    End Property
    Public ReadOnly Property DateProcessed() As DateTime
        Get
            Return _dateProcessed
        End Get
    End Property
    Public ReadOnly Property EventType() As AutoRenewEventTypes.EventType
        Get
            Return _eventType
        End Get
    End Property
    Public Sub Save()

        Dim repository = New SqlAutoRenewEventRepository
        repository.Save(Me)

    End Sub

End Class
