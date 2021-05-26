Public Class HtmlDataTable

    Sub New()

    End Sub
    Private _collection As List(Of IPresentationObject)
    Sub New(ByVal collection As List(Of IPresentationObject))

        _collection = collection

    End Sub

    Private _tableCode As String
    Public ReadOnly Property Code() As String
        Get
            Return _tableCode
        End Get
    End Property

    Private Function CreateHtmlCode() As String

        Dim table As String
        table = "<table>"

        For Each item In _collection

        Next


        'close table
        table += "</table>"



    End Function


End Class
