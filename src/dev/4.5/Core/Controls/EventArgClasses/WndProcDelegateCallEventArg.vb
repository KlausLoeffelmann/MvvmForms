Imports System.Windows.Forms

Public Class WndProcDelegateCallEventArg
    Inherits EventArgs

    Sub New(ByVal Message As Message)
        _Message = Message
    End Sub

    Property Message As Message

End Class
