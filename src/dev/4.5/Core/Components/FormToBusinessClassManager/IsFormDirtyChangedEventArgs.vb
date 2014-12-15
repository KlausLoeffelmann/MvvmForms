Imports System.Data.Objects

''' <summary>
''' Beinhaltet das Steuerelement, das dazu geführt hat, dass eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> 
''' das <see cref="EntitiesFormsLib.FormToBusinessClassManager.IsFormDirtyChanged">IsFormDirtyChanged-Ereignis</see> ausgelöst hat.
''' </summary>
''' <remarks>Das <see cref="EntitiesFormsLib.FormToBusinessClassManager.IsFormDirtyChanged">IsFormDirtyChanged-Ereignis</see> wird dann ausgelöst, wenn 
''' der Inhalt eines der Eingabefelder eines Formulars, das durch die 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet wird, vom Anwender verändert wurde, sodass 
''' der Entwickler entsprechende Maßnahmen ergreifen kann, um dem Anwender anzuzeigen, dass Änderungen am Formular noch gespeichert werden müssen.</remarks>
Public Class IsFormDirtyChangedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse und bestimmt das Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <param name="causingControl"></param>
    ''' <remarks></remarks>
    Sub New(causingControl As Object)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property CausingControl As Object

End Class

''' <summary>
''' Wird für das Ermitteln des Ziel-Objektkontextes verwendet, wenn eine 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> 
''' das <see cref="EntitiesFormsLib.FormToBusinessClassManager.GetTargetObjectContext">IsFormDirtyChanged-Ereignis</see> ausgelöst hat.
''' </summary>
Public Class GetTargetObjectContextEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        TargetObjectContext = Nothing
    End Sub

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse und definiert den Objekt-Context, der für das Schreiben in die Zielentität erfordelich ist.
    ''' </summary>
    ''' <param name="targetObjectContext"></param>
    ''' <remarks></remarks>
    Sub New(targetObjectContext As ObjectContext)
        Me.TargetObjectContext = targetObjectContext
    End Sub

    ''' <summary>
    ''' Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TargetObjectContext As ObjectContext

End Class
