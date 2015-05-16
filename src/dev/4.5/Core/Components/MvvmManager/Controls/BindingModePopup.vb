'*****************************************************************************************
'                                    BindingModePopup.vb
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
