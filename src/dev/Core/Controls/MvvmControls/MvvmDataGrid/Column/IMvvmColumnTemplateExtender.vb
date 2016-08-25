Imports System.Windows.Controls
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Runtime.InteropServices
Imports System.Windows

''' <summary>
''' Dieses Interface kann vom Anwendungsentwickler verwdenet werden, um die Columneigenscahften via ein TemplateExtender zu erweitert. Dazu implementiert er an einer
''' Klasse dieses Interface und ergaenzt dann die fehlenden Properties. ANschliessend muss er fuer die Initialisierung und der Bindungsueberfuehrung sorgren.
''' </summary>
''' <remarks></remarks>
<TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
Public Interface IMvvmColumnTemplateExtender
    ''' <summary>
    ''' Initialisiert eine Text-Column
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Sub InitilizeColumn(column As DataGridTextColumn)
    ''' <summary>
    ''' Initialisiert eine CheckBox-Column
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Sub InitilizeColumn(column As DataGridCheckBoxColumn)

    ''' <summary>
    ''' Initialisiert eine CBO-Column
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Sub InitilizeColumn(column As DataGridComboBoxColumn)

    ''' <summary>
    ''' Initialisiert eine Template-Column
    ''' </summary>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Sub InitilizeColumn(column As DataGridTemplateColumn)
End Interface