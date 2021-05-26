Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses
Public Class RemoveRenewalService

    Public Sub Remove(ByVal subscriptionID As Integer, ByVal userID As Integer)

        Dim subscription = SubscriptionManager.FetchUsingPrimaryKey(subscriptionID)
        subscription.IsAutoRenew = False
        SubscriptionManager.Save(subscription, True)

        'record event
        Dim removeEvent = New RemoveRenewalEvent(subscriptionID, userID)
        removeEvent.Save()

        'also need to remove any existing work record
        'TODO: more than 1 sale!
        Dim sale = AlisSalesManager.FetchNew(AlisSalesManager.Where.TransactionIDEquals(subscription.SalesJournalId))
        Dim workRecords As EntityCollection = WorkAutoRenewQueueManager.FetchCollection(WorkAutoRenewQueueManager.Where.AlisTransactionDetailIDEquals(sale.TransactionDetailId))
        WorkAutoRenewQueueManager.DeleteCollection(workRecords)


    End Sub

End Class
