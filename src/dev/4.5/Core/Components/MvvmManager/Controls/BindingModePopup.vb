Imports System.ComponentModel
Imports System.Windows

''' <summary>
''' Steuerelement zur aufklappbaren Darstellung von Elementen in DataGridView-Listen, das überdies Null-Werte verarbeitet, 
''' Such- und Auto-Complete-Funktionalitäten sowie eine vereinheitlichende Value-Eigenschaft bietet, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
<ToolboxItem(False)>
Public Class BindingSettingPopup
    Inherits TextBoxPopup

    Public Event BindingSettingChanged(sender As Object, e As EventArgs)

    Private myBindingModePopupForm As BindingModePopupView
    Private myBindingSetting As BindingSetting

    Sub New()
        MyBase.New()
        myBindingSetting = New BindingSetting(MvvmBindingModes.TwoWay, UpdateSourceTriggerSettings.LostFocus)
        Me.TextBoxPart.ReadOnly = True
        Me.TextBoxPart.Text = myBindingSetting.ToString
    End Sub


    Protected Overrides Function GetPopupContent() As System.Windows.Forms.Control
        If myBindingModePopupForm Is Nothing Then
            myBindingModePopupForm = New BindingModePopupView With {.BindingSetting = Me.BindingSetting}
            AddHandler myBindingModePopupForm.PopupCloseRequested,
                Sub(sender, e)
                    If Me.IsPopupOpen Then
                        Me.ClosePopup()
                    End If
                End Sub

        End If
        Return myBindingModePopupForm
    End Function

    Protected Overrides Sub OnPopupClosed(e As EventArgs)
        MyBase.OnPopupClosed(e)
        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        If myBindingModePopupForm.BindingSetting.BindingMode <> Me.BindingSetting.BindingMode Or
           myBindingModePopupForm.BindingSetting.UpdateSourceTrigger <> Me.BindingSetting.UpdateSourceTrigger Then
            Me.BindingSetting = myBindingModePopupForm.BindingSetting
            OnBindingSettingChanged(EventArgs.Empty)
        End If
        Me.TextBoxPart.Text = myBindingSetting.ToString
    End Sub

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property BindingSetting As BindingSetting
        Get
            If myBindingModePopupForm IsNot Nothing Then
                myBindingSetting = myBindingModePopupForm.BindingSetting
            End If
            Return myBindingSetting
        End Get
        Set(value As BindingSetting)
            myBindingSetting = value
            If myBindingModePopupForm IsNot Nothing Then
                myBindingModePopupForm.BindingSetting = myBindingSetting
            End If
            Me.TextBoxPart.Text = myBindingSetting.ToString
        End Set
    End Property

    Protected Overridable Sub OnBindingSettingChanged(e As EventArgs)
        RaiseEvent BindingSettingChanged(Me, e)
    End Sub

End Class
