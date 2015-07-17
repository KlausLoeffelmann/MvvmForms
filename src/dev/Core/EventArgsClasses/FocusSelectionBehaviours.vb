''' <summary>
''' Definiert die Verhaltensweisen der Einfügemarke, wenn ein von <see cref="NullableValueBase">NullableValueBase</see> 
''' abgeleitetes Steuerelement den Fokus erhält.
''' </summary>
''' <remarks>Die Verhaltensweise ist über dessen FocusSelectionBehaviour-Eigenschaft einstellbar.</remarks>
Public Enum FocusSelectionBehaviours
    ''' <summary>
    ''' Platziert den Cursor beim Fokussieren vor dem Text.
    ''' </summary>
    ''' <remarks></remarks>
    PlaceCarentUpFront

    ''' <summary>
    ''' Selektiert den gesamten Text, und platziert den Cursor hinter dem Text.
    ''' </summary>
    ''' <remarks>Dies ist die Standardeinstellung.</remarks>
    PreSelectInput

    ''' <summary>
    ''' Platziert den Cursor beim Fokussieren hinter dem Text ohne zu Selektieren.
    ''' </summary>
    ''' <remarks></remarks>
    PlaceCaretAtEnd
End Enum
