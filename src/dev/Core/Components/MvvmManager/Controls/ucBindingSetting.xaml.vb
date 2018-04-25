'*****************************************************************************************
'                                    ucBindingSetting.vb
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
Imports ActiveDevelop.EntitiesFormsLib

Public Class ucBindingSetting


    Public Property PropertyBindingItem As PropertyBindingItem
        Get
            Return DirectCast(GetValue(BindingSettingsProperty), PropertyBindingItem)
        End Get

        Set(ByVal value As PropertyBindingItem)
            SetValue(BindingSettingsProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BindingSettingsProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(PropertyBindingItem),
                           GetType(PropertyBindingItem), GetType(ucBindingSetting),
                           New PropertyMetadata(AddressOf BindingSettingChanged))

    Private Sub rbModeChanged(sender As Object, e As RoutedEventArgs) Handles rbTwoWay.Checked, rbOneWay.Checked, rbOneTime.Checked, rbOneWayToSource.Checked
        If Not _loadingDone Then Return
        Dim val = MvvmBindingModes.NotSet
        If rbOneTime.IsChecked Then
            val = MvvmBindingModes.OneTime
        ElseIf rbOneWay.IsChecked Then
            val = MvvmBindingModes.OneWay
        ElseIf rbOneWayToSource.IsChecked Then
            val = MvvmBindingModes.OneWayToSource
        ElseIf rbTwoWay.IsChecked Then
            val = MvvmBindingModes.TwoWay
        Else
            val = MvvmBindingModes.NotSet
        End If

        Dim v = Me.PropertyBindingItem.BindingSetting
        v.BindingMode = val
        Me.PropertyBindingItem.BindingSetting = v

        'Debug.WriteLine($"Change Mode to {val}/{Me.PropertyBindingItem.BindingSetting.BindingMode}")
    End Sub

    Private Sub rbTriggerChanged(sender As Object, e As RoutedEventArgs) Handles rbLostFocus.Checked, rbExplicit.Checked, rbImmediate.Checked
        If Not _loadingDone Then Return
        Dim val As UpdateSourceTriggerSettings = DirectCast(-1, UpdateSourceTriggerSettings)
        If rbExplicit.IsChecked Then
            val = UpdateSourceTriggerSettings.Explicit
        ElseIf rbLostFocus.IsChecked Then
            val = UpdateSourceTriggerSettings.LostFocus
        ElseIf rbImmediate.IsChecked Then
            val = UpdateSourceTriggerSettings.PropertyChangedImmediately
        End If
        Dim v = Me.PropertyBindingItem.BindingSetting
        v.UpdateSourceTrigger = val
        Me.PropertyBindingItem.BindingSetting = v
        'Debug.WriteLine($"Change Trigger to {val}/{Me.PropertyBindingItem.BindingSetting.UpdateSourceTrigger}")
    End Sub



    Private Shared Sub BindingSettingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim ucBS = TryCast(d, ucBindingSetting)
        If ucBS Is Nothing Then Return
        Dim bs = DirectCast(e.NewValue, PropertyBindingItem)

        Select Case bs.BindingSetting.BindingMode
            Case MvvmBindingModes.OneTime
                ucBS.rbOneTime.IsChecked = True
            Case MvvmBindingModes.OneWay
                ucBS.rbOneWay.IsChecked = True
            Case MvvmBindingModes.OneWayToSource
                ucBS.rbOneWayToSource.IsChecked = True
            Case MvvmBindingModes.TwoWay
                ucBS.rbTwoWay.IsChecked = True
        End Select
        Select Case bs.BindingSetting.UpdateSourceTrigger
            Case UpdateSourceTriggerSettings.Explicit
                ucBS.rbExplicit.IsChecked = True
            Case UpdateSourceTriggerSettings.LostFocus
                ucBS.rbLostFocus.IsChecked = True
            Case UpdateSourceTriggerSettings.PropertyChangedImmediately
                ucBS.rbImmediate.IsChecked = True
        End Select

    End Sub

    Private _loadingDone As Boolean = False
    Private Sub userControl_Loaded(sender As Object, e As RoutedEventArgs) Handles userControl.Loaded
        _loadingDone = True
    End Sub
End Class
