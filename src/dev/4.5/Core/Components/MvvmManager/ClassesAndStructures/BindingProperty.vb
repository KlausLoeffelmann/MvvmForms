<Serializable>
Public Class BindingProperty
    Inherits AttributeControlledComparableBase

    Sub New()
        MyBase.New()
    End Sub

    Sub New(propertyName As String, PropertyType As Type)
        MyBase.New()
        Me.PropertyName = propertyName
        Me.PropertyType = PropertyType
    End Sub

    <EqualityIndicator>
    Property PropertyName As String

    <Xml.Serialization.XmlIgnore>
    Property PropertyType As Type

    Property PropertyTypeAssemblyQualifiedName As String
        Get
            If PropertyType Is Nothing Then
                Return Nothing
            End If
            Return PropertyType.AssemblyQualifiedName
        End Get
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                PropertyType = Type.GetType(value)
            Else
                PropertyType = Nothing
            End If
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return PropertyName & " (" & PropertyType.TypeStringWithGenericParameters & ")"
    End Function

    Public Function Clone() As BindingProperty
        Return DirectCast(MyBase.MemberwiseClone, BindingProperty)
    End Function
End Class
