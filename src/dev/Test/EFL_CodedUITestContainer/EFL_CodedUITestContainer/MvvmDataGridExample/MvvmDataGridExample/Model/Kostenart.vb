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

    Private newPropertyValue As Test
    Public Property TestProp() As Test
        Get
            Return newPropertyValue
        End Get
        Set(ByVal value As Test)
            newPropertyValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class Test
    Property Test As Integer
End Class
