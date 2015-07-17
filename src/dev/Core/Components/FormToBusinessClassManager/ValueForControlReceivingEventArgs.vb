Imports System.ComponentModel

''' <summary>
''' Kapselt die Ereignisparameter für das <see cref="EntitiesFormsLib.FormToBusinessClassManager.ValueForControlReceiving">ValueForControlReceiving-Ereignis</see>.
''' </summary>
''' <remarks></remarks>
Public Class ValueForControlReceivingEventArgs
    Inherits CancelEventArgs

    Sub New(ByVal cancel As Boolean, ByVal bindingControl As INullableValueDataBinding, ByVal value As Object)
        MyBase.new(cancel)
        Me.BindingControl = bindingControl
        Me.Value = value
    End Sub

    ''' <summary>
    ''' Steuerelement, dass den Wert zur Verfügung stellt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BindingControl As INullableValueDataBinding

    ''' <summary>
    ''' Aktueller Wert der Value-Eigenschaft des Steuerelements.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Value As Object

End Class

