Imports System.IO
Imports System.Net
Imports IniParser
Imports IniParser.Model
Imports Newtonsoft.Json

Public Class CustomValidator
    Public Shared logWriter As LogWriter = New LogWriter("Log Started")

    Public Shared Function TestFunction() As Boolean
        'Dim isBatchStatusPostSuccess As Boolean = SyncClient.Services.PostBatchStatusXMLFile("D:\SyncControlForBranch\ADCStatus\rpt-20181117112935125_20181117112935125_LOS-LA_SC01.xml")
        'Dim setsList As List(Of String) = Silverlake.SynClient.Services.PostBatchXmlToSplit("")
        'For Each setXMLPath As String In setsList
        '    Dim isSetPostSuccess As Boolean = Silverlake.SynClient.Services.PostSetFile(setXMLPath)
        'Next
        Return True
    End Function

    Public Shared Function IsValidJob(ByVal jobAndType As String) As Boolean
        Dim ValidJob As Boolean
        Dim parser = New FileIniDataParser()
        Dim data As IniData = parser.ReadFile("ValidationAPISettings.ini")
        Dim keyData As KeyData = data.[Global].GetKeyData(jobAndType)
        If keyData IsNot Nothing Then
            ValidJob = True
        Else
            ValidJob = False
        End If
        Return ValidJob
    End Function



    Public Shared Function IsValidNo(ByVal job As String, ByVal type As String, ByVal no As String) As AAValidateResponse
        Dim aAValidateResponse As AAValidateResponse = New AAValidateResponse()

        If job <> "" AndAlso type <> "" AndAlso no <> "" Then

            Try
                Dim parser = New FileIniDataParser()
                Dim data As IniData = parser.ReadFile("ValidationAPISettings.ini")
                Dim enable As String = data.[Global].GetKeyData("ENABLE").Value
                If enable <> "0" Then
                    Dim apiUrl As String = data.[Global].GetKeyData(job & type).Value

                    Using client = New ExtendedWebClient()
                        client.Headers.Clear()
                        client.Headers.Add("Content-Type:application/json;charset=utf-8")
                        Dim destinationUrl As String = ""

                        apiUrl = apiUrl.Replace("##NO##", no)
                        Dim uri = New Uri(apiUrl)
                        Dim response = client.DownloadString(uri)
                        If response <> "" Then
                            Dim responseString As String = response
                            aAValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AAValidateResponse)(responseString)
                        Else
                            aAValidateResponse.Result = "Connection failure"
                        End If
                    End Using
                Else
                    aAValidateResponse.AccountName = "Nagul"
                    aAValidateResponse.Result = "AA"
                End If
            Catch ex As Exception
                aAValidateResponse.Result = ex.Message
                logWriter = New LogWriter("AA and Ac validation : " + ex.Message + ", AA " + no)

            End Try
        Else
            aAValidateResponse.ReservedField = "No Job/Type/No"
            aAValidateResponse.Result = "AA"
        End If

        Return aAValidateResponse
    End Function

    Public Shared Function IsValidACandAA(ByVal job As String, ByVal type As String, ByVal no As String, ByVal aa As String) As AAValidateResponse
        Dim aAValidateResponse As AAValidateResponse = New AAValidateResponse()

        If job <> "" AndAlso type <> "" AndAlso no <> "" Then

            Try
                Dim parser = New FileIniDataParser()
                Dim data As IniData = parser.ReadFile("ValidationAPISettings.ini")
                Dim enable As String = data.[Global].GetKeyData("ENABLE").Value
                If enable <> "0" Then
                    Dim apiUrl As String = data.[Global].GetKeyData(job & type).Value
                    Using client = New ExtendedWebClient()
                        client.Headers.Clear()
                        client.Headers.Add("Content-Type:application/json;charset=utf-8")
                        Dim destinationUrl As String = ""

                        apiUrl = apiUrl.Replace("##NO##", no)

                        apiUrl = apiUrl.Replace("##AA##", aa)
                        Dim uri = New Uri(apiUrl)
                        Dim response = client.DownloadString(uri)
                        If response <> "" Then
                            Dim responseString As String = response
                            aAValidateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AAValidateResponse)(responseString)
                        Else
                            aAValidateResponse.Result = "Connection failure"
                        End If
                    End Using
                Else
                    aAValidateResponse.AccountName = "Nagul"
                    aAValidateResponse.Result = "AA"
                End If

            Catch ex As Exception
                aAValidateResponse.Result = ex.Message
                logWriter = New LogWriter("AA and Ac validation : " + ex.Message + ", AA " + aa + " , Account No " + no)
            End Try
        Else
            aAValidateResponse.ReservedField = "No Job/Type/No"
            aAValidateResponse.Result = "AA"
        End If

        Return aAValidateResponse
    End Function



End Class

Public Class AAValidateResponse
    <JsonProperty("Result")>
    Public Property Result As String
    <JsonProperty("Account Name")>
    Public Property AccountName As String
    <JsonProperty("Reserved Field")>
    Public Property ReservedField As String
End Class

