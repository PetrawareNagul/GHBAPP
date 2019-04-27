Imports System.IO
Imports ValidationAPI

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim res As AAValidateResponse = CustomValidator.IsValidNo("ETP-LN", "AA", "301091343001")
        Dim res2 As AAValidateResponse = CustomValidator.IsValidACandAA("ETP-LN", "AC", "407570001380", "301091343001")

        '############### Post Batch Status File (Total 3 status from here)
        Dim isPosted As Boolean = SyncClient.Services.PostBatchStatusXMLFile("D:\SyncControlForBranch\ADCStatus\rpt-20181130141518062_20181130141251146_E-LIBRARY_SLB1.xml")
        '############### Post Batch XML to Split into Sets and Combine Tiffs into PDF
        Dim setXmls As List(Of String) = Services.PostBatchXmlToSplit("D:\SyncControlForBranch\FromADC\20181130141251146_E-LIBRARY_SLB1.xml")
        '############### Post Set Files
        Dim batchKey As String = ""
        For Each setXML As String In setXmls
            Dim fileName As String = Path.GetFileName(setXML)
            batchKey = fileName.Split(New Char() {"_"})(0)
            Dim isPostedSetXML As Boolean = Services.PostSetFile(setXML)
        Next
        '############### Post Batch Status - Transmission Done
        Services.UpdateBatchStatusDone(batchKey)
    End Sub
End Class
