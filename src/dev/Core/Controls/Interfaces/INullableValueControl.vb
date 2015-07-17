''' <summary>
''' Definiert die Eigenschaft, die ein NullableValueControl-Steuerelement aufweisen muss.
''' </summary>
''' <remarks></remarks>
Public Interface INullableValueControl
    Inherits IAssignableFormToBusinessClassManager, IKeyFieldProvider

    ''' <summary>
    ''' Bestimmt oder ermittelt den Wert, den ein Steuerelement aufweist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Value() As Object

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Meldungstext, der beispielsweise zur Darstellung einer Fehlermeldung verwendet wird, 
    ''' wenn das Steuerelement einen Null-Wert aufweist, aber zu einem bestimmten Zeitpunkt keinen solchen haben darf oder soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property NullValueMessage As String

    ''' <summary>
    ''' Ereignis, das ausgelöst wird, wenn die Validierung des Steuerelements fehlgeschlagen ist, und es die weitere Vorgehensweise erfragt.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs)

    ''' <summary>
    ''' Bestimmt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ExceptionBalloonDuration As Integer

End Interface
