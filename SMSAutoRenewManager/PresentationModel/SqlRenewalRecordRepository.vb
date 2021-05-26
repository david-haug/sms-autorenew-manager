Imports System.Data.SqlClient
Public Class SqlRenewalRecordRepository

    Public Function FetchAll(ByVal startDate As DateTime, ByVal endDate As DateTime) As IList(Of RenewalRecord)

        Dim renewals As New List(Of RenewalRecord)

        Dim sql = "AutoRenewalUnprocessedByEndDate"
        Dim con As New SqlConnection(ConfigurationManager.AppSettings("SMS.ConnectionString"))
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate


        Using con
            con.Open()
            Dim reader = cmd.ExecuteReader

            While reader.Read

                Dim renewal As New RenewalRecord
                renewal.Subscription.SubscriptionId = reader("SubscriptionID")
                renewal.Subscription.EndDate = reader("EndDate")
                renewal.Subscriber.AccountCode = reader("AccountCode")
                renewal.Subscriber.LocationName = reader("LocationName")
                renewal.Subscription.AlisInvoiceNumber = reader("InvoiceNumber")

                renewals.Add(renewal)
            End While

            con.Close()
        End Using


        Return renewals


    End Function

End Class
