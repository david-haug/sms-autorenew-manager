Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses

Public Class ProcessedRenewalRepository

    Public Function FetchAll() As IList(Of ProcessedRenewal)

        Dim processedRenewals As New List(Of ProcessedRenewal)
        Dim renewals As EntityCollection = AutoRenewLogManager.FetchCollection()

        For Each r As AutoRenewLogEntity In renewals

            Dim subEntity = SubscriptionManager.FetchUsingPrimaryKey(r.SubscriptionId)

            Dim subscription = New Subscription With { _
                                    .SubscriptionId = subEntity.SubscriptionId, _
                                    .Carrier = subEntity.Carrier, _
                                    .CarrierTerms = subEntity.CarrierTerms, _
                                    .CarrierAccount = subEntity.CarrierAccount, _
                                    .StartDate = subEntity.StartDate, _
                                    .EndDate = subEntity.EndDate, _
                                    .SalespersonId = subEntity.SalespersonId, _
                                    .Salesperson = subEntity.SalespersonId.ToString, _
                                    .AlisInvoiceNumber = FetchAlisInvoiceNumber(subEntity.SalesJournalId), _
                                    .AlisSalesJournalID = subEntity.SalesJournalId}

            Dim processed As New ProcessedRenewal
            processed.Subscription = subscription
            processed.AlisOrderRef = r.AlisorderRefNum
            processed.AutoRenewLogID = r.AutoRenewLogId

            If r.ProcessedByUserId.HasValue Then
                Dim userService = New AutoRenewUserService
                processed.ProcessedByUserName = userService.FetchAlisUserFullName(r.ProcessedByUserId)
            End If

            processed.DateProcessed = r.DateProcessed
            processedRenewals.Add(processed)
        Next

        Return processedRenewals

    End Function

    Private Function FetchAlisInvoiceNumber(ByVal transactionID) As String

        Dim sale = AlisSalesManager.FetchNew(AlisSalesManager.Where.TransactionIDEquals(transactionID))
        Return sale.InvoiceNumber & "-" & sale.InvoiceSuffix

    End Function


End Class
