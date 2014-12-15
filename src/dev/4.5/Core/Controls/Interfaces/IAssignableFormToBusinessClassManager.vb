Public Interface IGroupable
    ''' <summary>
    ''' Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property GroupName As String

End Interface

''' <summary>
''' Definiert eine Komponente, die einem <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see> zugewiesen werden kann.
''' </summary>
''' <remarks>Steuerelemente, die einem <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see> zugewiesen werden können, 
''' müssen diese Schnittstelle implementieren.</remarks>
Public Interface IAssignableFormToBusinessClassManager
    Inherits IGroupable
    ''' <summary>
    ''' Bestimmt oder ermittelt die <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz, dem diese Komponente zugeordnet werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property AssignedManagerControl As FormToBusinessClassManager

    ''' <summary>
    ''' Bestimmt oder ermittelt eine Priorität, die bestimmt, in welcher Reihenfolge eine 
    ''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz Komponenten verarbeitet. (Höherer Wert --> frühere Verarbeitung.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ProcessingPriority As Integer

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz 
    ''' diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property SelectedForProcessing As Boolean

End Interface
