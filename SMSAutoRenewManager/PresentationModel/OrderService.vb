
Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses
Imports Labelmaster.AlisOrder.BusinessObjects
Imports Labelmaster.AlisOrder.Services
Imports Labelmaster.AlisOrder.Validation
Imports Labelmaster.AlisOrder.Repositories
Imports System.Configuration

Public Class OrderService
    Private _order As Order

    Sub New()

    End Sub

    Public Sub PostToAlis(ByVal orderToPost As Order, ByVal subscriptionID As Integer)
        Try
            Dim alisConnection = ConfigurationManager.AppSettings("ALIS.ConnectionString")
            'if there is a webservice set up win't need refereence to Services and Repositories

            If SubscriptionNotProcessed(subscriptionID) Then



                Dim alisOrderService As New Labelmaster.AlisOrder.Services.OrderService(New SqlOrderRepository(alisConnection))
                alisOrderService.Post(orderToPost)

                LogAutoRenew(subscriptionID, orderToPost.OrderRefNum)
                DeleteWorkAutoRenew(subscriptionID)
            Else
                Throw New SMSAutoRenewException("SubscriptionID: " & subscriptionID & " has already been processed.")
            End If

        Catch ex As Exception
            Throw New SMSAutoRenewException(ex.Message)
        End Try


    End Sub

    Public Sub Validate(ByVal order As Order, Optional ByVal errorsToReturn As ArrayList = Nothing)

        Dim validator As New OrderValidator
        validator.Validate(order)

        Dim errors As New ArrayList
        For Each e In validator.ValidationErrors
            errors.Add(e.Description)
        Next

        If Not errorsToReturn Is Nothing Then
            errorsToReturn.AddRange(errors)
        End If

    End Sub
    Public Sub Validate(ByVal lineItem As LineItem, Optional ByVal errorsToReturn As ArrayList = Nothing)

        Dim validator As New OrderValidator
        validator.Validate(lineItem)

        Dim errors As New ArrayList
        For Each e In validator.ValidationErrors
            errors.Add(e.Description)
        Next

        If Not errorsToReturn Is Nothing Then
            errorsToReturn.AddRange(errors)
        End If

    End Sub

    Private Sub LogAutoRenew(ByVal subscriptionID As Integer, ByVal orderRef As String)

        Dim log As New AutoRenewLogEntity
        log.AlisorderRefNum = orderRef
        log.SubscriptionId = subscriptionID
        log.DateProcessed = Now
        log.ProcessedByUserId = App.CurrentUser.UserID
        AutoRenewLogManager.Save(log)

        'delete work records
        'DeleteWorkAutoRenew(subscriptionID)


    End Sub

    Private Sub DeleteWorkAutoRenew(ByVal subscriptionID As Integer)

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
        WorkAutoRenewQueueManager.DeleteCollection(workRecords)
    End Sub

    Private Function SubscriptionNotProcessed(ByVal subscriptionID As Integer) As Boolean

        Dim bucket As New RelationPredicateBucket
        Dim filter As New PredicateExpression()
        filter.Add(New FieldCompareValuePredicate(AutoRenewLogFields.SubscriptionId, Nothing, ComparisonOperator.Equal, subscriptionID))
        bucket.PredicateExpression.Add(filter)

        Dim processed As EntityCollection = AutoRenewLogManager.FetchCollection(bucket)
        If processed.Count = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
