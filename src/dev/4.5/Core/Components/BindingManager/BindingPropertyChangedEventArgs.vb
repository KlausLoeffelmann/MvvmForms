Imports System.Windows.Data

''' <summary>
''' Provides data for the BindingPropertyChanged Event.
''' </summary>
''' <remarks></remarks>
Public Class BindingPropertyChangedEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(originalSource As Object)
        Me.OriginalSource = originalSource
    End Sub

    ''' <summary>
    ''' Control, which caused this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalSource As Object

    ''' <summary>
    ''' BindingPath-Name of the property, which caused this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EventProperty As String

    ''' <summary>
    ''' The converter to use for data conversion, if a converter has been spacified, otherwise NULL (Nothing in VB).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Converter As IValueConverter

End Class
