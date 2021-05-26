Imports System.Data.SqlClient
Public Class SqlProcessedRenewalRepository

    Public Function FetchAll() As IList(Of ProcessedRenewal)

        Dim processedRenewals As New List(Of ProcessedRenewal)

        Dim sql = "select * from tblAutoRenewLog"
        Dim con As New SqlConnection(ConfigurationManager.AppSettings("SMS.ConnectionString"))
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandType = CommandType.Text
        'cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate
        'cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate


        Using con
            con.Open()
            Dim reader = cmd.ExecuteReader

            While reader.Read

                Dim processed As New ProcessedRenewal
                processed.Subscription.SubscriptionId = reader("SubscriptionID")
                processed.AlisOrderRef = reader("ALISOrderRefNum")
                processed.AutoRenewLogID = reader("AutoRenewLogID")
                processed.DateProcessed = reader("DateProcessed")


                Dim userService = New AutoRenewUserService
                processed.ProcessedByUserName = userService.FetchAlisUserFullName(reader("ProcessedByUserId"))
                processedRenewals.Add(processed)

            End While


            con.Close()
        End Using


        Return processedRenewals


    End Function
End Class
