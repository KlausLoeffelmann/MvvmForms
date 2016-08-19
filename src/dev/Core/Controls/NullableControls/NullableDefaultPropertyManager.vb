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
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).NullValueString), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Public Function GetDefaultGroupName(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                 NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).GroupName), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultExceptionBalloonDuration(sender As Object, predefinedValue As Integer) As Integer
        Dim e As New RequestNullableControlDefaultValueEventArgs(
            NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).ExceptionBalloonDuration), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CInt(e.Value)
    End Function

    Function GetDefaultOnUnassignableValueAction(sender As Object, predefinedValue As UnassignableValueAction) As UnassignableValueAction
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                NameOf(BindableDataGridView.OnUnassignableValueAction), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, UnassignableValueAction)
    End Function

    Function GetDefaultFocusSelectionBehaviour(sender As Object, predefinedValue As FocusSelectionBehaviours) As FocusSelectionBehaviours
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).FocusSelectionBehaviour), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, FocusSelectionBehaviours)
    End Function

    Function GetDefaultOnFocusColor(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs(
            NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).OnFocusColor), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
    End Function

    Function GetDefaultFocusColor(sender As Object, predefinedValue As Drawing.Color) As Drawing.Color
        Dim e As New RequestNullableControlDefaultValueEventArgs(
               NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).FocusColor), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Drawing.Color)
    End Function

    Function GetDefaultErrorColor(sender As Object, predefinedValue As Drawing.Color) As Drawing.Color
        Dim e As New RequestNullableControlDefaultValueEventArgs(
            NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).ErrorColor), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Drawing.Color)
    End Function

    Function GetDefaultFormatString(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).FormatString), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultBeepOnFailedValidation(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs(
                        NameOf(NullableValueBase(Of Integer, NullableValuePrimalTextBox).BeepOnFailedValidation),
                           predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
    End Function

    Function GetDefaultMinValue(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.MinValue), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultIncrement(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.Increment), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultMaxValue(sender As Object, predefinedValue As Decimal?) As Decimal?
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.MaxValue), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CType(e.Value, Decimal?)
    End Function

    Function GetDefaultMinValueExceededMessage(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.MinValueExceededMessage), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultMaxValueExceededMessage(sender As Object, predefinedValue As String) As String
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.MaxValueExceededMessage), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return e.Value.ToString
    End Function

    Function GetDefaultAllowFormular(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs(NameOf(NullableNumValue.AllowFormular), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
    End Function

    Function GetDefaultImitateTabByPageKeys(sender As Object, predefinedValue As Boolean) As Boolean
        Dim e As New RequestNullableControlDefaultValueEventArgs(
            NameOf(NullableValueBase(Of StringValue, NullableValuePrimalTextBox).ImitateTabByPageKeys), predefinedValue)
        RaiseEvent RequestNullableControlDefaultValue(sender, e)
        Return CBool(e.Value)
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