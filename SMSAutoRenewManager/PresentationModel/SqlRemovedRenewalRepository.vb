Imports System.Data.SqlClient

Public Class SqlRemovedRenewalRepository
    Public Function FetchAll() As IList(Of RemovedRenewal)

        Dim removedRenewals As New List(Of RemovedRenewal)

        Dim sql = "AutoRenewal_FetchAllRemoved"
        Dim con As New SqlConnection(ConfigurationManager.AppSettings("SMS.ConnectionString"))
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandType = CommandType.Text
        'cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate
        'cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate


        Using con
            con.Open()
            Dim reader = cmd.ExecuteReader

            While reader.Read

                Dim removed As New RemovedRenewal
                removed.Subscription.SubscriptionId = reader("SubscriptionID")
                removed.DateProcessed = reader("DateProcessed")
                removed.AccountCode = reader("AccountCode")
                removed.CompanyName = reader("CompanyName")
                removed.InvoiceNumber = reader("InvoiceNumber")

                Dim userService = New AutoRenewUserService
                removed.ProcessedByUserName = userService.FetchAlisUserFullName(reader("UserID"))
                removedRenewals.Add(removed)

            End While


            con.Close()
        End Using


        Return removedRenewals


    End Function
End Class
