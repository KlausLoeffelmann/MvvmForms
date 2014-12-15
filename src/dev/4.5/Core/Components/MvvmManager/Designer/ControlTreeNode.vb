Imports System.Windows.Forms
Imports System.ComponentModel

Public Class ControlTreeNode
    Inherits TreeNode

   
    Public Sub New()
    End Sub

    Public Sub New(comp As Component)
        SetComponent(comp)
    End Sub

    Private myComponent As WeakReference
    Private myCompIdentifier As String

    Public ReadOnly Property Component As Component
        Get
            If myComponent.IsAlive Then
                Return CType(myComponent.Target, Component)
            Else
                Throw New ObjectDisposedException("Das Object '" & myCompIdentifier & "' ist nicht mehr vorhanden.")
            End If
        End Get
    End Property

    Public Sub SetComponent(comp As Component)
        If comp Is Nothing Then Throw New ArgumentNullException("comp")
        myComponent = New WeakReference(comp)
        Dim c = TryCast(comp, Control)
        Dim typname = comp.GetType.FullName
        Dim tttxt As String = String.Format("... ({0})", comp.GetType.Name)
        If c IsNot Nothing Then
            myCompIdentifier = String.Format("'{0}' ({1})", c.Name, typname)
            tttxt = String.Format("{0} --- ({1})", c.Name, comp.GetType.Name)
        Else
            myCompIdentifier = String.Format("... ({0})", typname)
        End If

        Dim textTemp = If(String.IsNullOrWhiteSpace(c.Name), "...", c.Name)

        'Versuchen, den Inhalt der Texteigenschaft in Klammern dahinter zu setzen.
        Dim propInfo = c.GetType.GetProperty("Text")
        Dim propInfoText As String = ""
        If propInfo IsNot Nothing Then
            propInfoText = " (" & propInfo.GetValue(c, Nothing).ToString & ")"
        End If

        MyBase.Text = textTemp & propInfoText
        MyBase.ToolTipText = tttxt & propInfoText
    End Sub
End Class
