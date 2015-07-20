Imports System.Xml.Serialization

<AttributeUsage(AttributeTargets.Property)>
Public Class ModelPropertyIgnoreAttribute
    Inherits Attribute

End Class

<AttributeUsage(AttributeTargets.Class)>
Public Class ModelPropertiesCompleteAttribute
    Inherits Attribute

End Class

<AttributeUsage(AttributeTargets.Property)>
Public Class ModelPropertyNameAttribute
    Inherits Attribute

    Sub New(propertyName As String)
        Me.PropertyName = propertyName
    End Sub

    Sub New(propertyName As String, mustExist As Boolean)
        Me.PropertyName = propertyName
        Me.MustExist = mustExist
    End Sub

    Public Property PropertyName As String
    Public Property MustExist As Boolean

End Class
