Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses


Public Class RenewalRecordRepository

    Sub New()

    End Sub

    Public Function FetchAll(ByVal startDate As DateTime, ByVal endDate As DateTime) As IList(Of RenewalRecord)

        'get the subs
        'TODO: SubscriptionManager.Where.UnprocessedIsAutoRenewAndEndDateBetween needs changing...
        'also need filter to exclude subscriptions where SubID = PrevSubID of another subscription (indicates it has already been processed outside of system)
        Dim subscriptions = SubscriptionManager.FetchCollection(SubscriptionManager.Where.UnprocessedIsAutoRenewAndEndDateBetween(startDate, endDate), SubscriptionManager.Prefetch.RelatedEntiteis)
        Dim renewals As New List(Of RenewalRecord)
        For Each subEntity As SubscriptionEntity In subscriptions

            'create a renewal record
            Dim renewal As New RenewalRecord
            renewal.Subscription = New Subscription With { _
                                    .SubscriptionId = subEntity.SubscriptionId, _
                                    .Carrier = subEntity.Carrier, _
                                    .CarrierTerms = subEntity.CarrierTerms, _
                                    .CarrierAccount = subEntity.CarrierAccount, _
                                    .StartDate = subEntity.StartDate, _
                                    .EndDate = subEntity.EndDate, _
                                    .SalespersonId = subEntity.SalespersonId, _
                                    .AlisInvoiceNumber = FetchAlisInvoiceNumber(subEntity.SalesJournalId), _
                                    .AlisSalesJournalID = subEntity.SalesJournalId}

            '4/5/10 don't need slaesperson pull from above
            '.Salesperson = FetchSalespersonName(subEntity.SalespersonId), _

            renewal.Subscriber = New Subscriber With { _
                                    .SubscriberID = subEntity.Subscriber.SubscriberId, _
                                    .FirstName = subEntity.Subscriber.RenewalFirstName, _
                                    .LastName = subEntity.Subscriber.RenewalLastName, _
                                    .LocationName = subEntity.Subscriber.RenewalLocationName, _
                                    .Address1 = subEntity.Subscriber.RenewalAddress1, _
                                    .Address2 = subEntity.Subscriber.RenewalAddress2, _
                                    .City = subEntity.Subscriber.RenewalCity, _
                                    .State = subEntity.Subscriber.RenewalState, _
                                    .PostalCode = subEntity.Subscriber.RenewalPostal, _
                                    .Country = subEntity.Subscriber.RenewalCountry, _
                                    .Phone = subEntity.Subscriber.RenewalPhone, _
                                    .Email = subEntity.Subscriber.RenewalEmail, _
                                    .AccountCode = subEntity.Subscriber.Alisaccount}

 

            renewals.Add(renewal)

        Next

        Return renewals

    End Function

    Public Function FetchDetails(ByVal subscriptionID As Integer)

        Dim subscription As SubscriptionEntity = SubscriptionManager.FetchUsingPrimaryKey(subscriptionID)
        SubscriptionManager.FillSubscriber(subscription)

        Dim details = New List(Of RenewalRecordDetail)
        Dim alisSales As EntityCollection = AlisSalesManager.FetchCollection(AlisSalesManager.Where.TransactionIDEquals(subscription.SalesJournalId))

        'not all products 
        'for each sale as As AlisSalesEntity In alisSales.Where(Function(s) s.ItemName
        For Each sale As AlisSalesEntity In alisSales

            'see if transactiondetail is in workAutoRenewQueue
            'TODO: clean up getting max work 
            Dim workRecords As EntityCollection = WorkAutoRenewQueueManager.FetchCollection(WorkAutoRenewQueueManager.Where.AlisTransactionDetailIDEquals(sale.TransactionDetailId))

            Dim work As New WorkAutoRenewQueueEntity
            If workRecords.Count > 0 Then
                Dim maxWorkId = (From w As WorkAutoRenewQueueEntity In workRecords _
                            Select w.AutoRenewQueueId).Max

                work = WorkAutoRenewQueueManager.FetchUsingPrimaryKey(maxWorkId)
            End If

            Dim detail As New RenewalRecordDetail


            detail.AlisSale = New AlisSale With { _
                                .TransactionDetailId = sale.TransactionDetailId, _
                                .ProductName = sale.ItemName, _
                                .QuantityOrdered = sale.QuantityOrdered, _
                                .UnitPrice = sale.UnitPrice}

            If work.IsNew Or work Is Nothing Then

                Dim renewalProduct = RenewalProductService.FetchRenewalProduct(sale.ItemName, sale.AccountCode, sale.QuantityOrdered)
                If renewalProduct Is Nothing Then
                    detail.RenewalProduct.Name = sale.ItemName
                    detail.RenewalProduct.Quantity = sale.QuantityOrdered
                    detail.RenewalProduct.Price = sale.UnitPrice

                Else
                    detail.RenewalProduct = renewalProduct
                    'Per Kim P, HonorLastPrice not used (honorlastprice seems to be evrywhere...Subscription, SubscriptionDetail, Subscriber)
                    ' detail.HonorLastPrice = 

                End If

                detail.ExcludeFromInvoice = False
                detail.OrderType = "Billing Only"
                detail.CarrierName = sale.CarrierDescription
                detail.CarrierTerm = sale.CarrierTermsDescription
                detail.PaymentMethod = sale.PaymentMethod

            Else
                detail.RenewalProduct.Name = work.ProductName
                detail.RenewalProduct.Quantity = work.Quantity
                detail.RenewalProduct.Price = work.Price
                detail.CarrierName = work.Carrier
                detail.CarrierTerm = work.CarrierTerms
                detail.PaymentMethod = work.PaymentMethod
                detail.OrderType = work.OrderType
                detail.ExcludeFromInvoice = work.ExcludeFromInvoice

            End If

            detail.Subscriber.FirstName = subscription.Subscriber.RenewalFirstName
            detail.Subscriber.LastName = subscription.Subscriber.RenewalLastName
            detail.Subscriber.LocationName = subscription.Subscriber.RenewalLocationName

            details.Add(detail)
        Next

        Return details

    End Function

    Private Function FetchAlisSale(ByVal transactionID) As AlisSalesEntity

        Dim sale = AlisSalesManager.FetchNew(AlisSalesManager.Where.TransactionIDEquals(transactionID))
        Return sale

    End Function
    Private Function FetchAlisInvoiceNumber(ByVal transactionID) As String

        Dim sale = AlisSalesManager.FetchNew(AlisSalesManager.Where.TransactionIDEquals(transactionID))
        Return sale.InvoiceNumber & "-" & sale.InvoiceSuffix

    End Function


    Private Function FetchLineItems(ByVal subscriptionID As Integer) As List(Of InvoiceLineItem)

        Dim lineItems As New List(Of InvoiceLineItem)
        Return lineItems

    End Function

    Private Function FetchSalespersonName(ByVal salespersonID As Integer) As String

        'Dim user As SecurityUserEntity = SecurityUserManager.FetchNew()

        Return ""

    End Function

    Public Function SaveWorkAutoRenewQueueDetail(ByVal detail As RenewalRecordDetail) As Boolean

        Dim work As New WorkAutoRenewQueueEntity
        work.AlisTransactionDetailId = detail.AlisSale.TransactionDetailId
        work.Carrier = detail.CarrierName
        work.CarrierTerms = detail.CarrierTerm
        work.OrderType = detail.OrderType
        work.ProductName = detail.RenewalProduct.Name
        work.Quantity = detail.RenewalProduct.Quantity
        work.Price = detail.RenewalProduct.Price
        work.DateCreated = Now
        work.PaymentMethod = detail.PaymentMethod
        work.UserId = App.CurrentUser.UserID
        work.ExcludeFromInvoice = detail.ExcludeFromInvoice

        Return WorkAutoRenewQueueManager.Save(work)

    End Function

End Class
