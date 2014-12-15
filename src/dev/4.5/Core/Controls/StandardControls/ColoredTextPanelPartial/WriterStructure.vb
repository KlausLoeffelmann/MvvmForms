Imports System.Drawing

Partial Public Class ColoredTextPanel
    ''' <summary>
    ''' Hilfstruktur, um die WriteMethode mit einer Queue zu verwalten
    ''' </summary>
    ''' <remarks></remarks>
    Private Class WriterStructureManager
        Inherits Recyclables(Of WriterStructure)

        Public Sub New(ByVal count As Integer)
            MyBase.New(count, Nothing)
        End Sub

    End Class

    ''' <summary>
    ''' Hilfstruktur, um die WriteMethode mit einer Queue zu verwalten
    ''' </summary>
    ''' <remarks></remarks>
    Private Class WriterStructure
        Implements IRecyclable


        Private myMsg As String
        Public Property Msg() As String
            Get
                Return myMsg
            End Get
            Set(ByVal value As String)
                myMsg = value
            End Set
        End Property


        Private myFont As Font
        Public Property Font() As Font
            Get
                Return myFont
            End Get
            Set(ByVal value As Font)
                myFont = value
            End Set
        End Property


        Private myColor As Color
        Public Property Color() As Color
            Get
                Return myColor
            End Get
            Set(ByVal value As Color)
                myColor = value
            End Set
        End Property



        Public Sub AllocateRessource(ByVal parameter As IRecyclableParameters) Implements IRecyclable.AllocateRessource

        End Sub

        Public Sub InitializeRecyclable() Implements IRecyclable.InitializeRecyclable

        End Sub

        Private myParent As IRecycle
        Public Property Parent() As IRecycle Implements IRecyclable.Parent
            Get
                Return myParent
            End Get
            Set(ByVal value As IRecycle)
                myParent = value
            End Set
        End Property

        Public Sub Recycle() Implements IRecyclable.Recycle
            Parent.Recycle(Me)
        End Sub
    End Class



End Class
