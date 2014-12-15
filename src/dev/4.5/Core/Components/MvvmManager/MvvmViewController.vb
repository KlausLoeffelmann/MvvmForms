Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MvvmViewController
    Inherits Panel

    Private myView As ContainerControl

    Public Property View As ContainerControl
        Get
            Return myView
        End Get
        Set(value As ContainerControl)
            If Object.Equals(value, myView) Then Return

            myView = value
            If myView.GetType.IsAssignableFrom(GetType(Form)) Then
                DirectCast(myView, Form).ShowDialog()
            End If
        End Set
    End Property



End Class
