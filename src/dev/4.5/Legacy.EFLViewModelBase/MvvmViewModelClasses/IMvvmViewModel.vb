''' <summary>
''' Schreibt Funktionalitäten vor, mit deren Hilfe Views und modale Dialoge von ViewModels UI-technologieunabhängig 
''' verwendet bzw. zur Darstellung angefordert werden können.
''' </summary>
''' <remarks></remarks>
Public Interface IMvvmViewModel

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit ein entsprechender, einem ViewModel zugeordneter Dialog 
    ''' modal dargestellt werden kann.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event RequestModalView(sender As Object, e As RequestViewEventArgs)

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit ein entsprechender, einem ViewModel zugeordneter Dialog 
    ''' dargestellt werden kann (non-modal).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event RequestView(sender As Object, e As RequestViewEventArgs)

    ''' <summary>
    ''' Ereignis, das ein ViewModel auslöst, damit es einen entsprechenden Message-Dialog darstellen kann.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event RequestMessageDialog(sender As Object, e As RequestMessageDialogEventArgs)

    <Obsolete("Wird diskutiert, ob diese Funktionalität in Zukunft überhaupt benötigt wird.")>
    Sub InitializeViewModel()

End Interface
