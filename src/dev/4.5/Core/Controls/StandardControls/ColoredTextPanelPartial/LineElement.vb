Partial Public Class ColoredTextPanel
    Private Class LineElementManager
        Inherits Recyclables(Of LineElement)

        Public Sub New(ByVal maxLines As Integer)
            MyBase.New(maxLines, Nothing)
        End Sub
    End Class

    ''' <summary>
    ''' Ein LineElement stellt eine Zeile in der ColorTextBox dar
    ''' Sie kann aus mehreren ColorTextElement(en) bestehen
    ''' </summary>
    ''' <remarks></remarks>
    Private Class LineElement
        Implements IRecyclable
        Private myY As Integer
        Private myElems As List(Of ColorTextElement)

        ''' <summary>
        ''' Konstruktor 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="y">Y Position der Zeile</param>
        ''' <param name="elems">Liste der ColorTextElemente</param>
        ''' <remarks></remarks>
        Public Sub Init(ByVal y As Integer, ByVal elems As List(Of ColorTextElement))
            myY = y
            myElems = elems
        End Sub

        ''' <summary>
        ''' Ruft die Y Position der Zeile ab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Y() As Integer
            Get
                Return myY
            End Get
            Set(ByVal value As Integer)
                myY = value
            End Set
        End Property

        ''' <summary>
        ''' Liefert eine Liste von ColorTextElement(en)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Elements() As List(Of ColorTextElement)
            Get
                Return myElems
            End Get
        End Property

        Public Shared id As Integer = 0
        Private Shared locker As New Object

        Private myName As String = ""
        Private myParent As IRecycle

        Public Sub AllocateRessource(ByVal parameter As IRecyclableParameters) Implements IRecyclable.AllocateRessource
            SyncLock (locker)
                myName = "String " + id.ToString
                id += 1
            End SyncLock
        End Sub

        Public Sub InitializeRecyclable() Implements IRecyclable.InitializeRecyclable

        End Sub

        Public Property Parent() As IRecycle Implements IRecyclable.Parent
            Get
                Return myParent
            End Get
            Set(ByVal value As IRecycle)
                myParent = value
            End Set
        End Property

        Public Sub Recycle() Implements IRecyclable.Recycle
            For Each coloredElement In Me.Elements
                coloredElement.Recycle()
            Next
            Parent.Recycle(Me)
        End Sub
    End Class


End Class
