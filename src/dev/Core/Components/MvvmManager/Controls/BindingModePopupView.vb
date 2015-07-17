'*****************************************************************************************
'                                    BindingModePopupView.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.ComponentModel
Imports System.Windows.Forms

<ToolboxItem(False)>
Public Class BindingModePopupView

    Private myBinding As BindingSetting = New BindingSetting With {.BindingMode = MvvmBindingModes.TwoWay,
                                                                   .UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus}

    Public Event PopupCloseRequested(sender As Object, e As PopupCloseRequestedEventArgs)

    Private Sub BindingModePopupView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ReflectBindingProperty(myBinding)

        'For Each rButton As RadioButton In gbBindingMode.Controls
        '    If rButton.Name.Substring(2) Like myBinding.BindingMode.ToString Then
        '        rButton.Checked = True
        '        Exit For
        '    End If
        'Next

        'For Each rButton As ButtonBase In gbUpdateSourceTrigger.Controls
        '    If GetType(RadioButton).IsAssignableFrom(rButton.GetType) Then
        '        If rButton.Name.Substring(2) Like myBinding.UpdateSourceTrigger.ToString Then
        '            DirectCast(rButton, RadioButton).Checked = True
        '            Exit For
        '        End If
        '    End If
        'Next
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
            If Not Object.Equals(myBinding, value) Then
                myBinding = value
                ReflectBindingProperty(value)
            End If
        End Set
    End Property

    Private Sub ReflectBindingProperty(value As BindingSetting)
        Dim tmpBindingmode = value.BindingMode And Not MvvmBindingModes.ValidatesOnNotifyDataErrors

        Dim bindingModeRadioButton = (From rButtonItem In gbBindingMode.Controls
                                      Where DirectCast(rButtonItem, RadioButton).Name.Substring(2) = tmpBindingmode.ToString).SingleOrDefault

        DirectCast(bindingModeRadioButton, RadioButton).Checked = True

        Dim sourceTriggerRadioButton = (From rButtonItem In gbUpdateSourceTrigger.Controls
                                        Where GetType(RadioButton).IsAssignableFrom(rButtonItem.GetType) AndAlso
                                          DirectCast(rButtonItem, RadioButton).Name.Substring(2) = value.UpdateSourceTrigger.ToString).SingleOrDefault

        DirectCast(sourceTriggerRadioButton, RadioButton).Checked = True

        ValidatesCheckBox.Checked = value.BindingMode.HasFlag(MvvmBindingModes.ValidatesOnNotifyDataErrors)

    End Sub

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
