Imports System.Data.SqlClient

Public Class SqlAutoRenewEventRepository

    Public Function Save(ByVal autoRenewEvent As AutoRenewEvent) As Boolean

        Dim sql = "AutoRenewal_InsertEvent"
        Dim con As New SqlConnection(ConfigurationManager.AppSettings("SMS.ConnectionString"))
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("@SubscriptionID", SqlDbType.Int).Value = autoRenewEvent.SubscriptionID
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = autoRenewEvent.UserID
        cmd.Parameters.Add("@DateProcessed", SqlDbType.DateTime).Value = autoRenewEvent.DateProcessed
        cmd.Parameters.Add("@EventTypeID", SqlDbType.Int).Value = CInt(autoRenewEvent.EventType)

        Dim success As Boolean
        Try
            Using con
                con.Open()

                Dim result = cmd.ExecuteNonQuery()

                If result > 0 Then
                    success = True
                Else
                    success = False
                End If

                con.Close()
            End Using

            Return success
        Catch ex As Exception
            Throw New SMSAutoRenewException("Error saving RemoveRenewal event", ex)
        End Try



    End Function

End Class
