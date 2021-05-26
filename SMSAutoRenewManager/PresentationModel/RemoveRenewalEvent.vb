Public Class RemoveRenewalEvent
    Inherits AutoRenewEvent

    Sub New(ByVal subscriptionID As Integer, ByVal userID As Integer)
        MyBase.New(subscriptionID, userID, AutoRenewEventTypes.EventType.Remove_Renewal_From_Queue)
    End Sub

End Class
