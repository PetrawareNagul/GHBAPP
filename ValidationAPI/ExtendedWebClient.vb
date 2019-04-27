Imports System.Net

Public Class ExtendedWebClient
    Inherits WebClient

    Protected Overrides Function GetWebRequest(ByVal uri As Uri) As WebRequest
        Dim w As WebRequest = MyBase.GetWebRequest(uri)
        w.Timeout = 1000
        Return w
    End Function
End Class
