Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' CONCEPTIONAL, DO NOT USE, YET.
''' </summary>
<ToolboxItem(False)>
Public Class NullableMaskTextValue
    Inherits NullableValueBase(Of StringValue, NullableValuePrimalTextBox)

    Private myEliminateWhitespacesOnAssignment As Boolean
    Private WithEvents myTBox As TextBox

    Sub New()
        MyBase.New()
        myTBox = MyBase.TextBoxPart
    End Sub

    Protected Overrides Function GetDefaultFormatString() As String
        Return ""
    End Function

    Protected Overrides Function GetDefaultFormatterEngine() As INullableValueFormatterEngine
        Dim retTmp = New NullableStringValueFormatterEngine(Me.Value, Me.GetDefaultFormatString, Me.NullValueString)
        Return (retTmp)
    End Function

    Protected Overrides Function GetDefaultNullValueString() As String
        Return DEFAULT_NULL_VALUE_STRING
    End Function

    Protected Overrides Sub InitializeProperties()
        myEliminateWhitespacesOnAssignment = True
    End Sub

    Protected Overrides Function IsMultiLineControl() As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine FormsToBusinessManager-Instanz die MaxLength-Eigenschaft zur Laufzeit auf Basis des Business-Objektes setzen darf.
    ''' </summary>
    ''' <value>Name des Attributs, dass über der zu bindenden Eigenschaft stehen muss, dass die Länge des Strings angibt.</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob eine FormsToBusinessManager-Instanz die MaxLength-Eigenschaft zur Laufzeit auf Basis des Business-Objektes setzen darf."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(True)>
    Public Shared Property MaxLengthAttributeName As String

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob WhiteSpaces (Füllzeichen) die vor oder hinter dem eigentlichen Text stehen, bei einer Zuweisung eliminiert werden sollen.
    ''' </summary>
    ''' <value>True, wenn WhiteSpaces bei der Zuweisung eliminiert werden sollen.</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob WhiteSpaces (Füllzeichen) die vor oder hinter dem eigentlichen Text stehen, bei einer Zuweisung eliminiert werden sollen."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(True)>
    Property EliminateWhitespacesOnAssignment As Boolean
        Get
            Return myEliminateWhitespacesOnAssignment
        End Get
        Set(ByVal value As Boolean)
            myEliminateWhitespacesOnAssignment = value
        End Set
    End Property

    'Sorgt dafür, dass die EliminateWhitespacesOnAssignment-Eigenschaft berücksichtigt wird.
    Protected Overrides Sub OnValueChanging(ByVal e As ValueChangingEventArgs(Of StringValue?))
        MyBase.OnValueChanging(e)
        If EliminateWhitespacesOnAssignment Then
            If e.OriginalValue IsNot Nothing Then
                'Wir arbeiten bei Strings mit StringValue, deswegen auf Typgleichheit achten!
                If e.OriginalValue.GetType Is GetType(StringValue) Then
                    e.NewValue = New StringValue(e.OriginalValue.ToString.Trim)
                Else
                    'Eigentlich dürften wir hier nie hinkommen.
                    e.NewValue = e.OriginalValue.ToString.Trim
                End If
            End If
        End If
    End Sub

    Private Sub myTBox_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles myTBox.KeyPress

    End Sub
End Class
