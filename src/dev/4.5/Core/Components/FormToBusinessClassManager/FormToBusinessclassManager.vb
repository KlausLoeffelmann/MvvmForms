Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Reflection
Imports System.ComponentModel.Design
Imports System.Data.Objects
Imports System.Runtime.CompilerServices
Imports System.Numerics
Imports System.Drawing
Imports System.Data.Objects.DataClasses
Imports System.IO
Imports System.Drawing.Design

''' <summary>
''' Stellt Funktionalitäten zur Verfügung, mit denen die Daten in einem WindowsForms-Formular validiert und in eine Business-Objekt
''' (POCO) (und umgekehrt) übertragen werden können.
''' </summary>
''' <remarks>Voraussetzung für die Validierung und Verknüpfung eines Windows Forms-Formulars an ein Business-Objekt ist, 
''' dass die verwendeten Steuerelemente einen Satz von Funktionalitäten zur Verfügung stellen, über die sie vom 
''' FormsToBusinessClass-Manager verarbeitet werden können. 
''' <para>Um das zu gewährleiten, müssen die Steuerelemente des Formulars 
''' die <see cref="INullableValueDataBinding">INullableValueDataBinding-Schnittstelle</see> implementieren.</para>
''' <para><b>Steuerelemente für den FormsToBusinessClass-Manager:</b>
''' </para>
''' <para>Die EntitiesFormsLib-Assembly bietet einige Steuerelemente an, die diese Funktionalität bereits zur Verfügung stellen, 
''' und aus denen die Formulare aufgebaut werden sollten:
''' </para>
''' <para><list><see cref="NullableTextValue">NullableTextValue-Steuerelement:</see> Dient zur Eingabe von einzeiligen Textinhalten in Formularen.
''' </list></para>
''' <para><list><see cref="NullableMultilineTextValue">NullableMultilineTextValue-Steuerelement:</see> Dient zur Eingabe von mehrzeiligen Textinhalten in Formularen.
''' </list></para>
''' <para><list><see cref="NullableNumValue">NullableNumValue-Steuerelement:</see> Dient zur Eingabe formatierbaren numerischen Werten mit 
''' Nachkommastelen oder berechenbaren Formeln in Formularen. Basiert auf dem Decimal-Datentyp.
''' </list></para>
''' <para><list><see cref="NullableIntValue">NullableIntValue-Steuerelement:</see> Dient zur Eingabe von formatierbaren ganzzahligen numerischen Werten 
''' oder berechenbaren Formeln in Formularen. Basiert auf dem Integer-Datentyp.
''' </list></para>
''' <para><list><see cref="NullableDateValue">NullableDateValue-Steuerelement:</see> Dient zur Eingabe von formatierbaren Datumswerten 
''' die alternativ auch aus einem Auswahlkalender ausgewählt werden können.
''' </list></para>
''' <para><list><see cref="NullableValueRelationPopup">NullableValueRelationPopup-Steuerelement:</see> Dieses Steuerelement vereint 
''' für Lookup-Table-Funktionalitäten die Darstellung von Listen als DataGridView und eine konfigurierbare Suche als ComboBox (Aufklappliste).
''' </list></para>
''' <para><b>HINWEIS:</b>Alle hier gelisteten Steuerelemente können - entweder durch Implementierung der 
''' <see cref="INullableValueControl">INullableValueControl-Schnittstelle</see> oder durch Erben von der 
''' <see cref="NullableValueBase(Of NullableType, ControlType)">NullableValueBase(Of NullableType, ControlType)-Klasse</see> Null-Werte verarbeiten, wie sie 
''' im Umgang mit Werten aus Datenbanken häufig vorkommen. Jeder des Steuerelemente stellt zu diesem Zwecke eine NullValueString-Eigenschaft zur Verfügung, die 
''' die Zeichenfolge bestimmt, mit der ein Null-Wert für die Anzeige aufbereitet wird. Standardmäßig lautet diese Zeichenfolge <bold>* - - - *</bold>. Damit hat 
''' der Anwender selbst bei einem Textfeld die Möglichkeit, zwischen Null und einer leeren Zeichenfolge in einem Texteingabefeld 
''' (<see cref="NullableTextValue">NullableTextValue-Steuerelement</see>) zu unterscheiden.
''' </para>
''' </remarks>
<Designer(GetType(FormToBusinessClassManagerDesigner))>
Public Class FormToBusinessClassManager
    Inherits ErrorProvider
    Implements IDisposable

    Dim myHostingUserControl As UserControl
    Dim myDataSourceType As Type

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der Inhalt eines Steuerelementes eines Formulars, 
    ''' das eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet, geändert wird.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event IsFormDirtyChanged(ByVal sender As Object, ByVal e As IsFormDirtyChangedEventArgs)

    ''' <summary>
    ''' Wird ausgeführt, wenn die Validierung eines der Steuerelemente eines Formulars, 
    ''' das eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet, fehlschlägt.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValidationFailed(ByVal sender As Object, ByVal e As ValidationFailedEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, bevor die Validierung der Steuerelemente, die durch eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden, beginnt.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event BeforeFormValidating(ByVal sender As Object, ByVal e As CancelEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, nachdem die Validierung der Steuerelemente, die durch eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden, abgeschlossen ist.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event AfterFormValidated(ByVal sender As Object, ByVal e As EventArgs)

    ''' <summary>
    ''' Wird nach Aufruf der <see cref="ReceiveControlValues">ReceiveControlValues-Methode</see> für jedes Steuerelement ausgelöst, 
    ''' das durch eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet wird, um 
    ''' den aktuellen Wert des Steuerelementes abzufragen. DEBUG-Funktionalität.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValueForControlReceiving(ByVal sender As Object, ByVal e As ValueForControlReceivingEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> für die Zuweisung 
    ''' eines Relationsobjektes den aktuellen Objekt-Kontext benötigt, um die ausgewählte Entität der untergeordneten Tabelle dem entsprechenden 
    ''' Feld zuzuweisen.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter, mit deren Hilfe der Ziel-Objektkontext abgefragt werden kann.</param>
    ''' <remarks></remarks>
    Public Event GetTargetObjectContext(sender As Object, e As GetTargetObjectContextEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn bei der Zuweisung der Value-Eigenschaft eines INullableValue Controls 
    ''' ein Wert erkannt wurde, der zu einer Ausnahme führte.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter, mit deren Hilfe der Ziel-Objektkontext abgefragt werden kann.</param>
    ''' <remarks></remarks>
    Public Event UnassignableValueDetected(ByVal sender As Object, ByVal e As UnassignableValueDetectedEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> für die Zuweisung 
    ''' eines Relationsobjektes den aktuellen Objekt-Kontext benötigt, um die ausgewählte Entität der untergeordneten Tabelle dem entsprechenden 
    ''' Feld zuzuweisen.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">EventArgs, mit deren Hilfe der Ziel-Objektkontext abgefragt werden kann.</param>
    ''' <remarks></remarks>
    Public Event Value(sender As Object, e As GetTargetObjectContextEventArgs)

    ''' <summary>
    ''' Wird, wenn gesetzt aufgerufen, wenn die Änderungen im Formular gespeichert werden.
    ''' </summary>
    ''' <param name="par"></param>
    ''' <remarks></remarks>
    Public Delegate Sub SaveChanges(ByVal par As Object)

    Private myUpdatableInfoControls As UpdatableInfoControlCollection
    Private myBindingControls As NullableValueDataBindingControlCollection
    Private myEditorControls As NullableValueEditorControlCollection
    Private myHostingForm As Form
    Protected Friend myIsFormDirty As Boolean              ' Dieser Zustand von anderen ableitenden Komponenten in der gleichen Lib geändert werden können (MvvmManager, beispielsweise).
    Private mySaveChangesDelegate As SaveChanges
    Private myCancelButton As Button
    Private myCurrentEntityContext As ObjectContext
    Private myTemplateStartPos As Point = New Point(20, 20)
    Private myTemplateLabelSize As Size = New Size(80, 30)
    Private myTemplateInputControlSize As Size = New Size(200, 30)
    Private myTemplateAnchor As AnchorStyles = AnchorStyles.Left Or AnchorStyles.Top

    'Hinweis: Der letzte Eintrag ist kein Mapping, sondern nur ein Hinweis auf das Feld,
    'das verwendet wird, wenn aufgrund des Typen und der Namensgebung ('ID' vorne oder hinten) ein ForeignKey erkannt wurde.
    'Die Klasse ForeignKey ist nur eine Dummy-Definition, und hat keine weitere Funktion.
    Private myDatatypeMapping As New Dictionary(Of String, String) From { _
                    {GetType(Integer).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Long).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Byte).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Short).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Decimal).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Double).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Single).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Date).FullName, GetType(NullableDateValue).FullName},
                    {GetType(String).FullName, GetType(NullableTextValue).FullName},
                    {GetType(Char).FullName, GetType(NullableTextValue).FullName},
                    {GetType(Boolean).FullName, GetType(NullableCheckBox).FullName},
                    {GetType(Guid).FullName, GetType(NullableTextValue).FullName},
                    {GetType(Nullable(Of Integer)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Long)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Byte)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Short)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Decimal)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Double)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Single)).FullName, GetType(NullableNumValue).FullName},
                    {GetType(Nullable(Of Date)).FullName, GetType(NullableDateValue).FullName},
                    {GetType(Nullable(Of Boolean)).FullName, GetType(NullableCheckBox).FullName},
                    {GetType(Nullable(Of Char)).FullName, GetType(NullableTextValue).FullName},
                    {GetType(Nullable(Of Guid)).FullName, GetType(NullableTextValue).FullName},
                    {GetType(ForeignKey).FullName, GetType(NullableValueRelationPopup).FullName}
        }

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        MyBase.new()
        ThrowExceptionOnMissingDataSource = False
        AllowTypeNarrowing = True
    End Sub

    Private myLastSelection As EntityObjectSettings

    ''' <summary>
    ''' Ermittelt oder bestimmt die letzten Einstellungen für die ADO-Entity-Genrierung.
    ''' </summary>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
    EditorBrowsable(EditorBrowsableState.Never),
    Browsable(False)>
    Public Property LastSettings As EntityObjectSettings
        Get
            Return myLastSelection
        End Get
        Set(ByVal value As EntityObjectSettings)
            myLastSelection = value
        End Set
    End Property

    ''' <summary>
    ''' Internal infrastructural use - don't use directly.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ShouldSerializeLastSelection() As Boolean
        If myLastSelection Is Nothing Then Return False
        Return Not myLastSelection.CreatedBySerializer
    End Function

    ''' <summary>
    ''' Internal infrastructural use - don't use directly.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetLastSelection()
        myLastSelection.CreatedBySerializer = False
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt das Offset für alle Steuerelemente automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), Description("Bestimmt oder ermittelt das Offset für alle Steuerelemente automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes."),
     DefaultValue(GetType(Point), "20,20")>
    Public Property TemplateStartPos As Point
        Get
            Return myTemplateStartPos
        End Get
        Set(ByVal value As Point)
            myTemplateStartPos = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die Größe der Vorlage für Labels beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), Description("Bestimmt oder ermittelt die Größe der Vorlage für Labels beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes."),
     DefaultValue(GetType(Size), "80,30")>
    Public Property TemplateLabelSize As Size
        Get
            Return myTemplateLabelSize
        End Get
        Set(ByVal value As Size)
            myTemplateLabelSize = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt die Größe der Vorlage für Eingabefelder beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), Description("Bestimmt oder ermittelt die Größe der Vorlage für Eingabefelder beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes."),
     DefaultValue(GetType(Size), "200,30")>
    Public Property TemplateInputControlSize As Size
        Get
            Return myTemplateInputControlSize
        End Get
        Set(ByVal value As Size)
            myTemplateInputControlSize = value
        End Set
    End Property


    ''' <summary>
    ''' Bestimmt oder ermittelt die Vorgabeeinstellungen der Anchor-Eigenschaft der Eingabefelder beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), Description("Bestimmt oder ermittelt die Vorgabeeinstellungen der Anchor-Eigenschaft der Eingabefelder beim automatischen Aufbauen der Maske auf Basis eines Datenquellen-Objektes."),
     DefaultValue(GetType(AnchorStyles), "5")>
    Public Property TemplateAnchor As AnchorStyles
        Get
            Return myTemplateAnchor
        End Get
        Set(ByVal value As AnchorStyles)
            myTemplateAnchor = value
        End Set
    End Property

    ''' <summary>
    ''' Internal infrastructural use - don't use directly.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property DatatypeMapping As Dictionary(Of String, String)
        Get
            Return myDatatypeMapping
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            myDatatypeMapping = value
        End Set
    End Property

    'Wandelt das String/String-Typen-Mapping in eines vom Typ Type/Type um.
    Friend Function DataTypeMappingAsType() As Dictionary(Of Type, Type)
        Dim mappingAsType As New Dictionary(Of Type, Type)
        For Each item In DatatypeMapping
            mappingAsType.Add(Type.GetType(item.Key), Type.GetType(item.Value))
        Next
        Return mappingAsType
    End Function

    ''' <summary>
    ''' Internal infrastructural use - don't use directly.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Overrides WriteOnly Property Site() As ISite
        Set(ByVal value As ISite)
            MyBase.Site = value
            If value Is Nothing Then
                Return
            End If

            Dim host As IDesignerHost = TryCast(value.GetService(GetType(IDesignerHost)), IDesignerHost)
            If host IsNot Nothing Then
                Dim componentHost As IComponent = host.RootComponent
                If TypeOf componentHost Is Form Then
                    HostingForm = TryCast(componentHost, Form)
                ElseIf TypeOf componentHost Is UserControl Then
                    HostingUserControl = TryCast(componentHost, UserControl)
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt das Parent-Form, das diesem FormToBusinessClassManager zugeordnet ist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>HINWEIS: Diese Eigenschaft wird gesetzt, wenn Sie im Designer diese Komponente auf das Formular ziehen, und sie sollte 
    ''' anschließend nicht mehr geändert werden.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property HostingForm As Form
        Get
            Return myHostingForm
        End Get

        Set(ByVal value As Form)

            If value IsNot myHostingForm Then
                myHostingForm = value
                If myHostingForm IsNot Nothing Then
                    HostingUserControl = Nothing

                    'Jetzt nochmal das Load-Ereignis binden, damit wir die Steuerelemente bekommen,
                    'falls das Formular noch nicht geladen ist.
                    Dim loadHandler = Sub(s As Object, e As EventArgs)
                                          If myBindingControls IsNot Nothing Then
                                              myBindingControls.ForEach(
                                                  Sub(item)
                                                      'RemoveHandler item.IsDirtyChanged, AddressOf ControlDirtyHandlerInternal
                                                      'RemoveHandler item.RequestValidationFailedReaction, AddressOf FailedValidationHandlerInternal
                                                      'RemoveHandler item.ValueChanged, AddressOf ControlValueChangedInternal
                                                      Windows.WeakEventManager(Of IIsDirtyChangedAware, IsDirtyChangedEventArgs).RemoveHandler(item, "IsDirtyChanged", AddressOf ControlDirtyHandlerInternal)
                                                      Windows.WeakEventManager(Of INullableValueControl, RequestValidationFailedReactionEventArgs).RemoveHandler(item, "RequestValidationFailedReaction", AddressOf FailedValidationHandlerInternal)
                                                      Windows.WeakEventManager(Of INullableValueDataBinding, ValueChangedEventArgs).RemoveHandler(item, "ValueChanged", AddressOf ControlValueChangedInternal)
                                                  End Sub)
                                          Else
                                              If myHostingForm Is Nothing Then
                                                  Return
                                              End If
                                          End If

                                          myBindingControls = NullableValueDataBindingControlCollection.FromContainerControl(myHostingForm, Me)
                                          myUpdatableInfoControls = UpdatableInfoControlCollection.FromContainerControl(myHostingForm, Me)
                                          myEditorControls = NullableValueEditorControlCollection.FromContainerControl(myHostingForm, Me)
                                          myBindingControls.ForEach(
                                              Sub(item)
                                                  'AddHandler item.IsDirtyChanged, AddressOf ControlDirtyHandlerInternal
                                                  Windows.WeakEventManager(Of IIsDirtyChangedAware, IsDirtyChangedEventArgs).AddHandler(item, "IsDirtyChanged", AddressOf ControlDirtyHandlerInternal)
                                                  'AddHandler item.RequestValidationFailedReaction, AddressOf FailedValidationHandlerInternal
                                                  Windows.WeakEventManager(Of INullableValueControl, RequestValidationFailedReactionEventArgs).AddHandler(item, "RequestValidationFailedReaction", AddressOf FailedValidationHandlerInternal)
                                                  'AddHandler item.ValueChanged, AddressOf ControlValueChangedInternal
                                                  Windows.WeakEventManager(Of INullableValueDataBinding, ValueChangedEventArgs).AddHandler(item, "ValueChanged", AddressOf ControlValueChangedInternal)
                                              End Sub)
                                      End Sub

                    'Für den Fall, dass es das umgebende Formular noch nicht gibt, das HandleCreated-Ereignis binden, damit wir die Steuerelemente "mitbekommen".
                    AddHandler myHostingForm.HandleCreated, loadHandler

                    'Falls es das umgebende Formular jetzt schon gibt, die Steuerelemente schon jetzt einkassieren.
                    If myHostingForm.IsHandleCreated Then
                        loadHandler.Invoke(Me, EventArgs.Empty)
                    End If

                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt das Parent-Form, das diesem FormToBusinessClassManager zugeordnet ist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>HINWEIS: Diese Eigenschaft wird gesetzt, wenn Sie im Designer diese Komponente auf das Formular ziehen, und sie sollte 
    ''' anschließend nicht mehr geändert werden.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property HostingUserControl As UserControl
        Get
            Return myHostingUserControl
        End Get

        Set(ByVal value As UserControl)

            If value IsNot myHostingUserControl Then
                myHostingUserControl = value
                If myHostingUserControl IsNot Nothing Then
                    HostingForm = Nothing

                    'Jetzt nochmal das Load-Ereignis binden, damit wir die Steuerelemente bekommen,
                    'falls das Formular noch nicht geladen ist.
                    Dim loadHandler = Sub(s As Object, e As EventArgs)
                                          If myBindingControls IsNot Nothing Then
                                              myBindingControls.ForEach(Sub(item)
                                                                            'RemoveHandler item.IsDirtyChanged, AddressOf ControlDirtyHandlerInternal
                                                                            'RemoveHandler item.RequestValidationFailedReaction, AddressOf FailedValidationHandlerInternal
                                                                            'RemoveHandler item.ValueChanged, AddressOf ControlValueChangedInternal
                                                                            Windows.WeakEventManager(Of IIsDirtyChangedAware, IsDirtyChangedEventArgs).RemoveHandler(item, "IsDirtyChanged", AddressOf ControlDirtyHandlerInternal)
                                                                            Windows.WeakEventManager(Of INullableValueControl, RequestValidationFailedReactionEventArgs).RemoveHandler(item, "RequestValidationFailedReaction", AddressOf FailedValidationHandlerInternal)
                                                                            Windows.WeakEventManager(Of INullableValueDataBinding, ValueChangedEventArgs).RemoveHandler(item, "ValueChanged", AddressOf ControlValueChangedInternal)
                                                                        End Sub)
                                          Else
                                              If myHostingUserControl Is Nothing Then
                                                  Return
                                              End If
                                          End If

                                          myBindingControls = NullableValueDataBindingControlCollection.FromContainerControl(myHostingUserControl, Me)
                                          myUpdatableInfoControls = UpdatableInfoControlCollection.FromContainerControl(myHostingUserControl, Me)
                                          myEditorControls = NullableValueEditorControlCollection.FromContainerControl(myHostingUserControl, Me)
                                          myBindingControls.ForEach(Sub(item)
                                                                        'AddHandler item.IsDirtyChanged, AddressOf ControlDirtyHandlerInternal
                                                                        'AddHandler item.RequestValidationFailedReaction, AddressOf FailedValidationHandlerInternal
                                                                        'AddHandler item.ValueChanged, AddressOf ControlValueChangedInternal
                                                                        Windows.WeakEventManager(Of IIsDirtyChangedAware, IsDirtyChangedEventArgs).AddHandler(item, "IsDirtyChanged", AddressOf ControlDirtyHandlerInternal)
                                                                        Windows.WeakEventManager(Of INullableValueControl, RequestValidationFailedReactionEventArgs).AddHandler(item, "RequestValidationFailedReaction", AddressOf FailedValidationHandlerInternal)
                                                                        Windows.WeakEventManager(Of INullableValueDataBinding, ValueChangedEventArgs).AddHandler(item, "ValueChanged", AddressOf ControlValueChangedInternal)
                                                                    End Sub)
                                      End Sub

                    'Für den Fall, dass es das umgebende Steuerlement noch nicht gibt, das HandleCreated-Ereignis binden, damit wir die Steuerelemente "mitbekommen".
                    AddHandler myHostingUserControl.HandleCreated, loadHandler

                    'Falls es das umgebende Steuerelement jetzt schon gibt, die Steuerelemente schon jetzt einkassieren.
                    If myHostingUserControl.IsHandleCreated Then
                        loadHandler.Invoke(Me, EventArgs.Empty)
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Dieser Event wird jedesmal ausgelöst, sobald sich bei einem von dieser Instanz verwaltetes Control der Wert ändert.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ManagedControlValueChanged As EventHandler(Of ValueChangedEventArgs)

    Private Sub ControlValueChangedInternal(sender As Object, e As ValueChangedEventArgs)
        If Me.DatafieldLinkMode = DatafieldLinkModes.Automatic Then
            Me.AssignFieldsFromNullableControl(DirectCast(sender, INullableValueDataBinding))
        ElseIf Me.DatafieldLinkMode = DatafieldLinkModes.SelectedByDatafield And DirectCast(sender, INullableValueDataBinding).SelectedForProcessing Then
            Me.AssignFieldsFromNullableControl(DirectCast(sender, INullableValueDataBinding))
        End If

        RaiseEvent ManagedControlValueChanged(sender, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das IsDirty-handling dieser Instanz nach außen weitergegeben werden soll, oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(False)>
    Public Property SupressIsDirtyEvent As Boolean

    ''' <summary>
    ''' Bewirkt, dass für jedes Steuerelement, dass durch den FormToBusinessManager verwaltet wird, ein Ereignis ausgelöst wird, dass den Wert des Steuerelementes erhält.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReceiveControlValues()
        For Each item In myBindingControls
            Dim e As New ValueForControlReceivingEventArgs(False, item, item.Value)
            OnValueForControlReceiving(e)
            If e.Cancel Then
                Exit Sub
            End If
        Next
    End Sub

    ''' <summary>
    ''' Löst das ValueForControlReceiving-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValueForControlReceiving(ByVal e As ValueForControlReceivingEventArgs)
        RaiseEvent ValueForControlReceiving(Me, e)
    End Sub

    ''' <summary>
    ''' Löst das UnassignableValueDetected-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnUnassignableValueDetected(e As UnassignableValueDetectedEventArgs)
        RaiseEvent UnassignableValueDetected(Me, e)
    End Sub


    ''' <summary>
    ''' Bestimmt oder ermittelt die Schaltfläche auf dem Formular, der es erlaubt werden soll, 
    ''' auch bei Fehlern im Formular den Focus zu erhalten, da sie zum Abbruch des Vorgangs führt.
    ''' Die Validierung findet dabei - anders als bei CausesValidation = false - dennoch statt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     EditorBrowsable(EditorBrowsableState.Always),
     Description("Bestimmt oder ermittelt die Schaltfläche auf dem Formular, der es erlaubt werden soll, auch bei Fehlern im Formular den Focus zu erhalten, da sie zum Abbruch des Vorgangs führt."),
     Browsable(True)>
    Public Property CancelButton As Button
        Get
            Return myCancelButton
        End Get

        Set(ByVal value As Button)
            If value IsNot myCancelButton Then
                If value IsNot Nothing Then
                    myCancelButton = value
                End If
            End If
        End Set
    End Property

    Private Sub ControlDirtyHandlerInternal(ByVal sender As Object, ByVal e As EventArgs)
        If Not myIsFormDirty Then
            myIsFormDirty = True
            OnIsFormDirtyChanged(New IsFormDirtyChangedEventArgs(sender))
        End If
    End Sub

    Private Sub FailedValidationHandlerInternal(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs)
        Dim vfe As New ValidationFailedEventArgs(DirectCast(sender, INullableValueDataBinding), e)
        If myCancelButton IsNot Nothing Then
            If Me.HostingForm.ActiveControl Is myCancelButton Then
                vfe.OriginalEventArgs.ValidationFailedReaction = ValidationFailedActions.AllowLooseFocus
            End If
        End If
        OnValidationFailed(vfe)
    End Sub

    ''' <summary>
    ''' Löst das ValidationFailed-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValidationFailed(ByVal e As ValidationFailedEventArgs)
        RaiseEvent ValidationFailed(Me, e)
    End Sub

    ''' <summary>
    ''' Löst das IsFormDirtyChanged-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnIsFormDirtyChanged(ByVal e As IsFormDirtyChangedEventArgs)
        If Not Me.SupressIsDirtyEvent Then
            RaiseEvent IsFormDirtyChanged(Me, e)
        End If
    End Sub

    ''' <summary>
    ''' Ermittelt, ob Änderungen an den Eingabefeldern des Formulars vorgenommen wurden.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(False)>
    Public ReadOnly Property IsFormDirty As Boolean
        Get
            Return myIsFormDirty
        End Get
    End Property

    ''' <summary>
    ''' Setzt den 'Dirty'-Modus des Formulars zurück.
    ''' </summary>
    ''' <param name="NotifyPropertyChanged">Optionale Parameter, der bestimmt, ob im Bedarfsfall der 
    ''' IsFormDirtyChanged-Event ausgelöst werden soll, falls der IsFormDirty-Status zuvor signalisiert war.</param>
    ''' <remarks>Die Optionale Parameter wurde aus Abwärtskompatiblitätsgründen ergänzt.</remarks>
    Protected Overridable Sub ResetIsFormDirty(Optional NotifyPropertyChanged As Boolean = False)
        If myIsFormDirty Then
            myIsFormDirty = False
            If NotifyPropertyChanged Then
                OnIsFormDirtyChanged(New IsFormDirtyChangedEventArgs(Nothing))
            End If
        End If
    End Sub

    Public Overrides Function CreateObjRef(ByVal requestedType As System.Type) As System.Runtime.Remoting.ObjRef
        MessageBox.Show("CreateObjRef:" & requestedType.ToString)
        Return MyBase.CreateObjRef(requestedType)
    End Function

    Public Overrides Function InitializeLifetimeService() As Object
        MessageBox.Show("InitializeLifetimeService")
        Return MyBase.InitializeLifetimeService()
    End Function

    Protected Overrides Function GetService(ByVal service As System.Type) As Object
        MessageBox.Show("GetService:" & service.ToString)
        Return MyBase.GetService(service)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt die Datenquelle für diesen FormToBusinessClassManager.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <AttributeProvider(GetType(IListSource))>
    Public Overridable Overloads Property DataSource As Object
        Get
            Return MyBase.DataSource
        End Get

        Set(ByVal value As Object)
            If Not Object.Equals(value, MyBase.DataSource) Then
                MyBase.DataSource = value
                If Me.DesignMode Then
                    Return
                End If
                If DatafieldLinkMode = DatafieldLinkModes.Automatic Or DatafieldLinkMode = DatafieldLinkModes.SelectedByDatafield Then
                    'TODO: Wechsel des Currency-Managers mitbekommen!
                    AssignFieldsToNullableControls()
                End If
            End If
        End Set
    End Property

    <Editor(GetType(DataSourceTypeUIEditor), GetType(UITypeEditor))>
    Public Overridable Property DataSourceType As Type
        Get
            Return myDataSourceType
        End Get
        Set(value As Type)
            myDataSourceType = value
        End Set
    End Property

    Private Function GetCurrencyManagerFromDataSource() As CurrencyManager
        Dim tmpIList = TryCast(DataSource, IList)

        If tmpIList IsNot Nothing Then
            'Rausfinden, ob ein CurrencyManager implementiert ist, denn dann...
            Dim tmpICurrencyManager = TryCast(DataSource, CurrencyManager)
            If tmpICurrencyManager IsNot Nothing Then
                '...können wir gezielt das erste Objekt zurückgeben.
                Return tmpICurrencyManager
            End If
        End If
        Return Nothing
    End Function

    Private Function ObjectToBind() As Object

        'Erst rausfinden, ob ICollection oder IEnumerable implementiert ist.
        'Dann ist es nämlich eine Collection, und die können wir nicht direkt binden.
        'Nur wenn ein CurrencyManager dabei ist, wird es funktionieren, damit wir
        'das aktuelle Objekt zum Binden ermitteln können.
        Dim tmpICollection = TryCast(DataSource, ICollection)
        Dim tmpIEnumerable = TryCast(DataSource, IEnumerable)

        If tmpICollection IsNot Nothing OrElse tmpIEnumerable IsNot Nothing Then

            'Rausfinden, ob ein CurrencyManager...
            Dim tmpICurrencyManager = TryCast(DataSource, CurrencyManager)

            If tmpICurrencyManager Is Nothing Then
                '...oder ein CurrencyManager-Provider implementiert ist, denn dann...
                Dim tmpICurrencyManagerProvider = TryCast(DataSource, ICurrencyManagerProvider)
                If tmpICurrencyManagerProvider IsNot Nothing Then
                    tmpICurrencyManager = tmpICurrencyManagerProvider.CurrencyManager
                End If
            End If

            If tmpICurrencyManager IsNot Nothing Then
                '...können wir gezielt das erste Objekt zurückgeben.
                Return tmpICurrencyManager.Current
            End If

            'wird geben einfach das erste Objekt der Liste zurück.
            If tmpICollection IsNot Nothing Then
                If tmpICollection.Count > 0 Then
                    Return tmpICollection(0)
                End If
            ElseIf tmpIEnumerable IsNot Nothing Then
                Dim tmpEnum = tmpIEnumerable.GetEnumerator
                If tmpEnum IsNot Nothing Then
                    tmpEnum.Reset()
                    If tmpEnum.MoveNext Then
                        Return tmpEnum.Current
                    End If
                End If
            End If
        Else
            'Kein IEnumerable, kein ICollection, kein Currency-Manager - dann wird das Objekt direkt gebunden.
            If DataSource IsNot Nothing Then
                Return DataSource
            End If
        End If
        Return Nothing
    End Function

    Private Sub ClearMaskInternal()
        If myBindingControls Is Nothing Then Return
        For Each c As INullableValueDataBinding In (From item In myBindingControls Order By item.ProcessingPriority Descending)
            c.Value = Nothing
        Next
    End Sub

    ''' <summary>
    ''' Sorgt dafür, dass die Werte der Eigenschaften der DataSource den entsprechenden Feldern auf dem Formular zugeordnet werden.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AssignFieldsToNullableControls()
        Try
            If DataSource Is Nothing Then
                If ThrowExceptionOnMissingDataSource Then
                    Dim up As New NullReferenceException("Die DataSource-Eigenschaft der FormToBusinssClassManager-Komponente ist nicht gesetzt!")
                    Throw up
                Else
                    ClearMaskInternal()
                    Return
                End If
            End If

            'Info-Controls aktualisieren (die, die die komplette DataSource benötigen).
            myUpdatableInfoControls.ForEach(
                Sub(item)
                    item.UpdateDisplayData(DataSource)
                End Sub)


            For Each c As INullableValueDataBinding In (From item In myBindingControls Order By item.ProcessingPriority Descending)
                AssignFieldsToNullableControl(c)
            Next
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' Weist dem angegebenen NullableValueDataBinding-Steuerelement die entsprechende Eigenschaft der Datenquelle zu.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <remarks></remarks>
    Public Sub AssignFieldsToNullableControl(c As INullableValueDataBinding)

        If String.IsNullOrWhiteSpace(c.DatafieldName) Then
            Throw New MissingDatafieldNameException("The DataBinding Control '" & DirectCast(c, Control).Name & "' is missing its DatafieldName property setting.")
        End If


        Dim bindingObject = Me.ObjectToBind
#If DEBUG Then
        If c.DatafieldName.ToUpper.Contains("PARTNERNR") And bindingObject Is Nothing Then
            bindingObject = Me.ObjectToBind
        End If
#End If

        'Is Dirty darf hier nicht ausgelöst werden, ...
        c.IsLoading.Value = True

        'gleichzeitig müssen wir es hier zurücksetzen.
        c.ResetIsDirty()


        Dim temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(bindingObject, c.DatafieldName)
        If temp Is Nothing OrElse temp.Item2 Is Nothing Then
            Throw New MissingDatafieldNameException("The DataBinding Control '" & DirectCast(c, Control).Name & "' consists of an invalid property path.")
        End If

        Dim datafieldName = temp.Item2.Name
        bindingObject = temp.Item1

        Dim dataSourceProperty As PropertyInfo = bindingObject.GetType.GetProperty(datafieldName)
        If dataSourceProperty IsNot Nothing Then
            'Ist bereits generischer Typ, dann muss nichts gemacht werden.
            If dataSourceProperty.PropertyType.IsGenericType Then
                c.Value = dataSourceProperty.GetValue(bindingObject, Nothing)
            Else
                'Testen, ob es kein primitiver Datentyp oder String/StringValue ist.
                If Not (dataSourceProperty.PropertyType.IsPrimitive Or
                        (dataSourceProperty.PropertyType Is GetType(String)) Or
                        (dataSourceProperty.PropertyType Is GetType(StringValue))) Then
                    c.Value = dataSourceProperty.GetValue(bindingObject, Nothing)
                ElseIf dataSourceProperty.PropertyType.IsPrimitive Then
                    Try
                        c.Value = dataSourceProperty.GetValue(bindingObject, Nothing)
                    Catch ex As Exception
                        Dim e As New UnassignableValueDetectedEventArgs(False, Nothing)
                        OnUnassignableValueDetected(e)
                        If e.ExceptionHandled Then
                            Try
                                c.Value = e.NewValue
                            Catch ex2 As Exception
                                Dim valueAsString As String
                                If e.NewValue IsNot Nothing Then
                                    valueAsString = e.NewValue.ToString
                                Else
                                    valueAsString = "[NULL]"
                                End If
                                Dim paraName = "Value konnte nicht zugewiesen werden. (Value=" & valueAsString & ")"
                                Dim up As New UnassignableValueException(paraName, e.NewValue, "Der zugewiesene Value-Wert hatte keine Entsprechung in der Popup-Objektliste." & vbNewLine &
                                                                          "Behandeln Sie das ValueChanging- oder das UnassignableValueDetected-Ereignis, um bestimmte Werte auszutauschen, " & vbNewLine &
                                                                          "oder setzen Sie die OnUnassigableValueAction-Eigenschaft auf eine andere Aktion." & vbNewLine & vbNewLine &
                                                                          "Falls Sie das UnassignableValueDetected-Ereignis behandelt haben, prüfen Sie, ob Sie e.ExceptionHandled=true im Ereignishandler gesetzt haben!", c)
                            End Try
                        Else
                            Throw ex
                        End If
                    End Try
                Else
                    'komplizierter ist's bei primitiven Typen:
                    'Hier muss NullableFromObject aus dieser Klasse per Reflection eingebunden werden,
                    'um aus den primitiven Typen das generisches Äquivalent zu machen, denn die Steuerelemente verarbeiten ja
                    'nur Nullable(Of ) irgendwas, was insbesondere bei Strings die Krücke über StringValue erforderlich macht.
                    'Having said this:
                    'Bei der Zuweisung einer Eigenschaft an ein RelationalPopup können auch nicht-generische Eigenschaften an dieser
                    'Stelle auftreten; deswegen reicht es nicht, sofort den generischen ValueTyp der aktuellen Value-Eigenschaft zu ermitteln,
                    'sondern erstmal zu schauen, ob wir als Basiseigenschaft 'Object' bekommen und damit erst zu hantieren.
                    'Das bislang gesagte kommt dann anschließend.
                    Dim testForNonGenericObjectType = c.GetType.GetProperty("Value").PropertyType
                    If testForNonGenericObjectType Is GetType(Object) Then
                        'Hier ist das Steuerelement jetzt auf sich alleine gestellt, und muss selber sehen,
                        'dass es aus dem, was es bekommt, das richtige verarbeitet (Object:Object-Binding)
                        c.Value = dataSourceProperty.GetValue(bindingObject, Nothing)
                    Else
                        Dim genericBasedType = testForNonGenericObjectType.GetGenericArguments(0)
                        If genericBasedType Is GetType(StringValue) Then
                            'Methode, die String in Nullable(of StringValue) umwandelt
                            Dim toObjectMethod As MethodInfo = Me.GetType.GetMethod("NullableFromString",
                                                                                    BindingFlags.NonPublic Or
                                                                                    BindingFlags.Instance)
                            'Methode aufrufen
                            Dim tmpObject As Object = toObjectMethod.Invoke(Me, New Object() {dataSourceProperty.GetValue(bindingObject, Nothing)})
                            'Eigenschaft zuweisen
                            c.Value = tmpObject
                        Else
                            'Methode, die primitiv in Nullable umwandelt
                            Dim toObjectMethod As MethodInfo = Me.GetType.GetMethod("NullableFromObject",
                                                                                    BindingFlags.NonPublic Or BindingFlags.Instance)

                            'Eine generische Methode daraus machen (dazu den generischen TypParameter festlegen)
                            toObjectMethod = toObjectMethod.MakeGenericMethod(New Type() {genericBasedType})
                            'Wert erstmal als Objekt ermitteln, und dann in INullableValueDataBinding casten.
                            Dim tmpObject As Object = toObjectMethod.Invoke(Me, New Object() {dataSourceProperty.GetValue(bindingObject, Nothing)})
                            c.Value = tmpObject
                        End If
                    End If
                End If
            End If
        Else
            Dim up As New MissingMemberException("Die Eigenschaft mit dem Namen '" & c.DatafieldName & "' konnte nicht gefunden werden.")
            Throw up
        End If

        'Jetzt darf wieder geändert werden.
        c.IsLoading.Value = False
    End Sub

    ''' <summary>
    ''' Versetzt den "geändert"-Status der Maske und aller betroffenen Steuerelemente wieder in den Ausgangsmodus.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub CancelEdit()
        If myBindingControls Is Nothing OrElse
            myBindingControls.Count = 0 Then
            Exit Sub
        End If

        For Each c As INullableValueDataBinding In myBindingControls
            c.ResetIsDirty()
        Next

        Me.ResetIsFormDirty()
    End Sub

    ''' <summary>
    ''' Versetzt den "geändert"-Status der Maske und aller betroffenen Steuerelemente 
    ''' wieder in Ausgangsmodus und stellt im Bedarfsfall die Ursprungswerte in den Steuerelementen wieder her.
    ''' </summary>
    ''' <param name="restoreData"></param>
    ''' <remarks></remarks>
    Public Sub CancelEdit(ByVal restoreData As Boolean)
        If restoreData Then
            AssignFieldsToNullableControls()
            ResetIsFormDirty()
        Else
            CancelEdit()
        End If
    End Sub

    ''' <summary>
    ''' Führt eine Validierung der entsprechenden Steuerelemente im Formular durch.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateForm() As ValidationIssues

        Dim valIssues As New ValidationIssues()

        For Each controlItem In (From item In myEditorControls Order By item.ProcessingPriority Descending)
            Dim ex = controlItem.ValidateContent
            If ex IsNot Nothing Then
                controlItem.SetFailedValidation(ex.Message)
                valIssues.Add(New ValidationIssue With {.ControlCausedFailedValidation = controlItem,
                                                        .ValidationException = ex,
                                                        .Message = ex.Message})
            Else
                controlItem.ClearFailedValidation()
            End If
        Next

        For Each controlItem In (From item In myBindingControls Order By item.ProcessingPriority Descending)
            If Not String.IsNullOrWhiteSpace(controlItem.NullValueMessage) Then
                Dim tmpValue As Object = Nothing
                Dim tmpEx As Exception = Nothing
                Try
                    tmpValue = controlItem.Value
                Catch ex As Exception
                    tmpEx = ex
                End Try

                If tmpValue Is Nothing Then
                    If tmpEx IsNot Nothing Then
                        valIssues.Add(New ValidationIssue With {.ControlCausedFailedValidation = controlItem,
                                                                .ValidationException = tmpEx,
                                                                .Message = tmpEx.Message})
                    Else
                        valIssues.Add(New ValidationIssue With {.ControlCausedFailedValidation = controlItem,
                                        .ValidationException = New NullReferenceException(controlItem.NullValueMessage),
                                        .Message = controlItem.NullValueMessage})
                    End If
                End If
            End If
        Next
        Return valIssues
    End Function

    ''' <summary>
    ''' Löst das BeforeFormValidating-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Friend Overridable Sub OnBeforeFormValidating(ByVal e As CancelEventArgs)
        RaiseEvent BeforeFormValidating(Me, e)
    End Sub

    ''' <summary>
    ''' Löst das AfterFormValidated-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Friend Overridable Sub OnAfterFormValidated(ByVal e As AfterFormValidatedEventArgs)
        RaiseEvent AfterFormValidated(Me, e)
    End Sub

    ''' <summary>
    ''' Sorgt dafür, dass die Werte der entsprechenden Felder auf dem Formular wieder in die Eigenschaften der Datenquelle übertragen werden.
    ''' </summary>
    Public Overridable Sub AssignFieldsFromNullableControls()
        If myBindingControls IsNot Nothing Then
            For Each c As INullableValueDataBinding In (From item In myBindingControls Order By item.ProcessingPriority Descending)
                AssignFieldsFromNullableControl(c)
            Next
        End If
    End Sub

    ''' <summary>
    ''' Weist den Wert des Steuerelementes der entsprechende Eigenschaft der DataSource zu.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <remarks>Voraussetzung dafür ist, dass die <see cref="EntitiesFormsLib.INullableValueDataBinding.DatafieldName"></see> des Steuerelementes gesetzt ist. 
    ''' Dabei ist die Angabe verschachteleter Eigenschaften (Eigenschaftenpfade) durchaus möglich, also beispielsweise <b>Hauptadresse.Vorname</b>, falls die 
    ''' DataSource über eine Eigenschaft <b>Hauptadresse</b> vom Typ <b>Adresse</b> verfügt, und der Adresse-Typ eine <b>Vorname</b>-Eigenschaft zur Verfügung stellt.</remarks>
    Public Sub AssignFieldsFromNullableControl(c As INullableValueDataBinding)

        If DataSource Is Nothing Then
            If ThrowExceptionOnMissingDataSource Then
                Dim up As New NullReferenceException("Die DataSource-Eigenschaft der FormToBusinssClassManager-Komponente ist nicht gesetzt!")
                Throw up
            Else
                'Do Nothing
                Return
            End If
        End If

        If String.IsNullOrWhiteSpace(c.DatafieldName) Then
            Throw New MissingDatafieldNameException("The DataBinding Control '" & DirectCast(c, Control).Name & "' is missing its DatafieldName property setting.")
        End If

        'Zwischenspeichern
        Dim bindingObject = Me.ObjectToBind

        'Eigenschaftpfad auflösen
        Dim temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(bindingObject, c.DatafieldName)

        'Konnte nicht aufgelöst werden, dann Exception...
        If temp Is Nothing OrElse temp.Item2 Is Nothing Then
            Throw New MissingDatafieldNameException("The DataBinding Control '" & DirectCast(c, Control).Name & "' consists of an invalid property path.")
        End If

        'Neues DataSource-Objekt und neuen Eigenschaftennamen (aufgelöster Property Path) ab jetzt verwenden.
        Dim datafieldName = temp.Item2.Name
        bindingObject = temp.Item1

        'Namen der Eigenschaft ermitteln
        Dim dataSourceProperty As PropertyInfo = bindingObject.GetType.GetProperty(datafieldName)

        If dataSourceProperty IsNot Nothing Then

            'Zeiger (zum Beispiel EntityObjects), aber keine Strings! werden einfach umkopiert.
            If dataSourceProperty.PropertyType.IsClass AndAlso Not (dataSourceProperty.PropertyType Is GetType(String)) Then

                'Entity Objekt?
                If GetType(EntityObject).IsAssignableFrom(dataSourceProperty.PropertyType) Then

                    'Yep - Entity-Object.
                    Dim targetEntityObject = DirectCast(dataSourceProperty.GetValue(bindingObject, Nothing), EntityObject)
                    Dim sourceEntityObject As EntityObject
                    Try
                        sourceEntityObject = DirectCast(c.Value, EntityObject)
                    Catch ex As Exception
                        Dim up As New TypeMismatchException("Assigning a value type or a primitive type to a navigation property is not possible." & vbNewLine &
                                                            "Please check the databinding, the ValuaMember seems not to be compatible with the type of the property it is assigned to. (Control: " &
                                                            DirectCast(c, Control).Name & ")", ex)
                        Throw up
                    End Try

                    'Typenvergleich. Falls nicht passt --> Peng, bumm!
                    If targetEntityObject IsNot Nothing Then
                        If targetEntityObject.GetType IsNot sourceEntityObject.GetType Then
                            Throw New TypeMismatchException("Das zugewiesene Entity-Objekt ist nicht vom gleichen Typ wie das Ziel-Entity-Objekt!")
                        End If

                        'Entity-Key-Vergleich. Wenn das passt, sind wir durch, weil sich der Entity-Key in der Zuordnung verändern muss,
                        'wenn sich die Relation geändert hat.

                        Dim keysmatch = True
                        For otherCounter = 0 To targetEntityObject.EntityKey.EntityKeyValues.Count - 1
                            If targetEntityObject.EntityKey.EntityKeyValues(otherCounter).Key <> sourceEntityObject.EntityKey.EntityKeyValues(otherCounter).Key OrElse
                                targetEntityObject.EntityKey.EntityKeyValues(otherCounter).Value.ToString <> sourceEntityObject.EntityKey.EntityKeyValues(otherCounter).Value.ToString Then
                                keysmatch = False
                                Exit For
                            End If
                        Next

                        'Entity-Keys stimmen überein - wir müssen hier nix machen.
                        If keysmatch Then
                            Exit Sub
                        End If
                    End If

                    'Objekt-Context ermitteln - geht zuverlässig nur per Ereignis
                    Dim eTargObjContext As New GetTargetObjectContextEventArgs()
                    OnGetTargetObjectContextEventArgs(eTargObjContext)
                    If eTargObjContext.TargetObjectContext IsNot Nothing Then

                        'Neue Instanz erstellen
                        Dim newEntity = DirectCast(dataSourceProperty.PropertyType.InvokeMember(".ctor",
                                                    BindingFlags.CreateInstance, Nothing, Nothing, Nothing), EntityObject)

                        If GetType(EntityObject).IsAssignableFrom(c.Value.GetType) Then
                            For Each item As PropertyInfo In DirectCast(c.Value, EntityObject).GetType().GetProperties

                                Dim ScalarInEDM = item.GetCustomAttributes(GetType(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), True)
                                If ScalarInEDM.Length > 0 Then
                                    newEntity.GetType.GetProperty(item.Name).SetValue(newEntity, item.GetValue(c.Value, Nothing), Nothing)
                                End If
                            Next

                            'Entity-Key kopieren
                            newEntity.EntityKey = DirectCast(c.Value, EntityObject).EntityKey

                            'Dem Objekt-Context hinzufügen
                            eTargObjContext.TargetObjectContext.Attach(newEntity)
                            dataSourceProperty.SetValue(bindingObject, newEntity, Nothing)
                        Else
                            dataSourceProperty.SetValue(bindingObject, c.Value, Nothing)
                        End If
                    Else
                        Dim up As New MissingObjectContextException("You have to handle the GetTargetObjectContextEvent in order to assign a relation value of another table to a navigation property." & vbNewLine &
                                    "Please handle this event by assigning the target object context to the TargetObjectContext property of the EventArgs. (Control: " &
                                    DirectCast(c, Control).Name & ")")
                        Throw up
                    End If
                Else

IsNoEntityObjectOrEntityObjectNotApplicable:
                    dataSourceProperty.SetValue(bindingObject, c.Value, Nothing)
                End If
                'Ist bereits generischer Typ, dann muss nichts gemacht werden.
            ElseIf (dataSourceProperty.PropertyType.IsPrimitive Or
                        (dataSourceProperty.PropertyType Is GetType(String)) Or
                        (dataSourceProperty.PropertyType Is GetType(StringValue))) Then

                'Falls es ein numerischer Typ ist, und narrowing erlaubt ist, mit Convert aufrufen.
                Dim convertedValue As Object = Nothing

                'Wenn Primitiver Typ oder Generic(Of Primitiver Typ) dann und Type Narrowing (Decimal nach Integer zum Beispiel) erlaubt ist, dann:
                If c.Value IsNot Nothing AndAlso (IsNumeric(c.Value.GetType) OrElse IsGenericNumeric(c.Value.GetType)) AndAlso AllowTypeNarrowing Then
                    Dim toType As Type = dataSourceProperty.PropertyType
                    'Typ ändern!
                    convertedValue = Convert.ChangeType(c.Value, toType)
                Else
                    'Sonst Typ nicht konvertieren, es sei denn wir müssen noch zurück von ...
                    If c.Value IsNot Nothing AndAlso c.Value.GetType Is GetType(StringValue) Then
                        '...StringValue zu String.
                        convertedValue = c.Value.ToString
                    Else
                        convertedValue = c.Value
                    End If
                End If

                Try
                    dataSourceProperty.SetValue(bindingObject, convertedValue, Nothing)
                Catch ex As Exception
                    Dim up As New TypeMismatchException("Beim Zuweisen einer Value-Eigenschaft an die Eigenschaft '" & dataSourceProperty.Name & "' ist ein Fehler aufgetreten." & vbNewLine & _
                                                        "Überprüfen Sie, ob die Typen kompatibel sind. Eventuell Setzen Sie die AllowTypeNarrowing-Eigenschaft der FormToBusinessManager-Komponente auf True, um Typverkleinerungen zum Beispiel vom 'Decimal' zu 'Single' zu erlauben.")
                    Throw up
                End Try
            Else
                If c.Value Is Nothing Then
                    'Nothing Sonderbehandlung für generische Typen
                    dataSourceProperty.SetValue(bindingObject, Nothing, Nothing)
                Else
                    'komplizierter ist's bei primitiven Typen:
                    'Hier muss NullableFromObject aus dieser Klasse per Reflection eingebunden werden,
                    'um aus den primitiven Typen das generisches Äquivalent zu machen.
                    Dim genericBasedType As Type
                    If dataSourceProperty.PropertyType.IsGenericType Then
                        genericBasedType = dataSourceProperty.PropertyType.GetGenericArguments(0)
                    Else
                        genericBasedType = dataSourceProperty.PropertyType
                    End If
                    'Methode, die primitiv in Nullable umwandelt
                    Dim toObjectMethod As MethodInfo = Me.GetType.GetMethod("NullableToObject",
                                                                            BindingFlags.NonPublic Or BindingFlags.Instance)

                    'Eine generische Methode daraus machen (dazu den generischen TypParameter festlegen)
                    toObjectMethod = toObjectMethod.MakeGenericMethod(New Type() {genericBasedType})
                    'Wert erstmal als Objekt ermitteln, und dann in INullableValueDataBinding casten.
                    'TODO: Wichtig: AllowNarrowing für Generics berücksichtigen!!!
                    Dim tmpObject As Object = toObjectMethod.Invoke(Me, New Object() {Convert.ChangeType(c.Value, genericBasedType)})
                    If Not tmpObject Is Nothing Then
                        If tmpObject.GetType Is GetType(StringValue) Then
                            'String-Value auflösen
                            tmpObject = tmpObject.ToString
                        End If
                    End If
                    dataSourceProperty.SetValue(bindingObject, tmpObject, Nothing)
                End If
            End If
        Else
            Dim up As New MissingMemberException("Die Eigenschaft mit dem Namen '" & c.DatafieldName & "' konnte nicht gefunden werden.")
            Throw up
        End If
    End Sub

    ''' <summary>
    ''' Löst das GetTargetObjectContext-Ereignis aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnGetTargetObjectContextEventArgs(e As GetTargetObjectContextEventArgs)
        RaiseEvent GetTargetObjectContext(Me, e)
    End Sub

    Friend Function NullableFromString(ByVal value As Object) As Nullable(Of StringValue)
        If value Is Nothing Then
            Return Nothing
        Else
            Dim s As String
            If value.GetType Is GetType(String) Then
                s = value.ToString
            ElseIf value.GetType Is GetType(StringValue) Then
                s = value.ToString
            Else
                Dim up As New InvalidCastException("Object is not of of type string and can't be converted to a Nullable<StringValue>!")
                Throw up
            End If
            Return New Nullable(Of StringValue)(New StringValue(s))
        End If
    End Function

    Friend Function NullableFromObject(Of PrimType As {IComparable, Structure})(ByVal value As Object) As Nullable(Of PrimType)
        Dim retNullable As New Nullable(Of PrimType)
        If (value Is Nothing) Then
            Return retNullable
        ElseIf (TypeOf value Is DBNull) Then
            Return retNullable
        End If

        If TypeOf value Is PrimType Then
            retNullable = CTypeDynamic(Of PrimType)(value)
            Return retNullable
        End If

        'Ein Versuch ist es Wert!
        Try
            retNullable = CTypeDynamic(Of PrimType)(value)
            Return retNullable
        Catch ex As Exception
            Dim up As New InvalidCastException("Object is not of the correct type!")
            Throw up
        End Try
    End Function

    ''' <summary>
    ''' Infrastructural. 
    ''' </summary>
    ''' <typeparam name="PrimType">Der zugrundeliegende Basistyp</typeparam>
    ''' <param name="value">In seinen primitiven Typ zu konvertierender ADDBNullable</param>
    ''' <returns>Object mit geboxtem primitivem Typ</returns>
    ''' <remarks></remarks>
    Friend Function NullableToObject(Of PrimType As {IComparable, Structure})(ByVal value As Nullable(Of PrimType)) As Object
        If Not value.HasValue Then
            Return Nothing
        End If
        Return CType(value.Value, PrimType)
    End Function

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine Ausnahme ausgelöst werden soll, wenn keine Datenquelle bestimmt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DefaultValue(False)> _
    Property ThrowExceptionOnMissingDataSource As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DefaultValue(DatafieldLinkModes.Manual)> _
    Property DatafieldLinkMode As DatafieldLinkModes

    <DefaultValue(True)> _
    Property AllowTypeNarrowing As Boolean

    ''' <summary>
    ''' Erlaubt das Speichern eines Objektcontexts zur Übergabe an das Formular und zum Zugriff über die Schnittstelle IFormToBusinessClassManager. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Diese Eigenschaft dient nur zur Übergabe eines ObjektContexts im Bedarfsfall, damit zum Beispiel mit einem selben Objektkontext
    ''' auch Relations-Steuerelemente wie Tabellen, Listen oder ComboBoxen gefüllt werden können, bevor die Maske bearbeitet wird.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(False)>
    Property CurrentEntityContext As ObjectContext
        Get
            Return myCurrentEntityContext
        End Get
        Set(ByVal value As ObjectContext)
            myCurrentEntityContext = value
        End Set
    End Property

    ''' <summary>
    ''' Not implemented yet. Stellt eine Liste aller Ereignisse für die entsprechenden Steuerelemente im Formular zur Verfügung, die durch den FTBCM verwaltet werden können.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property DynamicEventHandlingList As List(Of DynamicEventsItem)

    ''' <summary>
    ''' Diese Klasse ist ein Zwitter. Wird vom FormToBuisnessclassManager erstellt, sind alle Werte gesetzt. 
    ''' Wird sie jedoch durch den Deserialisierer des Designers erstellt sind nur die *Namen-Felder belegt
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Class EntityObjectSettings
        <NonSerialized()>
        Private myParentControl As Control
        Private myParentControlName As String

        <NonSerialized()>
        Private myAssemblyWithEntityObject As Assembly
        Private myAssemblyWithEntityObjectName As String

        <NonSerialized()>
        Private myEntityObjectType As Type
        Private myEntityObjectTypeName As String

        Private myListOfFields As List(Of PropertyCheckBoxItemController)   ' wird auch vom Deserialisierer verwendet

        Private myCreatedBySerializer As Boolean = True

        ''' <summary>
        ''' ACHTUNG: Nicht verwenden, wird nur für die Deseriealisierung benötigt
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Für die Verwendung vom Designer
        ''' </summary>
        ''' <param name="parentControl"></param>
        ''' <param name="assemblyWithEntityObject"></param>
        ''' <param name="entityObjectType"></param>
        ''' <param name="listOfFields"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal parentControl As Control, ByVal assemblyWithEntityObject As Assembly, ByVal entityObjectType As Type, ByVal listOfFields As List(Of PropertyCheckBoxItemController))
            If parentControl Is Nothing Then Throw New ArgumentNullException("parentControl")
            If assemblyWithEntityObject Is Nothing Then Throw New ArgumentNullException("assemblyWithEntityObject")
            If entityObjectType Is Nothing Then Throw New ArgumentNullException("entityObjectType")
            If listOfFields Is Nothing Then Throw New ArgumentNullException("listOfFields")
            If listOfFields.Count = 0 Then Throw New ArgumentException("Es sind keine Elemente in dem Parameter 'properties' vorhanden")

            myParentControl = parentControl
            myAssemblyWithEntityObject = assemblyWithEntityObject
            myEntityObjectType = entityObjectType
            myListOfFields = listOfFields
            myCreatedBySerializer = False

            myParentControlName = myParentControl.Name
            myAssemblyWithEntityObjectName = myAssemblyWithEntityObject.GetName.ToString
            myEntityObjectTypeName = myEntityObjectType.FullName
        End Sub

        Public ReadOnly Property ParentControl As Control
            Get
                If myCreatedBySerializer Then Throw New InvalidOperationException("Die Klasse wurde vom Deserialisierer erstellt. Diese Property ist nicht gesetzt.")
                Return myParentControl
            End Get
        End Property

        Public ReadOnly Property AssemblyWithEntityObject As Assembly
            Get
                If myCreatedBySerializer Then Throw New InvalidOperationException("Die Klasse wurde vom Deserialisierer erstellt. Diese Property ist nicht gesetzt.")
                Return myAssemblyWithEntityObject
            End Get
        End Property

        Public ReadOnly Property EntityObjectType As Type
            Get
                If myCreatedBySerializer Then Throw New InvalidOperationException("Die Klasse wurde vom Deserialisierer erstellt. Diese Property ist nicht gesetzt.")
                Return myEntityObjectType
            End Get
        End Property

        Public Property ListOfFields As List(Of PropertyCheckBoxItemController)
            Get
                Return myListOfFields
            End Get
            Set(ByVal value As List(Of PropertyCheckBoxItemController))
                myListOfFields = value
            End Set
        End Property

        Public Property CreatedBySerializer As Boolean
            Get
                Return myCreatedBySerializer
            End Get
            Set(ByVal value As Boolean)
                myCreatedBySerializer = value
            End Set
        End Property

        Public Property ParentControlName As String
            Get
                Return myParentControlName
            End Get
            Set(ByVal value As String)
                myParentControlName = value
            End Set
        End Property

        Public Property AssemblyWithEntityObjectName As String
            Get
                Return myAssemblyWithEntityObjectName
            End Get
            Set(ByVal value As String)
                myAssemblyWithEntityObjectName = value
            End Set
        End Property

        Public Property EntityObjectTypeName As String
            Get
                Return myEntityObjectTypeName
            End Get
            Set(ByVal value As String)
                myEntityObjectTypeName = value
            End Set
        End Property
    End Class

End Class

Public Class DynamicEventsItem

    Public Sub New()
        MyBase.new()
    End Sub

    <TypeConverter(GetType(TypeListNameConverter)), TypeList(GetType(Control), GetType(INullableValueControl))>
    Public Property ControlType As String
    Public Property EventName As String
End Class

Public Class TypeListAttribute
    Inherits Attribute

    Private myBaseTypeNames As List(Of Type)
    Private myTypeList As List(Of String)

    Sub New(ByVal ParamArray basetypes As Type())
        myBaseTypeNames = basetypes.ToList
    End Sub

    ReadOnly Property TypeList As List(Of String)
        Get
            myTypeList = New List(Of String)
            For Each item In myBaseTypeNames
                Dim myCurrentType = item
                If myCurrentType IsNot Nothing Then
                    myTypeList.Add(myCurrentType.Name)
                    For Each item2 In myCurrentType.Assembly.GetTypes
                        If myCurrentType.IsAssignableFrom(item2) Then
                            myTypeList.Add(item2.Name)
                        End If
                    Next
                End If
            Next
            Return myTypeList
        End Get
    End Property
End Class

Public Class TypeListNameConverter
    Inherits StringConverter

    Public Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Debug.Print("GetStandardValuesSupported")
        Return True
    End Function

    Public Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(DirectCast(context.PropertyDescriptor.Attributes(GetType(TypeListAttribute)), TypeListAttribute).TypeList)
    End Function
End Class

''' <summary>
''' Einstellungen, die bestimmen, wie die Daten aus den Steuerelementen in den Datenquellen gelangen.
''' </summary>
''' <remarks></remarks>
Public Enum DatafieldLinkModes
    ''' <summary>
    ''' Das Übertragen der Werte von den Datenfeldern zur Datenquelle und umgekehrt wird mit den Methoden 
    ''' <see cref="FormToBusinessClassManager.AssignFieldsFromNullableControls">AssignFieldsFromNullableControls</see> sowie 
    ''' <see cref="FormToBusinessClassManager.AssignFieldsToNullableControls">AssignFieldsToNullableControls</see> durchgeführt.
    ''' </summary>
    ''' <remarks></remarks>
    Manual

    ''' <summary>
    ''' Das Übertragen der Werte von den Datenfeldern zur Datenquelle passiert automatisch, sobald das ValueChanged-Ereignis eingetreten ist.
    ''' </summary>
    ''' <remarks></remarks>
    Automatic

    ''' <summary>
    ''' Das Übertragen der Werte von den Datenfeldern zur Datenquelle passiert automatisch nur für die Steuerelemente, deren 
    ''' <see cref="IAssignableFormToBusinessClassManager.SelectedForProcessing">SelectedForProcessing-Eigenschaft</see> auf true gesetzt ist.
    ''' </summary>
    ''' <remarks></remarks>
    SelectedByDatafield
End Enum
