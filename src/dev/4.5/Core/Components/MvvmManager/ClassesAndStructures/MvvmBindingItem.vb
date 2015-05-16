'*****************************************************************************************
'                                    MvvmBindingItem.vb
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

Imports System.IO
Imports System.Windows.Forms
Imports System.Xml.Serialization
Imports System.ComponentModel.Design.Serialization

<Serializable, DesignerSerializer(GetType(MvvmBindingItemsCodeDomSerializer),
                                  GetType(CodeDomSerializer))>
Public Class MvvmBindingItems
    Inherits ExtenderProviderPropertyStore(Of MvvmBindingItem)

    Public Overloads Sub Add(control As Control, storeItem As MvvmBindingItem)
        Me.Add(New ExtenderProviderPropertyStoreItem(Of MvvmBindingItem) With
               {.Control = control, .Data = storeItem})
    End Sub

    Public Overloads Sub AddPropertyBinding(control As Control, bindingMode As BindingSetting, controlProperty As BindingProperty,
                             converter As Type, converterParameter As String, viewModelProperty As BindingProperty)

        Dim propBindItem As New PropertyBindingItem() With
                    {.BindingSetting = bindingMode,
                     .ControlProperty = controlProperty,
                     .Converter = converter,
                     .ConverterParameter = converterParameter,
                     .ViewModelProperty = viewModelProperty}

        If Me.Contains(control) Then
            If Me(control).Data.PropertyBindings Is Nothing Then
                Me(control).Data.PropertyBindings = New PropertyBindings From {propBindItem}
            Else
                Me(control).Data.PropertyBindings.Add(propBindItem)
            End If
        Else
            Me.Add(New ExtenderProviderPropertyStoreItem(Of MvvmBindingItem) With
               {.Control = control, .Data = New MvvmBindingItem With
                   {.PropertyBindings = New PropertyBindings From {propBindItem}}})
        End If
    End Sub
End Class

''' <summary>
''' Definiert eine Datenstruktur, die einem Bindungs-Set für ein Steuerelement an ein ViewModel entspricht. 
''' Infrastrukturfunktion, die von der MvvmManager-Komponente verwendet wird.
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class MvvmBindingItem
    Property PropertyBindings As PropertyBindings
    Property EventBindings As ObservableBindingList(Of EventBindingItem)
    Property ConverterAssembly As String

    Public Sub Serialize(textWriter As TextWriter)
        Dim serializer As New XmlSerializer(GetType(MvvmBindingItem),
                                                {GetType(PropertyBindings),
                                                GetType(ObservableBindingList(Of EventBindingItem)),
                                                GetType(BindingEvent),
                                                GetType(BindingCommand),
                                                GetType(BindingProperty)})

        serializer.Serialize(textWriter, Me)
    End Sub

    Public Shared Function FromXmlStream(textReader As TextReader) As MvvmBindingItem
        Dim serializer As New XmlSerializer(GetType(MvvmBindingItem),
                                                {GetType(PropertyBindings),
                                                GetType(ObservableBindingList(Of EventBindingItem)),
                                                GetType(BindingEvent),
                                                GetType(BindingCommand),
                                                GetType(BindingProperty)})

        Return DirectCast(serializer.Deserialize(textReader), MvvmBindingItem)
    End Function

    Public Overrides Function ToString() As String
        Return If(PropertyBindings Is Nothing, "0",
            PropertyBindings.Count.ToString) & " PropertyBindings"
    End Function

End Class
