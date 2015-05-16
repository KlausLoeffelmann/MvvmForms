'*****************************************************************************************
'                                    EventBindingClasses.vb
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

<Serializable, DesignerSerializer(GetType(PropertyBindingsCodeDomSerializer), GetType(CodeDomSerializer))>
Public Class EventBindings
    Inherits List(Of EventBindingItem)

    Sub New()
        MyBase.New()
    End Sub

    Sub New(enumList As IEnumerable(Of EventBindingItem))
        For Each enumItem In enumList
            Me.Add(enumItem)
        Next
    End Sub

    Public Function ToObservableItemList() As ObservableBindingList(Of EventBindingItem)
        Return New ObservableBindingList(Of EventBindingItem)(Me)
    End Function

    Public Overloads Sub Add(controlEvent As BindingEvent, viewModelCommand As BindingCommand, controlCanExecuteProperty As BindingProperty)

        Dim propBindItem As New EventBindingItem() With
                    {.ControlEvent = controlEvent,
                     .ViewModelCommand = viewModelCommand,
                     .ControlCanExecuteProperty = controlCanExecuteProperty}
        Me.Add(propBindItem)
    End Sub


    Public Function Clone() As EventBindings
        Dim retList As New EventBindings
        Me.ForEach(Sub(item)
                       retList.Add(item.Clone)
                   End Sub)
        Return retList
    End Function
End Class

<Serializable>
Public Class EventBindingItem
    Public Property ControlEvent As BindingEvent
    Public Property ControlCanExecuteProperty As BindingProperty
    Public Property ViewModelCommand As BindingCommand

    Public Function Clone() As EventBindingItem
        Dim copy = DirectCast(MyBase.MemberwiseClone, EventBindingItem)
        'Diese hier müssen extra gecloned werden, da es Referenztypen sind.
        copy.ControlEvent = copy.ControlEvent.Clone
        copy.ViewModelCommand = copy.ViewModelCommand.Clone
        Return copy
    End Function

End Class

<Serializable>
Public Class BindingEvent
    Inherits AttributeControlledComparableBase

    Sub New()
        MyBase.New()
    End Sub

    Sub New(eventName As String)
        Me.EventName = eventName
    End Sub

    <DisplayIndicator(1), EqualityIndicator>
    Property EventName As String

    Public Function Clone() As BindingEvent
        Return DirectCast(MyBase.MemberwiseClone, BindingEvent)
    End Function

End Class


