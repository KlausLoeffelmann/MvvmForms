Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem("False")>
Public Class NullableValueContainerBase
    Inherits UserControl

    Private myIsDirty As Boolean

    Public Event IsDirtyChanged(sender As Object, e As EventArgs)

    Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        If GetType(IIsDirtyChangedAware).IsAssignableFrom(e.Control.GetType) Then
            AddHandler DirectCast(e.Control, IIsDirtyChangedAware).IsDirtyChanged, AddressOf IsDirtyChangedEventHandlerProc
        End If
    End Sub

    Protected Overrides Sub OnControlRemoved(e As ControlEventArgs)
        MyBase.OnControlRemoved(e)
        If GetType(IIsDirtyChangedAware).IsAssignableFrom(e.Control.GetType) Then
            RemoveHandler DirectCast(e.Control, IIsDirtyChangedAware).IsDirtyChanged, AddressOf IsDirtyChangedEventHandlerProc
        End If
    End Sub

    Protected Overridable Sub OnIsDirtyChanged(e As EventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    Public Overridable Property IsDirty As Boolean
        Get
            Return myIsDirty
        End Get
        Set(value As Boolean)
            myIsDirty = value
        End Set
    End Property

    Private Sub IsDirtyChangedEventHandlerProc(sender As Object, e As EventArgs)
        If DirectCast(sender, IIsDirtyChangedAware).IsDirty <> IsDirty Then
            IsDirty = DirectCast(sender, IIsDirtyChangedAware).IsDirty
        End If
    End Sub

End Class
