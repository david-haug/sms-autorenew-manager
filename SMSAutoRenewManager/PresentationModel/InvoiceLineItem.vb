Public Class InvoiceLineItem

    Public Product As String
    Public Quantity As Double
    Public Price As Double

    Sub New()

    End Sub

    Sub New(ByVal productName As String, ByVal quantity As Double, ByVal price As Double)
        Me.Product = productName
        Me.Price = price
        Me.Quantity = quantity
    End Sub

End Class
