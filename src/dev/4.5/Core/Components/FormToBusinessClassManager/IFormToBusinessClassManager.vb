Imports System.Data.Objects

''' <summary>
''' Kennzeichnet ein Formular, dass mit einer Instanz einer <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet wird.
''' </summary>
''' <remarks>Diese Schnittstelle muss in ein Formular eingebunden werden, damit Extender-Methoden des 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManagerExtender">FormToBusinessClassManagerExtender-Modules</see> verwendet werden können.</remarks>
Public Interface IFormToBusinessClassManagerHost
    ''' <summary>
    ''' Ermittelt die <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see>, 
    ''' mit der dieses Formular seine Business-Klasse verwaltet.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property FormToBusinessClassManager As FormToBusinessClassManager

    ''' <summary>
    ''' Bestimmt oder ermittelt den Objekt-Kontext, der benötigt wird, um Navigation-Properties aus DataFieldName-Eigenschaften zu setzen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TargetObjectContext As ObjectContext

End Interface

