Public Interface IIsDirtyChangedAware

    ''' <summary>
    ''' Ereignis, das ausgelöst wird, wenn sich der Wert des Steuerelementes geändert hat, und sein Inhalt deswegen gespeichert werden sollte.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs)

    ''' <summary>
    ''' Zeigt an, ob der Wert des Steuerelementes seit dem letzten Speichern geändert wurde, und sein Inhalt deswegen wieder zum Speichern ansteht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property IsDirty() As Boolean

    ''' <summary>
    ''' Löscht den Zustandsspeicher, der angibt, ob ein Steuerelement seit dem letzten Speichern geändert wurde.
    ''' </summary>
    ''' <remarks></remarks>
    Sub ResetIsDirty()

End Interface
