Public Class SMSAutoRenewException
    Inherits System.ApplicationException

    Public Sub New()
        LogError()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
        LogError()
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Private Sub LogError()

        'EventLog.WriteEntry("SMSAutoRenewManager", "MESSAGE: " & MyBase.Message & MyBase.StackTrace)

    End Sub



End Class
