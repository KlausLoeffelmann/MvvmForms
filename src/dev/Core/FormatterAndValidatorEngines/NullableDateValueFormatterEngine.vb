Imports System.Globalization

Public Class NullableDateValueFormatterEngine
    Inherits NullableValueFormatterEngineBase(Of Date)

    Private Shared myCombinedParseFormatStrings As String()
    Private Shared myDateParseFormatStrings As String()
    Private Shared myTimeParseFormatStrings As String()
    Private Shared myDateDisplayFormatStrings As String()
    Private Shared myTimeDisplayFormatStrings As String()

    Shared Sub New()

        If CultureInfo.CurrentCulture.TwoLetterISOLanguageName = "de" Then

            myCombinedParseFormatStrings = New String() {"ddM", "ddMM", "ddMMyy", "ddMMyyyy",
                         "d.M.y", "dd.M.y", "d.MM.y", "d.M.yy", "dd.M.yy", "dd.MM.yy", "d.M.yyyy",
                         "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy", "d,M,y", "dd,M,y", "d,MM,y", "d,M,yy",
                         "dd,M,yy", "dd,MM,yy", "d,M,yyyy", "dd,M,yyyy", "d,MM,yyyy", "dd,MM,yyyy",
                         "dddd, dd.MM.yyyy", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                         "dd.MM.yy HH:mm", "dd.MM.yyyy HH:mm",
                         "ddMMyy HHmm", "ddMMyyyy HHmm",
                         "dd.MM.yy HH:mm:ss", "dd.MM.yyyy HH:mm:ss",
                         "HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm",
                         "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss",
                         "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

            myDateParseFormatStrings = New String() {"ddM", "ddMM", "ddMMyy", "ddMMyyyy",
                 "dddd, dd.MM.yyyy",
                 "d.M.y", "dd.M.y", "d.MM.y", "d.M.yy", "dd.M.yy", "dd.MM.yy", "d.M.yyyy",
                 "dd.M.yyyy", "d.MM.yyyy", "dd.MM.yyyy", "d,M,y", "dd,M,y", "d,MM,y", "d,M,yy",
                 "dd,M,yy", "dd,MM,yy", "d,M,yyyy", "dd,M,yyyy", "d,MM,yyyy", "dd,MM,yyyy",
                 CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}

            myTimeParseFormatStrings = New String() {"HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm",
                "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss",
                "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

            myDateDisplayFormatStrings = New String() {"HH:mm",
                                                   "HH:mm:ss",
                                                   "dd.MM.yy",
                                                   "dddd, dd.MM.yyyy",
                                                   "dd.MM.yy - HH:mm",
                                                   "dddd, dd.MM.yyyy HH:mm:ss",
                                                   "dddd, \der dd. MMM yyyy"}

            myTimeDisplayFormatStrings = New String() {"HH:mm",
                                                   "HH:mm:ss",
                                                   "dd.MM.yy",
                                                   "dd.MM.yyyy",
                                                   "dd.MM.yy HH:mm",
                                                   "dd.MM.yyyy HH:mm:ss",
                                                   "dd.MM.yy HH:mm:ss"}
        Else

            myCombinedParseFormatStrings = New String() {"ddM", "ddMM", "ddMMyy", "ddMMyyyy",
                         "M.d.y", "M.dd.y", "MM.d.y", "M.d.yy", "M.dd.yy", "MM.dd.yy", "M.d.yyyy",
                         "M.dd.yyyy", "MM.d.yyyy", "MM.dd.yyyy", "M/d/y", "M/dd/y", "MM/d/y", "M/d/yy",
                         "M/dd/yy", "MM/dd/yy", "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy", "MM/dd/yyyy",
                         CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                         "MM/dd/yy HH:mm", "MM/dd/yyyy HH:mm",
                         "MMddyy HHmm", "MMddyyyy HHmm",
                         "MM/dd/yy HH:mm:ss", "MM/dd/yyyy HH:mm:ss",
                         "HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm",
                         "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss",
                         "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

            myDateParseFormatStrings = New String() {"Mdd", "MMdd", "MMddyy", "MMddyyyy",
                 "dddd, MM/dd/yyyy",
                 "M/d/y", "M/dd/y", "MM/d/y", "M/d/yy", "M/dd/yy", "MM/dd/yy", "M/d/yyyy",
                 "M/dd/yyyy", "MM/d/yyyy", "MM/dd/yyyy", "M.d.y", "M.dd.y", "MM.d.y", "M.d.yy",
                 "M.dd.yy", "MM.dd.yy", "M.d.yyyy", "M.dd.yyyy",
                 CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}

            myTimeParseFormatStrings = New String() {"HH", "HHmm", "HHmmss", "H.m", "H.mm", "HH.m", "HH.mm",
                "HH.mm.ss", "H:m", "H:mm", "HH:m", "HH:mm", "HH:mm:ss",
                "H,m", "H,mm", "HH,m", "HH,mm", "HH,mm,ss"}

            myDateDisplayFormatStrings = New String() {"HH:mm",
                                                   "HH:mm:ss",
                                                   "MM/dd/yy",
                                                   "dddd, MM/dd/yyyy",
                                                   "MM/dd/yy - HH:mm",
                                                   "dddd, MM/dd/yyyy HH:mm:ss"}

            myTimeDisplayFormatStrings = New String() {"HH:mm",
                                                   "HH:mm:ss",
                                                   "MM/dd/yy",
                                                   "MM/dd/yyyy",
                                                   "MM/dd/yy HH:mm",
                                                   "MM/dd/yyyy HH:mm:ss",
                                                   "MM/dd/yy HH:mm:ss"}

        End If


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