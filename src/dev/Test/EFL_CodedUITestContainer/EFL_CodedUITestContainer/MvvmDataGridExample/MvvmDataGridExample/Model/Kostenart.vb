Imports ActiveDevelop.MvvmBaseLib

Public Class Kostenart
    Inherits BindableBase

    Private _name As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            MyBase.SetProperty(_name, value)
        End Set
    End Property
    Public Const NameProperty As String = "Name"


End Class
