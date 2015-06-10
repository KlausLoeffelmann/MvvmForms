'*****************************************************************************************
'                                    PropertyBindingClasses.vb
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

Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Data

#Disable Warning
<Serializable, DesignerSerializer(GetType(PropertyBindingsCodeDomSerializer),
                                  GetType(CodeDomSerializer))>
Public Class PropertyBindings
    Inherits List(Of PropertyBindingItem)

#Enable Warning

    Sub New()
        MyBase.New()
    End Sub

    Sub New(enumList As IEnumerable(Of PropertyBindingItem))
        For Each enumItem In enumList
            Me.Add(enumItem)
        Next
    End Sub

    Public Function ToObservableItemList() As ObservableBindingList(Of PropertyBindingItem)
        Return New ObservableBindingList(Of PropertyBindingItem)(Me)
    End Function

    Public Overloads Sub Add(bindingMode As BindingSetting, controlProperty As BindingProperty,
                             converter As Type, converterParameter As String, viewModelProperty As BindingProperty)

        Dim propBindItem As New PropertyBindingItem() With
                    {.BindingSetting = bindingMode,
                     .ControlProperty = controlProperty,
                     .Converter = converter,
                     .ConverterParameter = converterParameter,
                     .ViewModelProperty = viewModelProperty}
        Me.Add(propBindItem)
    End Sub

    Public Function Clone() As PropertyBindings
        Dim retList As New PropertyBindings
        Me.ForEach(Sub(item)
                       retList.Add(item.Clone)
                   End Sub)
        Return retList
    End Function
End Class

<Serializable>
Public Class PropertyBindingItem

    Private myConverterInstance As IValueConverter

    Public Property ControlProperty As BindingProperty
    Public Property Converter As Type
    Public Property ConverterParameter As String
    Public Property BindingSetting As BindingSetting
    Public Property ViewModelProperty As BindingProperty
    Public Property IsDirty As Boolean
    Friend Property UpdatingViewmodelInProgress As Boolean
    Friend Property UpdatingControlInProgress As Boolean

    <Xml.Serialization.XmlIgnore>
    Friend Property ControlPropertyChangedEventHandlerTarget As Action(Of BindingPropertyChangedEventArgs)

    <Xml.Serialization.XmlIgnore>
    Friend Property ControlLostFocusEventHandlerTarget As Action(Of BindingPropertyChangedEventArgs)

    <Xml.Serialization.XmlIgnore>
    Friend Property ViewModelPropertyChangedEventHandlerTarget As Action(Of BindingPropertyChangedEventArgs)

    <Xml.Serialization.XmlIgnore>
    Friend Property ViewModelPropertyPathPartChangedEventHandlerTarget As Action(Of BindingPropertyChangedEventArgs)

    Friend ReadOnly Property ConverterInstance As IValueConverter
        Get
            If myConverterInstance Is Nothing Then
                'TODO: Converter ist ein Type - das wird nicht funktionieren.
                If TryCast(Converter, IValueConverter) IsNot Nothing Then
                    myConverterInstance = DirectCast(Activator.CreateInstance(Converter), IValueConverter)
                End If
            End If
            Return myConverterInstance
        End Get
    End Property

    Property PropertyBindingManager As PropertyBindingManager

    'Der Eventhandler für PropertyChange liegt in dieser Klasse, damit das Dirty-Handling hier erfolgen kann.
    Friend Sub ControlPropertyChangedEventHandler(sender As Object, e As EventArgs)

        'Wenn PropertyChanged nicht direkt eine Bindung auslöst, dann erstmal merken, dass diese Bindung Dirty ist und später berücksichtigt werden muss,
        'wenn der eigentliche SourceTrigger ausgelöst wird.
        If BindingSetting.UpdateSourceTrigger <> UpdateSourceTriggerSettings.PropertyChangedImmediately Then
            IsDirty = True
            'TODO: Hier müsste der Timer neu gesetzt werden für die verzögerte Auslösung von PropertyChanged.
        Else
            'Wenn Propertychange direkt gebunden ist, dann Ereignis weiterleiten.
            If ControlPropertyChangedEventHandlerTarget IsNot Nothing Then
                Dim eArgs As New BindingPropertyChangedEventArgs
                eArgs.Converter = ConverterInstance
                eArgs.OriginalSource = sender
                eArgs.EventProperty = ControlProperty.PropertyName
                ControlPropertyChangedEventHandlerTarget.Invoke(eArgs)
            End If
        End If

    End Sub

    Public Function Clone() As PropertyBindingItem
        Dim copy = DirectCast(MyBase.MemberwiseClone, PropertyBindingItem)
        'Diese hier müssen extra gecloned werden, da es Referenztypen sind.
        copy.ControlProperty = copy.ControlProperty.Clone
        copy.ViewModelProperty = copy.ViewModelProperty.Clone
        copy.ControlLostFocusEventHandlerTarget = Nothing
        copy.ControlPropertyChangedEventHandlerTarget = Nothing
        Return copy
    End Function

    Public Overrides Function ToString() As String
        Dim retString = ""
        If ControlProperty IsNot Nothing Then
            retString &= "Control." & ControlProperty.PropertyName & " "
        End If

        Dim tmpBindingMode = BindingSetting.BindingMode And Not MvvmBindingModes.ValidatesOnNotifyDataErrors

        Select Case tmpBindingMode

            Case MvvmBindingModes.TwoWay
                retString &= "<-- " & BindingSetting.BindingMode.ToString & "--> "

            Case MvvmBindingModes.OneWay
                retString &= "--> " & BindingSetting.BindingMode.ToString & "--> "

            Case MvvmBindingModes.OneWayToSource
                retString &= "<-- " & BindingSetting.BindingMode.ToString & "<-- "

            Case MvvmBindingModes.OneTime
                retString &= "--> " & BindingSetting.BindingMode.ToString & "!-- "

            Case Else
                retString &= "--- " & BindingSetting.BindingMode.ToString & "---"

        End Select

        If ViewModelProperty IsNot Nothing Then
            retString &= "ViewModel." & ViewModelProperty.PropertyName
        End If

        If BindingSetting.BindingMode.HasFlag(MvvmBindingModes.ValidatesOnNotifyDataErrors) Then
            retString &= " [VAL]"
        End If

        Return retString

    End Function

End Class
