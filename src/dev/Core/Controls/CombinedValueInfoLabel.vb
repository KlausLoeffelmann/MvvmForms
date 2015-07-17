Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Steuerelement, mit dessen Hilfe ein NullableValue zur Anzeige als Text mit 
''' Formatierungsoptionen gebracht werden kann. Derzeit KEINE getestete Mvvm-Unterstützung!!
''' </summary>
Public Class CombinedValueInfoLabel
    Inherits TextBox
    Implements IUpdatableInfoControl

    Dim myGroupName As String = "Default"

    Sub New()
        MyBase.New()
        Me.ReadOnly = True
        Me.BorderStyle = BorderStyle.FixedSingle
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt den in der TextBox dieses Steuerelementes angezeigten Wert, 
    ''' nachdem ein Elemente in der Liste ausgewählt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks><para>Der DisplayMember wird aus mindestens zwei Teilen als Zeichenfolge angegeben werden, 
    ''' die durch Komma getrennt werden. Der erste Teil - in Anführungszeichen - definiert die Formatierung, 
    ''' der zweite Teil, welche Eigenschaften der Datenquelle angezeigt werden sollen. 
    ''' Eigenschaftennamen werden in geschwifte Klammern gesetzt. Das ist wichtig, um zu einem späteren Zeitpunkt 
    ''' hier noch um Formelauswertungsfunktioanlitäten zu erweitern.
    ''' </para>
    ''' <para>Beispiel: Kundennummer 6-stellig und Betrag mit zwei Nachkomma, Tausendertrennzeichen und Euro-Symbol darstellen:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1:#,##0.00} €"", {Kundennummer},{Betrag}"
    ''' </code>
    ''' <para>Beispiel: Kundennummer 6-stellig, mit anschließendem Doppelpüunkt, Leerzeichen,
    ''' dann Nachname, Komma und Vorname:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1}, {2}"", {Kundennummer},{Nachname},{Vorname}"
    ''' </code>
    ''' </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DisplayMember As String

    ''' <summary>
    ''' Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property AssignedManagerControl As FormToBusinessClassManager Implements IAssignableFormToBusinessClassManager.AssignedManagerControl

    ''' <summary>
    ''' Aktualisiert die Darstellung.
    ''' </summary>
    ''' <param name="Datasource"></param>
    Public Sub UpdateDisplayData(ByVal Datasource As Object) Implements IUpdatableInfoControl.UpdateDisplayData
        Me.Text = ObjectAnalyser.ObjectToString(Datasource, Me.DisplayMember)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge 
    ''' das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)"),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(0)>
    Public Property ProcessingPriority As Integer Implements IAssignableFormToBusinessClassManager.ProcessingPriority

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz 
    ''' diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob eine FormToBusinessClassManager-Instanz diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property SelectedForProcessing As Boolean Implements IAssignableFormToBusinessClassManager.SelectedForProcessing

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue("Default")>
    Public Property GroupName As String Implements IAssignableFormToBusinessClassManager.GroupName
        Get
            Return myGroupName
        End Get
        Set(value As String)
            myGroupName = value
        End Set
    End Property

End Class
