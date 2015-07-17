Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Text

''' <summary>
''' Definiert Ereignisse, Methoden und Eigenschaften, damit ein NullableValueControl automatisch 
''' über einen FormToBusinessClassManager mit Werten befüllt werden kann.
''' </summary>
''' <remarks></remarks>
Public Interface INullableValueDataBinding
    Inherits INullableValueControl, IIsDirtyChangedAware

    ''' <summary>
    ''' Ereignis, das ausgelöst wird, wenn sich der Wert des Steuerelements geändert hat.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs)

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Maske, die das Steuerelement enthält, gerade die Steuerelemente mit Daten befüllt, oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Das Setzen dieser Eigenschaft sollte nur über diese Schnittstelle (in der Regel von der Maskensteuerung) vorgenommen werden.</remarks>
    Property IsLoading() As HistoricalBoolean

    ''' <summary>
    ''' Bestimmt den Namen der Eigenschaft oder den Pfad der Eigenschaft, die an die Datenquelle mit ihrer Value-Eigenschaft gebunden wird.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bei der Angabe der Eigenschaften sollten Steuerelemente auch den Pfad zu einer Eigenschaft auswerten können, der mit Punkten angegeben werden kann. 
    ''' Also könnte beispielsweise mit dem DatafieldName "Contact.FirstName" auf eine Eigenschaft namens <i>Contact</i> zugegriffen werden, die eine Instanz vom Typ 
    ''' <i>Contact</i> zurückliefert, die ihrerseits über <i>eine Firstname</i>-Eigenschaft verfügt, die erst die eigentlich zu bindende Eigenschaft darstellt.</remarks>
    Property DatafieldName() As String

    ''' <summary>
    ''' Bestimmt oder ermittelt die Beschreibung der Eigenschaft, die an die Value-Eigenschaft gebunden werden soll, um beispielsweise in Hilfe-Texten verarbeitet werden zu können.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property DatafieldDescription As String
End Interface
