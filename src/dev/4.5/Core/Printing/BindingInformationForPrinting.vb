Imports ActiveDevelop.EntitiesFormsLib


''' <summary>
''' Klasse, welches die Bindungsinformationen und Controlinformationen eines Controls speichert
''' </summary>
''' <remarks></remarks>
Public Class BindingInformationForPrinting

    ''' <summary>
    ''' Benötigt das MvvmBindingItem sowie das PropertyBindingItem um an alle Informationen zu gelangen
    ''' </summary>
    ''' <param name="bindingItem"></param>
    ''' <param name="propBinding"></param>
    ''' <remarks></remarks>
    Sub New(bindingItem As ExtenderProviderPropertyStoreItem(Of MvvmBindingItem), propBinding As PropertyBindingItem)
        ControlName = bindingItem.Control.Name
        ControlType = bindingItem.Control.GetType.Name


        If propBinding IsNot Nothing Then
            BindingMode = propBinding.BindingSetting.BindingMode.ToString()
            UpdateSourceTrigger = propBinding.BindingSetting.UpdateSourceTrigger.ToString()
            ViewModelPropertyName = propBinding.ViewModelProperty.PropertyName
            ViewModelPropertyType = propBinding.ViewModelProperty.PropertyType.Name
            ControlPropertyName = propBinding.ControlProperty.PropertyName
            If propBinding.ControlProperty.PropertyType.BaseType = Nothing Then
                ControlPropertyType = propBinding.ControlProperty.PropertyType.Name
            Else
                ControlPropertyType = propBinding.ControlProperty.PropertyType.BaseType.Name
            End If

        End If

    End Sub

    Public Property ControlName As String = Nothing
    Public Property ControlType As String = Nothing
    Public Property ControlPropertyType As String = Nothing
    Public Property ControlPropertyName As String = Nothing
    Public Property ViewModelPropertyName As String = Nothing
    Public Property ViewModelPropertyType As String = Nothing
    Public Property BindingMode As String = Nothing
    Public Property UpdateSourceTrigger As String = Nothing

End Class