Imports Labelmaster.Data.Sms.Llbl
Imports Labelmaster.Data.Sms.Llbl.EntityClasses
Imports Labelmaster.Data.Sms.Llbl.FactoryClasses
Imports Labelmaster.Data.Sms.Llbl.HelperClasses
Imports Labelmaster.Data.Sms.Llbl.DatabaseSpecific
Imports Labelmaster.Data.Sms.Llbl.ManagerClasses
Imports SD.LLBLGen.Pro.ORMSupportClasses

Public Class SelectListsService

    Public Sub PopulateOrderTypeSelect(ByVal htmlSelect As HtmlSelect)

        'order alis type do not change...will use enum for population
        For Each orderType As AlisOrderType In [Enum].GetValues(GetType(AlisOrderType))

            Select Case orderType
                Case AlisOrderType.Product
                    htmlSelect.Items.Add(New ListItem("Product", "Product"))
                Case AlisOrderType.Billing_Only
                    htmlSelect.Items.Add(New ListItem("Billing Only", "Billing Only"))
                Case Else
                    'nothing to add, do't need any other types for this app

            End Select

        Next

    End Sub
    Public Sub PopulateCarrierSelect(ByVal htmlSelect As HtmlSelect)

        Dim bucket As New RelationPredicateBucket
        Dim filter As New PredicateExpression()
        filter.Add(AlisdistCarrierFields.CcsmaySelect = True)
        bucket.PredicateExpression.Add(filter)

        Dim alisCarriers As EntityCollection = AlisdistCarrierManager.FetchCollection(bucket)

        Dim sortedCarriers = From carrier As AlisdistCarrierEntity In alisCarriers _
                     Order By carrier.CarrierName _
                     Select carrier

        For Each carrier In sortedCarriers
            htmlSelect.Items.Add(New ListItem(carrier.CarrierName, carrier.CarrierName))
        Next


    End Sub
    Public Sub PopulateCarrierTermSelect(ByVal htmlSelect As HtmlSelect)

        Dim terms As EntityCollection = AlisdistCarrierTermManager.FetchCollection()

        Dim sortedTerms = From term As AlisdistCarrierTermEntity In terms _
                          Order By term.TermDescription _
                          Select term

        For Each term In sortedTerms
            htmlSelect.Items.Add(New ListItem(term.TermDescription, term.TermDescription))
        Next

    End Sub
    Public Sub PopulatePaymentMethodSelect(ByVal htmlSelect As HtmlSelect)

        For Each paymentMethod As PaymentMethodType In [Enum].GetValues(GetType(PaymentMethodType))

            Select Case paymentMethod
                Case PaymentMethodType.PURCHASE_ORDER
                    htmlSelect.Items.Add(New ListItem("Purchase Order", "Purchase Order"))
                Case PaymentMethodType.VISA
                    htmlSelect.Items.Add(New ListItem("VISA", "VISA"))
                Case PaymentMethodType.MASTERCARD
                    htmlSelect.Items.Add(New ListItem("MASTERCARD", "MASTERCARD"))
                Case PaymentMethodType.AMERICAN_EXPRESS
                    htmlSelect.Items.Add(New ListItem("AMERICAN EXPRESS", "AMERICAN EXPRESS"))
                Case Else
                    'nothing to add, do't need any other types for this app

            End Select

        Next


    End Sub
    Private Enum AlisOrderType
        [Billing_Only] = 82
        Product = 80
        Quote = 154
        [Shipping_Only] = 81
    End Enum

    Private Enum PaymentMethodType

        [PURCHASE_ORDER]
        [VISA]
        [MASTERCARD]
        [AMERICAN_EXPRESS]
        '[Cyber_Cash]
        '[Future_Credit_Card]

    End Enum

End Class
