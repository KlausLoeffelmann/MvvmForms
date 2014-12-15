''' <summary>
''' Definiert Methoden für Steuerelemente, deren Inhalt von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> lediglich zur Anzeige aktualisiert wird.
''' </summary>
''' <remarks>FUNTKIONSBESCHREIBUNG ÜBERPRÜFEN/ERGÄNZEN!</remarks>
Public Interface IUpdatableInfoControl
    Inherits IAssignableFormToBusinessClassManager
    Sub UpdateDisplayData(ByVal datasource As Object)
End Interface