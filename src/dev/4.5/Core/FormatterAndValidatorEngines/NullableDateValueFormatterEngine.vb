Imports System.Globalization

Public Class NullableDateValueFormatterEngine
    Inherits NullableValueFormatterEngineBase(Of Date)

    Private Shared myCombinedParseFormatStrings As String()
    Private Shared myDateParseFormatStrings As String()
    Private Shared myTimeParseFormatStrings As String()
    Private Shared myDateDisplayFormatStrings As String()
    Private Shared myTimeDisplayFormatStrings As String()

    Shared Sub New()

        myCombinedParseFormatStrings = New String() {"ddM", "ddMM", "ddMMyy", "ddMMyyyy", _
                         "d.M.y", "dd.M.y", "d.MM.y", "d.M.yy", "dd.M.yy", "dd.MM.yy", "d.M.yyyy", _
                         "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy", "d,M,y", "dd,M,y", "d,MM,y", "d,M,yy", _
                         "dd,M,yy", "dd,MM,yy", "d,M,yyyy", "dd,M,yyyy", "d,MM,yyyy", "dd,MM,yyyy", _
                         "dddd, dd.MM.yyyy", _
                         "dd.MM.yy HH:mm", "dd.MM.yyyy HH:mm", _
                         "ddMMyy HHmm", "ddMMyyyy HHmm", _
                         "dd.MM.yy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", _
                         "HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm", _
                         "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss", _
                         "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

        myDateParseFormatStrings = New String() {"ddM", "ddMM", "ddMMyy", "ddMMyyyy", _
                 "dddd, dd.MM.yyyy", _
                 "d.M.y", "dd.M.y", "d.MM.y", "d.M.yy", "dd.M.yy", "dd.MM.yy", "d.M.yyyy", _
                 "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy", "d,M,y", "dd,M,y", "d,MM,y", "d,M,yy", _
                 "dd,M,yy", "dd,MM,yy", "d,M,yyyy", "dd,M,yyyy", "d,MM,yyyy", "dd,MM,yyyy"}

        myTimeParseFormatStrings = New String() {"HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm", _
                "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss", _
                "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

        myDateDisplayFormatStrings = New String() {"HH:mm", _
                                                    "HH:mm:ss", _
                                                    "dd.MM.yy", _
                                                    "dddd, dd.MM.yyyy", _
                                                    "dd.MM.yy - HH:mm", _
                                                    "dddd, dd.MM.yyyy HH:mm:ss", _
                                                    "dddd, \der dd. MMM yyyy"}

        myTimeDisplayFormatStrings = New String() {"HH:mm", _
                                                    "HH:mm:ss", _
                                                    "dd.MM.yy", _
                                                    "dd.MM.yyyy", _
                                                    "dd.MM.yy HH:mm", _
                                                    "dd.MM.yyyy HH:mm:ss", _
                                                    "dd.MM.yy HH:mm:ss"}

    End Sub

    Sub New(ByVal value As Date?)
        MyBase.New(value)
    End Sub

    Sub New(ByVal value As Date?, ByVal FormatString As String)
        MyBase.New(value, FormatString)
    End Sub

    Sub New(ByVal value As Date?, ByVal FormatString As String, ByVal NullValueString As String)
        MyBase.New(value, FormatString, NullValueString)
    End Sub

    Public Overrides Function ConvertToDisplay() As String
        If Me.Value.HasValue Then
            Dim valueAsString = Me.Value.Value.ToString(Me.FormatString)
            Return valueAsString
        Else
            Return Me.NullValueString
        End If
    End Function

    Public Overrides Function ConvertToValue(ByVal value As String) As Date?
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        End If
        Return Date.ParseExact(value, myDateParseFormatStrings, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AllowWhiteSpaces)
    End Function

    Public Overrides Function InitializeEditedValue() As String
        If Me.Value.HasValue Then
            Return Me.Value.Value.ToShortDateString
        Else
            Return ""
        End If
    End Function

    Public Overrides Function Validate(ByVal text As String) As System.Exception
        Try
            If String.IsNullOrEmpty(text) Then
                Return Nothing
            End If
            Date.ParseExact(text, myDateParseFormatStrings, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AllowWhiteSpaces)
            Return Nothing
        Catch ex As Exception
            Return ex
        End Try
    End Function
End Class