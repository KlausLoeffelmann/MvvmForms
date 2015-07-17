''' <summary>
''' Zentraler Manager der Standardwerte für alle NullableControl-Steuerelemente, der per Ereignis bindbar ist und damit eigene Standardwerte ermöglicht.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class NullableControlManager

    Private Shared myNullableDefaultProperyManagerInstance As NullableControlManager

    Public Shared Event RequestNullableControlDefaultValue(sender As Object, e As RequestNullableControlDefaultValueEventArgs)

    Private Sub New()
        MyBase.new()
    End Sub

    Public Shared Function GetInstance() As NullableControlManager
        If myNullableDefaultProperyManagerInstance Is Nothing Then
            myNullableDefaultProperyManagerInstance = New NullableControlManager
        End If
        Return myNullableDefaultProperyManagerInstance
    End Function

    Public Function GetDefaultNullValueString(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs("NullValueString", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Public Function GetDefaultGroupName(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs("DefaultGroupName", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultExceptionBalloonDuration(sender As Object, predefinedValue As Integer) As Integer
        Dim e As New RequestNullableControlDefaultValueEventArgs("ExceptionBalloonDuration", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CInt(e.Value)
    End Function

    Function GetDefaultOnUnassignableValueAction(sender As Object, predefinedValue As UnassignableValueAction) As UnassignableValueAction
        Dim e As New RequestNullableControlDefaultValueEventArgs("OnUnassignableValueAction", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, UnassignableValueAction)
    End Function

    Function GetDefaultFocusSelectionBehaviour(sender As Object, predefinedValue As FocusSelectionBehaviours) As FocusSelectionBehaviours
        Dim e As New RequestNullableControlDefaultValueEventArgs("FocusSelectionBehaviour", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, FocusSelectionBehaviours)
    End Function

    Function GetDefaultOnFocusColor(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs("OnFocusColor", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
    End Function

    Function GetDefaultFocusColor(sender As Object, predefinedValue As Drawing.Color) As Drawing.Color
        Dim e As New RequestNullableControlDefaultValueEventArgs("FocusColor", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Drawing.Color)
    End Function

    Function GetDefaultErrorColor(sender As Object, predefinedValue As Drawing.Color) As Drawing.Color
        Dim e As New RequestNullableControlDefaultValueEventArgs("ErrorColor", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Drawing.Color)
    End Function

    Function GetDefaultFormatString(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs("FormatString", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultBeepOnFailedValidation(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs("BeepOnFailedValidation", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
    End Function

    Function GetDefaultMinValue(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs("MinValue", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultIncrement(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs("Increment", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultMaxValue(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs("MaxValue", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultMinValueExceededMessage(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs("MinValueExceededMessage", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultMaxValueExceededMessage(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs("MaxValueExceededMessage", predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

End Class

Public Class RequestNullableControlDefaultValueEventArgs
    Inherits EventArgs

    Sub New(propertyName As String, value As Object)
        Me.PropertyName = propertyName
        Me.Value = value
    End Sub

    Public Property PropertyName As String
    Public Property Value As Object
End Class