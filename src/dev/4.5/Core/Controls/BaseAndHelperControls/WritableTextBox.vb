Imports System.Windows.Forms

Public Class WritableTextBox
    Inherits System.Windows.Forms.TextBox

    Public Shared Event GlobalWriteOccured(ByVal sender As Object, ByVal e As GlobalWriteEventArgs)

    Sub New()
        Me.Multiline = True
        Me.ScrollBars = Windows.Forms.ScrollBars.Vertical
        AddHandler WritableTextBox.GlobalWriteOccured, Sub(sender, e)
                                                           If ConsumeGlobalWrite Then
                                                               Write(e.Text)
                                                           End If
                                                       End Sub
    End Sub

    Public Sub Write(text As String)
        Dim writer = Sub()
                         Me.AppendText(text)
                         Me.SelectionStart = Me.Text.Length
                         Me.ScrollToCaret()
                     End Sub

        If Me.InvokeRequired Then
            Me.Invoke(writer)
        Else
            writer()
        End If
    End Sub

    Public Sub WriteLine(text As String)
        Write(text & vbNewLine)
    End Sub

    Public Shared Sub GlobalWrite(ByVal Text As String)
        RaiseEvent GlobalWriteOccured(GetType(WritableTextBox), New GlobalWriteEventArgs(Text))
    End Sub

    Property ConsumeGlobalWrite As Boolean

    Public Shared Sub GlobalWriteLine(ByVal Text As String)
        GlobalWrite(Text & vbNewLine)
    End Sub

End Class

Public Class GlobalWriteEventArgs
    Inherits EventArgs

    Sub New(ByVal text As String)
        Me.Text = text
    End Sub

    Public Property Text As String

End Class
