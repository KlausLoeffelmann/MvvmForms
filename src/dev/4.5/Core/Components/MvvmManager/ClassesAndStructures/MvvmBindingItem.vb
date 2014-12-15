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
