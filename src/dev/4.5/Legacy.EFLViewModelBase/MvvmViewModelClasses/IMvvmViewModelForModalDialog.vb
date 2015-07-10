''' <summary>
''' Erweitert das <see cref="IMvvmViewModel">IMvvmViewModel</see>-Interface um eine Funktionalität, 
''' sodass die korrelierende View als modaler Dialog vom ViewModel-Controller aufgerufen werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface IMvvmViewModelForModalDialog
    Inherits IMvvmViewModel

    ''' <summary>
    ''' Bestimmt oder ermittelt den (bindbaren) Wert, der von der korrelierenden View per 
    ''' Bindung gesetzt wird und nach Beenden des Dialogs den Rückgabewert widerspiegelt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property DialogReturnValue As MvvmMessageBoxReturnValue

End Interface
