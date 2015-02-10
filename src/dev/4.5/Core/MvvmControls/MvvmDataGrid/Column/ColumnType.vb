''' <summary>
''' Der elementare Typ der Spalte
''' </summary>
''' <remarks></remarks>
Public Enum ColumnType
    ''' <summary>
    ''' Einfache TextBox welche auch bearbeitet werden kann und auch Zahlen anzeigen kann (ohne Converter)
    ''' </summary>
    ''' <remarks></remarks>
    TextAndNumbers
    ' ''' <summary>
    ' ''' Spezielle TextBox, welche fuer die Eingabe und Darstellung von Zahlen verwendet werden kann
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Num
    ''' <summary>
    ''' Einfacher Spaltentyp fuer Booleanwerte
    ''' </summary>
    ''' <remarks></remarks>
    CheckBox
    ''' <summary>
    ''' ComboBox-Spaltentyp, fuer die Auswahl von mehreren Werten
    ''' </summary>
    ''' <remarks></remarks>
    ComboBox
    'Idee fuer spezielle vom Anwendungsentwickler erstelle ColumnTemplates...
    'Custom
    ''' <summary>
    ''' Link zu einer Url
    ''' </summary>
    ''' <remarks></remarks>
    Hyperlink

    ''' <summary>
    ''' Spaltentyp fuer die Verwendung eines Bildes
    ''' </summary>
    ''' <remarks></remarks>
    Image
End Enum
