''' <summary>
''' Ermöglicht einer an den FormsToBusiness-Manager gebundenen Business-Class-Objektes das selbständige Validieren, wenn das Business-Objekt dieses Interface einbindet.
''' </summary>
''' <remarks></remarks>
Public Interface IValidatable

    Function ValidateData() As ValidationInfoBase

End Interface
