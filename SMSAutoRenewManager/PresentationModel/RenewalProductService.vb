Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses
Imports SMSAutoRenewManager.AlisItemService
Public Class RenewalProductService

    Public Shared Function FetchRenewalProduct(ByVal productToRenew As String) As RenewalProduct

        Return FetchRenewalProduct(productToRenew, Nothing, Nothing)

    End Function
    Public Shared Function FetchRenewalProduct(ByVal productToRenew As String, ByVal accountCode As String, ByVal quantity As Integer) As RenewalProduct

        Try

            Dim subItem = SubscriptionItemManager.FetchNew(SubscriptionItemManager.Where.ItemNameEquals(productToRenew))

            Dim renewSubItem As SubscriptionItemEntity
            If subItem.Is1YrNew Or subItem.Is1YrRenew Then
                'get 1 yr renewal
                renewSubItem = SubscriptionItemManager.FetchNew(SubscriptionItemManager.Where.Is1YrRenewAndItemKeyEquals(subItem.ItemKey))
            Else
                'get 3 year renewal
                renewSubItem = SubscriptionItemManager.FetchNew(SubscriptionItemManager.Where.Is3YrRenewAndItemKeyEquals(subItem.ItemKey))
            End If

            'create a renewalitem using the subitem entity
            Dim renewalProduct As New RenewalProduct
            renewalProduct.Name = renewSubItem.ItemName

            'ItemID can be null in SubscriptionItem...better place to get it?
            'Dim alisProduct As AlisSoftwareProductEntity '= AlisSoftwareProductManager.Where.Equals(AlisSoftwareProductFieldIndex.ItemName, renewalProduct.Name)
            'renewalProduct.ItemID = alisProduct.ItemId

            'get price...use account, honorlast price...


            Dim itemService = New AlisItemService.ItemService
            renewalProduct.Price = itemService.PriceFetchByItemIDQuantityAccount(renewSubItem.ItemId, quantity, accountCode)


            Return renewalProduct
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
End Class
