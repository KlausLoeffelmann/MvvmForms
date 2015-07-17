Imports System.Windows.Forms
Imports System.Text

''' <summary>
''' Stellt Eigenschaften und Methoden für ein NullableValueControl bereit, das über einen Eingabeeditor (beispielsweise TextBox-basierend) verfügt und 
''' darüber eine entsprechende Infrastruktur für die Formatierung und Validierung der Eingabe zur Verfügung stellt.
''' </summary>
''' <remarks></remarks>
Public Interface INullableValueEditor
    Inherits INullableValueControl

    Property NullValueString As String

    ''' <summary>
    ''' Der ursprüngliche Wert der Eingabe.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property OriginalValue() As Object

    ''' <summary>
    ''' Eine Formatierungs-Engine, die die aufbereitung des Wertes für eine formatierte Darstellung übernimmt, wenn das Eingabefeld den Fokus verlässt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property FormatterEngine() As INullableValueFormatterEngine

    ''' <summary>
    ''' Bestimmt oder ermittelt eine Formatzeichenfolge, mit der der Wert für die formatierte Darstellung über seine Formateirungs-Engine aufbereitet werden kann.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property FormatString As String

    ''' <summary>
    ''' Ermittelt, ob der Editor mehrzeilige Eingaben verarbeiten soll.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function IsMultilineControl() As Boolean

    ''' <summary>
    ''' Validiert die Eingabe und liefert im Bedarfsfall eine entsprechende Ausnahme oder Nothing zurück.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValidateContent() As ContainsUIMessageException

    ''' <summary>
    ''' Bestimmt, dass die Validierung auf Formularebene fehlgeschlagen ist, und definiert eine entsprechende Fehlermeldung für die Anzeige.
    ''' </summary>
    ''' <param name="ErrorMessage"></param>
    ''' <remarks></remarks>
    Sub SetFailedValidation(ByVal ErrorMessage As String)

    ''' <summary>
    ''' Setzt die Validierung auf Formularebene zurück.
    ''' </summary>
    ''' <remarks></remarks>
    Sub ClearFailedValidation()

End Interface

