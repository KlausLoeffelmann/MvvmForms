''' <summary>
''' Stellt einen Boolean-Datentyp zur Verfügung, der erst dann wieder FALSE zurückliefert, 
''' wenn ihm FALSE sooft zugewiesen wurde, wie ihm zuvor TRUE zugewiesen wurde (und umgekehrt).
''' </summary>
''' <remarks></remarks>
Public Class HistoricalBoolean

    Private truecount As Integer

    Property Value As Boolean
        Get
            Return truecount > 0
        End Get
        Set(ByVal value As Boolean)
            truecount += If(value, 1, -1)
            truecount = If(truecount < 0, 0, truecount)
        End Set
    End Property
End Class
