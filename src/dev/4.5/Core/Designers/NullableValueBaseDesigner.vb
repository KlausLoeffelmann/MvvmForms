Imports System.Drawing

Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports System.Windows.Forms.Design.Behavior

Imports System.ComponentModel
Imports System.ComponentModel.Design

Public Class TextBoxBasedControlDesigner
    Inherits ControlDesigner

    Public Overrides Sub InitializeNewComponent(ByVal defaultValues As IDictionary)
        MyBase.InitializeNewComponent(defaultValues)

        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        'Wir schauen, ob es eine UIGuid-Eigenscht gibt, und setzen diese dann.
        Dim descriptor As PropertyDescriptor = TypeDescriptor.GetProperties(MyBase.Component).Item("UIGuid")
        If (((Not descriptor Is Nothing) AndAlso
             (descriptor.PropertyType Is GetType(Guid))) AndAlso
                (Not descriptor.IsReadOnly AndAlso descriptor.IsBrowsable)) Then
            descriptor.SetValue(MyBase.Component, Guid.NewGuid)
        End If
    End Sub

    Public Overrides ReadOnly Property SelectionRules As SelectionRules
        Get
            Dim control = TryCast(Me.Control, INullableValueEditor)
            If control IsNot Nothing AndAlso control.IsMultilineControl Then
                Return MyBase.SelectionRules
            Else
                Return (MyBase.SelectionRules And (Not (SelectionRules.BottomSizeable Or SelectionRules.TopSizeable)))
            End If
        End Get
    End Property

    Public Overrides ReadOnly Property SnapLines() As IList
        Get
            Dim bsOffset = 0

            Dim mySnapLines As IList = MyBase.SnapLines

            ' Rausfinden, ob das selektierte Steuerelement TextBoxbasierend ist.
            Dim control = DirectCast(Me.Control, ITextBoxBasedControl)
            If control Is Nothing Then
                Return MyBase.SnapLines
            Else
                'Per Reflection rausfinden, ob das entsprechende Steuerelement
                'eine BorderStyle-Eigenschaft hat, weil sich dann die innere
                'TextBox um ein zwei Pixel nach unten verschiebt:
                Dim propInfo = control.GetType.GetProperty("Borderstyle")
                If propInfo IsNot Nothing Then
                    Dim bstemp = (DirectCast(propInfo.GetValue(control, Nothing), BorderStyle))
                    If bstemp = BorderStyle.None Then
                        bsOffset = 0
                    Else
                        bsOffset = 2
                    End If
                End If

                'Schauen, ob wird zusätzliches Offset für die Baseline benötigen:
                Dim addOffsetControl = TryCast(control, IRequestAdditionalSnapBaselineOffset)
                If addOffsetControl IsNot Nothing Then
                    bsOffset += addOffsetControl.AdditionalSnapBaselineOffset
                End If
            End If

            'Probieren, den Designer für diese Textbox zu ermitteln.
            Dim designer As IDesigner = TypeDescriptor.CreateDesigner(
                control.TextBoxPart, GetType(IDesigner))

            If designer Is Nothing Then
                Return MyBase.SnapLines
            End If

            ' Designer mit der entsprechenden Instant der Textbox initialisieren.
            designer.Initialize(control.TextBoxPart)

            Dim boxDesigner = DirectCast(designer, ControlDesigner)
            If boxDesigner Is Nothing Then
                Return MyBase.SnapLines
            End If

            'Die Snaplines der TextBox auslesen und in unser SnapLines-Array übertragen.
            Dim line As SnapLine
            For Each line In boxDesigner.SnapLines
                If line.SnapLineType = SnapLineType.Baseline Then
                    mySnapLines.Add(New SnapLine(SnapLineType.Baseline,
                        line.Offset + bsOffset + control.TextBoxPart.Top,
                        line.Filter, line.Priority))
                    Exit For
                End If
            Next

            designer.Dispose()
            Return mySnapLines
        End Get
    End Property
End Class
