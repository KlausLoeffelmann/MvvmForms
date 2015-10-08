Imports System.Collections.ObjectModel
Imports ActiveDevelop.EntitiesFormsLib

''' <summary>
''' Wrapper class around Binding Property for presentation in MVVM assignment dialog
''' </summary>
Public Class PropertyBindingNodeDefinition

    Public Sub New(binding As BindingProperty)
        Me.Binding = binding
    End Sub

    Public Sub New()
    End Sub

    Private _subProperties As Lazy(Of ObservableCollection(Of PropertyBindingNodeDefinition)) _
        = New Lazy(Of ObservableCollection(Of
        PropertyBindingNodeDefinition))(Function()
                                            Dim propertiesList = New List(Of PropertyCheckBoxItemController)
                                            ReflectionHelper.CreateFlatSubPropAsList(Binding.PropertyType, "", propertiesList, False)

                                            Return New ObservableCollection(Of PropertyBindingNodeDefinition)(From pItem In propertiesList
                                                                                                              Order By pItem.PropertyFullname
                                                                                                              Select New PropertyBindingNodeDefinition With
                                                                                                   {.Binding = New BindingProperty() With
                                                                                                       {.PropertyName = Binding.PropertyName & "." & pItem.PropertyFullname,
                                                                                                        .PropertyType = pItem.PropertyType},
                                                                                                                  .PropertyName = pItem.PropertyFullname})

                                        End Function)
    ''' <summary>
    ''' SubProperties of a custom type
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property SubProperties As ObservableCollection(Of PropertyBindingNodeDefinition)
        Get
            Return _subProperties.Value
        End Get
    End Property

    ''' <summary>
    ''' Description for UI-Presentation
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Description As String
        Get
            Return String.Format("{0} : ({1})", PropertyName, ToGenericTypeString(Binding.PropertyType))
        End Get
    End Property

    ''' <summary>
    ''' Generates the generic type list as String with full type params
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    Private Function ToGenericTypeString(t As Type) As String
        If Not t.IsGenericType Then Return t.Name

        Dim genericTypeName = t.GetGenericTypeDefinition().Name

        genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf("`"c))
        Dim genericArgs = String.Join(",", t.GetGenericArguments().Select(Function(ta) ToGenericTypeString(ta)).ToArray())

        Return $"{genericTypeName}<{genericArgs}>"
    End Function

    ''' <summary>
    ''' Inner BindingProperty-Instance
    ''' </summary>
    ''' <returns></returns>
    Property Binding As BindingProperty

    ''' <summary>
    ''' Small Propertyname (without Path)
    ''' </summary>
    ''' <returns></returns>
    Property PropertyName As String

    ''' <summary>
    ''' Overrides Equals for delegation to BindingProperty-Equals
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If

        If TypeOf obj Is PropertyBindingNodeDefinition Then
            Return Binding.Equals(DirectCast(obj, PropertyBindingNodeDefinition).Binding)
        Else
            Return MyBase.Equals(obj)
        End If
    End Function

    ''' <summary>
    ''' Overrides GetHashCode for delegation to BindingProperty-Equals
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetHashCode() As Integer
        Return (Binding.PropertyName + Binding.PropertyType.FullName).GetHashCode()
    End Function
End Class