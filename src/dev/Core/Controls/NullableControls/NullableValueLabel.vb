Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Steuerelement zur Erfassungs von Texten (Zeichenketten), das überdies Null-Werte verarbeitet, 
''' eine vereinheitlichende Value-Eigenschaft bietet, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
<ToolboxItem(True), ToolboxBitmap(GetType(Label))>
Public Class NullableValueLabel
    Inherits NullableValueBase(Of StringValue,
                               NullableValuePrimalTextBox)

    Private myEliminateWhitespacesOnAssignment As Boolean

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
        Me.TextBoxPart.ReadOnly = True
    End Sub

    Protected Overrides Function IsMultiLineControl() As Boolean
        Return False
    End Function

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


    ''' <summary>
    ''' Bestimmt oder ermittelt, wie der Text innerhalb des Controls formatiert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, wie der Text innerhalb des Controls formatiert werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(HorizontalAlignment.Left)>
    Property Alignment As HorizontalAlignment
        Get
            Return Me.TextBoxPart.TextAlign
        End Get
        Set(value As HorizontalAlignment)
            Me.TextBoxPart.TextAlign = value
        End Set
    End Property

End Class
