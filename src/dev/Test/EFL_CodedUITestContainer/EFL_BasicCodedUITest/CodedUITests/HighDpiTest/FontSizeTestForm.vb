Imports ActiveDevelop.MvvmBaseLib.Threading

Public Class FontSizeTestForm

    Private myTimer As Timer
    Private myCounter As Integer = 0

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        myTimer = New Timer(
            Sub()
                Debug.Print("Timer Invoked")
                Label1.Invoke(Sub()
                                  Label1.Text = myCounter.ToString
                                  myCounter += 1
                              End Sub)
            End Sub, Nothing, 2000, 100)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        myTimer.Cancel()
    End Sub
End Class