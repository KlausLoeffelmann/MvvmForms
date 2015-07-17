Imports System.Drawing

Partial Public Class ColoredTextPanel
    ''' <summary>
    ''' Ein Viewpanel stellt den farbigen Text dar 
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ColoredTextElementManager
        Inherits Recyclables(Of ColorTextElement)

        Public Sub New(ByVal count As Integer)
            MyBase.New(count, Nothing)
        End Sub
    End Class

    ''' <summary>
    ''' Ein ColorTextElement stellt einen einfarbigen Text dar
    ''' Zudem kann für das ColorTextElement der Font festgelegt werden
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ColorTextElement
        Implements IRecyclable

        Private myPoint As Point
        Private myFont As Font
        Private myCol As Color
        Private myMessage As String
        Private myMeasure As Size = New Size(-1, -1)
        Private myMeasureSet As Boolean = False
        Private myCharPositions As New List(Of Integer)


        ''' <summary>
        ''' Konstuktor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pos">virtuelle Position</param>
        ''' <param name="fnt">zu verwendender Font </param>
        ''' <param name="col">zu verwendende Farbe</param>
        ''' <param name="msg">Text des Elements</param>
        ''' <remarks></remarks>
        Public Sub Init(ByVal pos As Point, ByVal fnt As Font, ByVal col As Color, ByVal msg As String)
            myPoint = pos
            myFont = fnt
            myCol = col
            myMessage = msg
        End Sub

        ''' <summary>
        ''' ruft die Position des Textes ab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Position() As Point
            Get
                Return myPoint
            End Get
            Set(ByVal value As Point)
                myPoint = value
            End Set
        End Property

        ''' <summary>
        ''' ruft den Font des Textes ab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Font() As Font
            Get
                Return myFont
            End Get
        End Property

        ''' <summary>
        ''' ruft den eigentlichen Text ab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Message() As String
            Get
                Return myMessage
            End Get
        End Property

        ''' <summary>
        ''' Ruft die Farbe des Textes ab
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Color() As Color
            Get
                Return myCol
            End Get
        End Property

        ''' <summary>
        ''' Ruft die Größe des Textes in Pixeln ab (Liefert ein SizeF Objekt)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Measure() As Size
            Get
                Return myMeasure
            End Get
            Friend Set(ByVal value As Size)
                myMeasure = value
                myMeasureSet = True
            End Set
        End Property

        ''' <summary>
        ''' Prüft ob schon ein Measure-Wert gesetzt wurde
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function IsMeasureSet() As Boolean
            If Not myMeasureSet Then
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Liefert eine List(of Integer), die die Endposition eines Buchstabens in Pixeln beinhalten
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CharPositions() As List(Of Integer)
            Get
                Return myCharPositions
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
            Parent.Recycle(Me)
        End Sub
    End Class


End Class
