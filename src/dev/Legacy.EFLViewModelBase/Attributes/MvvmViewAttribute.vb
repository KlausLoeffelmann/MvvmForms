Imports System.Reflection
Imports System.ServiceModel

''' <summary>
''' Kennzeichnet eine Klasse als MVVM-View und damit, 
''' welches Form/UserControl für ein bestimmtes ViewModel implizit verwendet werden soll.
''' </summary>
<AttributeUsage(AttributeTargets.Class)>
Public NotInheritable Class MvvmViewAttribute
    Inherits Attribute

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse.
    ''' </summary>
    ''' <param name="viewTypeName">Typ der View. In Abhängigkeit von der verwendeten UI-Technologie kann das beispielsweise 
    ''' der vollqualifizierte Name des Typs einer Fenster-Klasse sein.</param>
    ''' <remarks></remarks>
    Sub New(viewTypeName As String)
        Me.ViewTypeName = viewTypeName
        Me.Assemblyname = Me.GetType().GetTypeInfo.AssemblyQualifiedName
        Me.ContextGuid = Guid.NewGuid
    End Sub

    Sub New(viewname As String, assemblyName As String)
        Me.ViewTypeName = viewname
        Me.Assemblyname = assemblyName
        Me.ContextGuid = Guid.NewGuid
    End Sub

    Sub New(viewName As String, assemblyname As String, contextGuid As String)
        Me.ViewTypeName = viewName
        Me.Assemblyname = assemblyname
        Me.ContextGuid = New Guid(contextGuid)
    End Sub

    Public Property ContextGuid As Guid?
    Public Property Assemblyname As String
    Public Property ViewTypeName As String
End Class
