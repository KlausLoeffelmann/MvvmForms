Imports System.ComponentModel

''' <summary>
''' Schreibt das Vorhandensein einer IsKeyField-Eigenschaft vor, die Steuerelemente als besondere Schlüsselfelder markiert, mit
''' denen Datensätze ausgewählt werden und nicht etwa angezeigte, zum Datensatz zugehörige Daten editiert werden.
''' </summary>
''' <remarks></remarks>
Public Interface IKeyFieldProvider

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Property IsKeyField As Boolean

End Interface
