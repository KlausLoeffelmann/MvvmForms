Imports System.Windows.Forms
Imports System.Windows

Public Class DeferralTimerCarrier

    Friend Shared DeferralTimer As New Timer With {.Interval = 50, .Enabled = True}

End Class

Public Class TextBoxDeferrer
    Implements IDisposable

    Private myTextBox As TextBox
    Private myNoDeferOnNextTextChange As Boolean


    Public Event DeferralTextChanged(sender As Object, e As EventArgs)

    Private myStopWatch As Stopwatch = New Stopwatch

    Sub New(textBox As TextBox)
        MyBase.New()
        If textBox Is Nothing Then
            Throw New ArgumentNullException("textBox", "TextBox argument must not be null.")
        End If

        myTextBox = textBox

        WeakEventManager(Of Timer, EventArgs).AddHandler(DeferralTimerCarrier.DeferralTimer,
                                                          "Tick",
                                                          AddressOf DeferralTimerEventHandler)

        AddHandler myTextBox.TextChanged, (Sub(sender As Object, e As EventArgs)
                                               If IgnoreTextChange Then Return
                                               If IgnoreNextTextChange Then
                                                   IgnoreNextTextChange = False

                                                   'Es reicht nicht, das TextChange zu verhindern -
                                                   'ein bereits getriggertes DeferralTextChange darf nun auch nicht mehr kommen!
                                                   myStopWatch.Stop()
                                                   myStopWatch.Reset()
                                                   Return
                                               End If

                                               If NoDeferOnNextTextChange Then
                                                   NoDeferOnNextTextChange = False
                                                   OnDeferralTextChanged(sender, e)
                                               End If

                                               myStopWatch.Restart()
                                           End Sub)


    End Sub

    Public Sub DeferralTimerEventHandler(sender As Object, e As EventArgs)
        If myStopWatch.ElapsedMilliseconds > DeferredTextChangeDelay Then
            OnDeferralTextChanged(myTextBox, EventArgs.Empty)
        End If
    End Sub

    Protected Overridable Sub OnDeferralTextChanged(sender As Object, e As EventArgs)
        myStopWatch.Stop()
        myStopWatch.Reset()
        RaiseEvent DeferralTextChanged(sender, e)
    End Sub

    Public Property DeferredTextChangeDelay As Integer = 300
    Public Property IgnoreTextChange As Boolean
    Public Property IgnoreNextTextChange As Boolean
    Public Property NoDeferOnNextTextChange As Boolean
        Get
            Return myNoDeferOnNextTextChange
        End Get
        Set(value As Boolean)
            myNoDeferOnNextTextChange = value
        End Set
    End Property

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                WeakEventManager(Of Timer, EventArgs).RemoveHandler(DeferralTimerCarrier.DeferralTimer,
                                                                     "Tick",
                                                                     AddressOf DeferralTimerEventHandler)
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
