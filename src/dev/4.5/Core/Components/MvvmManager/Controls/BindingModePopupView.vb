Imports System.Windows.Forms

Public Class BindingModePopupView

    Private myBinding As BindingSetting = New BindingSetting With {.BindingMode = MvvmBindingModes.TwoWay,
                                                                   .UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus}

    Public Event PopupCloseRequested(sender As Object, e As PopupCloseRequestedEventArgs)

    Private Sub BindingModePopupView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For Each rButton As RadioButton In gbBindingMode.Controls
            If rButton.Name.Substring(2) Like myBinding.BindingMode.ToString Then
                rButton.Checked = True
                Exit For
            End If
        Next

        For Each rButton As ButtonBase In gbUpdateSourceTrigger.Controls
            If GetType(RadioButton).IsAssignableFrom(rButton.GetType) Then
                If rButton.Name.Substring(2) Like myBinding.UpdateSourceTrigger.ToString Then
                    DirectCast(rButton, RadioButton).Checked = True
                    Exit For
                End If
            End If
        Next
    End Sub

    Property BindingSetting As BindingSetting
        Get
            Dim bindingModeRadioButton = (From rButtonItem In gbBindingMode.Controls
                                          Where DirectCast(rButtonItem, RadioButton).Checked).SingleOrDefault

            Dim sourceTriggerRadioButton = (From rButtonItem In gbUpdateSourceTrigger.Controls
                                          Where GetType(RadioButton).IsAssignableFrom(rButtonItem.GetType) AndAlso
                                          DirectCast(rButtonItem, RadioButton).Checked).SingleOrDefault

            Dim bindingToReturn = New BindingSetting With
                      {.BindingMode = DirectCast([Enum].Parse(GetType(MvvmBindingModes), If(bindingModeRadioButton IsNot Nothing, DirectCast(bindingModeRadioButton, RadioButton).Name.Substring(2), "Default")), MvvmBindingModes),
                       .UpdateSourceTrigger = DirectCast([Enum].Parse(GetType(UpdateSourceTriggerSettings), If(sourceTriggerRadioButton IsNot Nothing, DirectCast(sourceTriggerRadioButton, RadioButton).Name.Substring(2), "Default")), UpdateSourceTriggerSettings)}

            If ValidatesCheckBox.Checked Then
                bindingToReturn.BindingMode = bindingToReturn.BindingMode Or MvvmBindingModes.ValidatesOnNotifyDataErrors
            End If

            Return bindingToReturn
        End Get
        Set(value As BindingSetting)

            Dim tmpBindingmode = value.BindingMode And Not MvvmBindingModes.ValidatesOnNotifyDataErrors

            Dim bindingModeRadioButton = (From rButtonItem In gbBindingMode.Controls
                                          Where DirectCast(rButtonItem, RadioButton).Name.Substring(2) = tmpBindingmode.ToString).SingleOrDefault

            Dim sourceTriggerRadioButton = (From rButtonItem In gbUpdateSourceTrigger.Controls
                                          Where GetType(RadioButton).IsAssignableFrom(rButtonItem.GetType) AndAlso
                                          DirectCast(rButtonItem, RadioButton).Name.Substring(2) = value.UpdateSourceTrigger.ToString).SingleOrDefault

            ValidatesCheckBox.Checked = value.BindingMode.HasFlag(MvvmBindingModes.ValidatesOnNotifyDataErrors)

        End Set
    End Property

    Protected Overridable Sub OnPopupCloseRequested(e As PopupCloseRequestedEventArgs)
        RaiseEvent PopupCloseRequested(Me, e)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.BindingSetting.ToString
    End Function

    Private Sub btnPopupDropUp_Click(sender As Object, e As EventArgs) Handles btnPopupDropUp.Click
        Dim popupCloseE As New PopupCloseRequestedEventArgs(PopupCloseRequestReasons.PopupFormRequest)
        OnPopupCloseRequested(popupCloseE)
    End Sub
End Class
