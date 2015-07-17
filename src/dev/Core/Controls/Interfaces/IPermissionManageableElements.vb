Imports System.Drawing
Imports System.Windows.Forms
Imports System.Reflection

''' <summary>
''' Implementiert eine GUID-Eigenschaft für eine Komponente, sodass diese eindeutig indentifiziert werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface IRecognizableComponent
    Property IdentificationGuid As Guid
End Interface

''' <summary>
''' Definiert einen Satz von Eigenschaften für eine Komponente, sodass sie eine grundlegenden Rechteverwaltung untergeordnet werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface IPermissionManageableComponent
    Inherits IRecognizableComponent

    Property IsManageable As Boolean
    Property PermissionReason As String
    ReadOnly Property ElementType As PermissionManageableUIElementType
End Interface

''' <summary>
''' Definiert eine Komponente als inhaltspräsentierende Komponente, sodass ihr Inhalt (Content) rechtemäßig geregelt werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface IPermissionManageableUIContentElement
    Inherits IPermissionManageableComponent

    Property ContentPresentPermission As ContentPresentPermissions
End Interface

''' <summary>
''' Definiert eine Komponente als befehlsauslösende Komponente.
''' </summary>
''' <remarks></remarks>
Public Interface IPermissionManageableUIControlElement
    Inherits IPermissionManageableComponent

    Property ExecuteCallback As Action(Of Object)
End Interface

''' <summary>
''' Konzeptionell - wird derzeitig noch nicht verwendet.
''' </summary>
''' <remarks></remarks>
<Obsolete("Diese Klasse ist konzeptionell und wird noch nicht verwendet.")>
Public Structure PermissionManageableUIElementParams

    Sub New(ByVal name As String)
        Me.UIGuid = Guid.NewGuid
    End Sub

    Property UIGuid As Guid
    Property Control As PermissionManageableControlInfo
    Property Parent As PermissionManageableControlInfo
    Property RootParent As PermissionManageableControlInfo
End Structure

''' <summary>
''' Konzeptionell - wird derzeitig noch nicht verwendet.
''' </summary>
''' <remarks></remarks>
<Obsolete("Diese Klasse ist konzeptionell und wird noch nicht verwendet.")>
Public Class PermissionManageableControlInfo

    Private myControl As Control

    Sub New(ByVal control As Control)
        myControl = control
    End Sub

    ReadOnly Property Control As Control
        Get
            Return myControl
        End Get
    End Property

    ReadOnly Property FullTypeName As String
        Get
            Return Control.GetType.FullName
        End Get
    End Property

    ReadOnly Property HostingAssemblyName As String
        Get
            Return Control.GetType.Assembly.FullName
        End Get
    End Property

    ReadOnly Property Text As String
        Get
            Return Control.Text
        End Get
    End Property

    ReadOnly Property Name As String
        Get
            Return Control.Name
        End Get
    End Property
End Class

''' <summary>
''' Konzeptionell - wird derzeitig noch nicht verwendet.
''' </summary>
''' <remarks></remarks>
<Obsolete("Diese Klasse ist konzeptionell und wird noch nicht verwendet.")>
Public Class HasPermissionManageableUIElementsAttribute
    Inherits Attribute

    Sub New()
        MyBase.new()
    End Sub

    'TODO: Rekursiv alle betroffenen Steuerelemente im Formular einkassieren und zurückliefern.
    Public Shared Function GetPermissionControls(ByVal container As Control) As List(Of IPermissionManageableComponent)
        Return Nothing
    End Function
End Class

Public Enum PermissionManageableUIElementType
    Component
    Control
    Content
    Container
End Enum

Public Enum ContentPresentPermissions
    Normal
    Hidden
    Locked
    Obfuscated
    TopSecret
End Enum
