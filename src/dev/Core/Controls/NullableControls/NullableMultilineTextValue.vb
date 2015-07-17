''' <summary>
''' Steuerelement zur Erfassungs von mehrzeiligen Texten (Zeichenketten), das überdies Null-Werte verarbeitet, 
''' eine vereinheitlichende Value-Eigenschaft bietet, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
Public Class NullableMultilineTextValue
    Inherits NullableTextValue

    Protected Overrides Function IsMultiLineControl() As Boolean
        Return True
    End Function
End Class