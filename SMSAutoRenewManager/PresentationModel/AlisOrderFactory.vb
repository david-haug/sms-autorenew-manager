Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses
Imports Labelmaster.AlisOrder.BusinessObjects


Public Class AlisOrderFactory
    Private _subscription As SubscriptionEntity
    Sub New()

    End Sub

    Public Function CreateOrder(ByVal subscriptionID As Integer) As Order

        Dim order As New Order

        Try
            _subscription = SubscriptionManager.FetchUsingPrimaryKey(subscriptionID)
            'get subscriber too
            SubscriptionManager.FillSubscriber(_subscription)
            ''this will be complicated, get the last work detail....
            'get the transactiondetailids for given subscriptionID

            'fetch max transactiondetail for given subscriptionID to get the "header" data
            Dim work = FetchMaxWorkAutoRenew(subscriptionID)

            order.AccountCode = _subscription.Subscriber.Alisaccount
            '    'order.BillingInstructions = 
            order.BillTo = CreateBillTo()
            order.KeyCode = "NOCODE"
            order.OrderMethodType = "Phone(Telesales)" '"AutoRenewal"
            order.OrderType = work.OrderType
            order.PaymentMethod = work.PaymentMethod

            If order.PaymentMethod.ToUpper <> "PURCHASE ORDER" Then
                'add a credit card
                Dim alisSale = AlisSalesManager.FetchUsingPrimaryKey(work.AlisTransactionDetailId)
                order.CreditCard = New CreditCard With {.ReferenceNumber = alisSale.InvoiceNumber}
            End If

            order.PONumber = Left("LMAUTO" & Now.ToShortDateString.Replace("/", ""), 20)
            order.SalesPersonID = _subscription.SalespersonId
            order.EmployeeID = App.CurrentUser.UserID

            'ship tos
            order.ShipTos.AddRange(CreateShippings)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try

        'before returning order, clean up any field differences between SMS and ALIS
        ComplyToAlisPersistenceConstraints(order)

        Return order
    End Function

    Private Function CreateBillTo() As Location

        Dim billTo = New Location
        billTo.FirstName = _subscription.Subscriber.RenewalFirstName
        billTo.LastName = _subscription.Subscriber.RenewalLastName
        billTo.CompanyName = _subscription.Subscriber.RenewalLocationName
        billTo.DivisionName = ""
        billTo.Address1 = _subscription.Subscriber.RenewalAddress1
        billTo.Address2 = _subscription.Subscriber.RenewalAddress2
        billTo.City = _subscription.Subscriber.RenewalCity
        billTo.State = _subscription.Subscriber.RenewalState
        billTo.PostalCode = _subscription.Subscriber.RenewalPostal
        billTo.Country = _subscription.Subscriber.RenewalCountry
        billTo.Email = _subscription.Subscriber.RenewalEmail
        billTo.Phone = _subscription.Subscriber.RenewalPhone
        billTo.PhoneExt = ""

        Return billTo

    End Function
    Private Function CreateShipTo() As Location

        Dim shipTo = New Location
        shipTo.FirstName = _subscription.Subscriber.RenewalFirstName
        shipTo.LastName = _subscription.Subscriber.RenewalLastName
        shipTo.CompanyName = _subscription.Subscriber.RenewalLocationName
        shipTo.DivisionName = ""
        shipTo.Address1 = _subscription.Subscriber.RenewalAddress1
        shipTo.Address2 = _subscription.Subscriber.RenewalAddress2
        shipTo.City = _subscription.Subscriber.RenewalCity
        shipTo.State = _subscription.Subscriber.RenewalState
        shipTo.PostalCode = _subscription.Subscriber.RenewalPostal
        shipTo.Country = _subscription.Subscriber.RenewalCountry
        shipTo.Email = _subscription.Subscriber.RenewalEmail
        shipTo.Phone = _subscription.Subscriber.RenewalPhone
        shipTo.PhoneExt = ""
        Return shipTo

    End Function
    Private Function CreateShippings() As List(Of Shipping)

        Dim shippings As New List(Of Shipping)

        '3/26/10 DH
        'Per Kim P, create a separate shipment for each renewal item
        'there should be only 1 item per subscription -- SMS does not allow for the editing of multiple items subscriptions
        'for example, if a subscription had a REGSTICK and IMO software, and the customer decided not to renew the IMO product
        'SMS does not have the capability of cancelling the IMO (entire sub needs to be cancelled) and preventing it from reaching the renewal queue

        'need to fetch WorkAutoRenewEntities for our line items
        Dim renewItems = FetchWorkAutoRenewCollection()

        For Each r As WorkAutoRenewQueueEntity In renewItems

            If r.ExcludeFromInvoice = False Then
                Dim shipping As New Shipping
                shipping.Location = CreateShipTo()
                shipping.Instructions = "Automatic Software License Renewal"
                shipping.Carrier = r.Carrier
                shipping.CarrierTerm = r.CarrierTerms
                'shipping.ShippingAccount = 
                shipping.ShipComplete = False
                shipping.LineItems.Add(CreateLineItem(r))

                shippings.Add(shipping)
            End If

        Next

        Return shippings

    End Function
    Private Function CreateLineItem(ByVal renewalItem As WorkAutoRenewQueueEntity) As LineItem

        If Not renewalItem.ExcludeFromInvoice Then
            Return New LineItem With {.Product = New Product With {.Name = renewalItem.ProductName}, _
                            .QuantityOrdered = renewalItem.Quantity, _
                            .Price = renewalItem.Price, _
                            .Taxable = True} 'TODO: Get tax info?
        Else
            Return Nothing
        End If



    End Function
    'Private Function CreateLineItems(ByVal renewalItems As EntityCollection) As List(Of LineItem)

    '    Dim lineItems As New List(Of LineItem)

    '    For Each item As WorkAutoRenewQueueEntity In renewalItems
    '        'do not add item if it is excluded
    '        If Not item.ExcludeFromInvoice Then
    '            lineItems.Add(New LineItem With {.Product = New Product With {.Name = item.ProductName}, _
    '                            .QuantityOrdered = item.Quantity, _
    '                            .Price = item.Price, _
    '                            .Taxable = True}) 'TODO: Get tax info?
    '        End If

    '    Next

    '    Return lineItems

    'End Function

    Private Function FetchMaxWorkAutoRenew(ByVal subscriptionID As Integer) As WorkAutoRenewQueueEntity

        Dim subDetails As EntityCollection = SubscriptionDetailManager.FetchCollection(SubscriptionDetailManager.Where.SubscriptionIDEquals(subscriptionID))
        Dim detailsArray As New ArrayList
        For Each subDetail As SubscriptionDetailEntity In subDetails
            detailsArray.Add(subDetail.AlissalesJournalDetailId)
        Next


        Dim bucket As New RelationPredicateBucket
        Dim filter As New PredicateExpression()
        filter.Add(New FieldCompareRangePredicate(WorkAutoRenewQueueFields.AlisTransactionDetailId, Nothing, detailsArray))
        bucket.PredicateExpression.Add(filter)

        Dim workRecords As EntityCollection = WorkAutoRenewQueueManager.FetchCollection(bucket)

        Dim work As New WorkAutoRenewQueueEntity
        If workRecords.Count > 0 Then
            Dim maxWorkId = (From w As WorkAutoRenewQueueEntity In workRecords _
                        Select w.AutoRenewQueueId).Max

            Return WorkAutoRenewQueueManager.FetchUsingPrimaryKey(maxWorkId)
        Else
            Return workRecords(0)
        End If

    End Function

    Private Function FetchWorkAutoRenewCollection() As EntityCollection

        'this will retrun the max WorkAutoRenewQueue record for each Transactiondetail in subscription
        'get the transactiondetails
        Dim subDetails As EntityCollection = SubscriptionDetailManager.FetchCollection(SubscriptionDetailManager.Where.SubscriptionIDEquals(_subscription.SubscriptionId))
        Dim detailsArray As New ArrayList
        For Each subDetail As SubscriptionDetailEntity In subDetails
            detailsArray.Add(subDetail.AlissalesJournalDetailId)
        Next

        Dim maxWorkCollection As New EntityCollection

        For Each d In detailsArray
            'get all work records for this detail
            Dim workRecords = WorkAutoRenewQueueManager.FetchCollection(WorkAutoRenewQueueManager.Where.AlisTransactionDetailIDEquals(d))
            'get last entry
            Dim maxWork = From work As WorkAutoRenewQueueEntity In workRecords _
                          Order By work.AutoRenewQueueId Descending _
                          Select work

            If maxWork.Count > 0 Then
                maxWorkCollection.Add(maxWork(0))
            End If


        Next

        Return maxWorkCollection
    End Function

    'adding this method to keep all persistence constraints in one location...really not sure best spot
    'for this, but I know I don't want these rules spread throughout the codebase
    Private Sub ComplyToAlisPersistenceConstraints(ByVal order As Order)

        'ALIS only allows 32 characters in SiteName
        order.BillTo.CompanyName = Left(order.BillTo.CompanyName, 32)

        For Each s In order.ShipTos
            s.Location.CompanyName = Left(s.Location.CompanyName, 32)
        Next


    End Sub

End Class
