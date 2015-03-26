Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Windows.Data
Imports System.Drawing.Design
Imports System.Collections.ObjectModel
Imports System.Reflection
Imports System.IO
Imports System.Globalization
Imports System.ComponentModel.Design.Serialization
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase
Imports ActiveDevelop.EntitiesFormsLib

''' <summary>
''' Managed das Binden eines ViewModels (Klasse auf Basis <see cref="MvvmViewModelBase">MvvmViewModelBase</see> an eine View, 
''' die durch ein Formular oder ein UserControl repräsentiert wird. 
''' </summary>
''' <remarks>
''' <para>MVVM ist die Abkürzung von Model View ViewModel, und stellt ein Architekturmuster in der Softwaretechnik dar. Es ist 
''' State of the Art bei der Entwicklung größerer Entwicklungen für Silverlight, WPF oder Windows 8-Store-Applications (vormals: Metro-Apps) 
''' und mit den Boardmitteln von Windows Forms nicht ohne immensen Aufwand umsetzbar. Zwar kennt auch Windows Forms prinzipielles Databinding 
''' durch die Binding-Klasse, die allerdings für die Umsetzung von Anwendungen im MVVM-Stil nicht alle erforderlichen Features bereit stellt - beispielsweise 
''' Fehlen ihr eine UI-Unterstützung für das Binden von Eigenschaftenpfaden (Customer.Address.CommunicationDevices.HomePhone), WPF oder Windows-Store-Apps 
''' kompatible Converter oder das exakte Steuern von Bindungquelle und Bindungsziel (TwoWay, OneWay, OneWayToSource, OneTime).</para>
''' <para>Die Grundsätzliche Vorgehensweise beim Arbeiten mit dem MVVM-Binding-Manager ist wie folgt:</para>
''' <list type="number">
''' <item><description>Erstellen Sie die View in Form eines UserControls oder eine Windows Forms.</description></item>
''' <item><description>Erstellen Sie ein ViewModel zur Steuerung des Forms oder UserControls. Dieses ViewModel können Sie von der Basisklasse 
''' <see cref="MvvmViewModelBase">MvvmViewModelBase</see>, und Sie geben als Type (T) exakt die Klasse an, die Sie gerade erstellen. Also Beispielsweise:
''' <code>
''' Public Class AddEditTimeCollectionItemViewModel
'''     Inherits MvvmViewModelBase(Of AddEditTimeCollectionItemViewModel)
'''
''' </code>
''' Das ViewModel steuert die Eigenschaften der View durch ein entsprechendes Binding. Inkompatible Typen (Beispiel: Eine Eigenschaft vom Typ String soll an eine 
''' Eigenschaft vom Typ Boolean gebunden werden) werden dabei durch Converter-Klassen angepasst. Ein Converter lässt sich durch Einbinden des 
''' Interfaces <see cref="IValueConverter">IValueConverter</see> implementieren. Ein Beispiel dafür sieht folgender Maßen aus:
''' <code>
''' Imports System.Windows.Data
''' 
''' Public Class TimeSpanToStringConverter
'''     Implements IValueConverter
''' 
'''     Public Function Convert(value As Object, targetType As Type, parameter As Object,
'''                             culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
'''         Dim tSpan = DirectCast(value, TimeSpan)
'''         Return tSpan.ToString(parameter.ToString)
'''     End Function
''' 
'''     Public Function ConvertBack(value As Object, targetType As Type, parameter As Object,
'''                                 culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
'''         Dim timeAsString As String = DirectCast(value, String)
'''         Try
'''             Return TimeSpan.Parse(timeAsString)
'''         Catch ex As Exception
'''             Return Date.Now.TimeOfDay
'''         End Try
'''     End Function
''' End Class
''' </code>
''' </description></item>
''' <item><description>Platzieren Sie die MVVMManager-Komponente im Form oder im UserControl</description></item>
''' <item><description>WICHTIG: Erstellen Sie Ihre Projektmappe neu! (Rebuild).</description></item>
''' <item><description>Setzen Sie die <see cref="MvvmManager.DataContextType">DataContextType-Eigenschaft</see> der MvvmManager-Komponente, indem Sie 
''' im Dialog erst bestimmen, in welcher Assembly Ihr ViewModel liegt, und darunter, welche Klasse der Assembly als ViewModel zum Einsatz kommen soll.</description></item>
''' <item><description>Die MVVMManager-Komponente fungiert als so genannter Property-Extender. Jedes Steuerelement auf dem Formular "erhält" durch sie zwei 
''' weitere Eigenschaften. Mit der PropertyBinding-Eigenschaft jedes Controls rufen Sie den Dialog auf, mit dem Sie die Bindungen zwischen der ViewModel 
''' der View (Formular oder UserControl) definieren können.</description></item>
''' </list>
''' <item><description>Weisen Sie eine Instanz Ihres ViewModels zur Laufzeit an die DataContext-Eigenschaft der MVVMManager-Komponente zu. Ein geeigneter Zeitpunkt 
''' ist beispielsweise das FormLoad-Ereignis, falls es sich bei Ihrer View um ein Form-Objekt handelt.</description></item>
''' <para>Wichtige Hinweise zur Anwendung der MvvmManager-Komponente: Damit die Kommunikation zwischen ViewModel und View problemlos funktionieren kann, 
''' müssen folgende Voraussetzungen erfüllt sein:</para>
''' <list type="bullet">
''' <item><description>Nur Eigenschaften von Steuerelementen lassen sich binden, die ein entsprechendes 'EigenschaftennameChanged'-Ereignis zur 
''' Verfügung stellen. Das Implementieren einer Eigenschaft namens 'NeueFarbe' beispielsweise in einem Steuerelement ist nicht ausreichend; 
''' das Steuerelement muss auch ein entsprechendes 'NeueFarbeChanged'-Ereignis zur Verfügung stellen, das ausgelöst wird, sobald sich 'NeueFarbe' ändert.</description></item>
''' <item><description>Das ViewModel (immer abgeleitet von <see cref="MvvmViewModelBase">MvvmViewModelBase</see> - siehe oben!) muss seine 
''' Eigenschaften so zur Verfügung stellen, dass ein entsprechendes PropertyChange-Ereignis ausgelöst wird, das im ViewModel durch INotifyPropertyChanged 
''' vorgegeben wird. Das entsprechende Muster innerhalb eines ViewModels sieht beispielhaft folgender Maßen aus:
''' <code>
''' Public Property Vorname As String
'''     Get
'''         Return myVorname
'''     End Get
'''     Set(value As String)
'''         'Dieser Aufruf sorgt dafür, dass PropertyChanged mit "Vorname" 
'''         'als Parameter ausgelöst wird.
'''         MyBase.SetProperty(myVorname, value)
'''     End Set
''' End Property
''' </code>
''' </description></item>
''' </list>
''' </remarks>
<ProvideProperty("PropertyBindings", GetType(Control)),
 ProvideProperty("EventBindings", GetType(Control)),
 Designer(GetType(MvvmManagerDesigner))>
Public Class MvvmManager
    Inherits FormToBusinessClassManager
    Implements IExtenderProvider
    Implements IMvvmManager

    Public Const DEFAULT_CONTEXT_GUID As String = "{861FAFC2-3724-48CE-9BCF-D4A6F0DC5F0B}"

    Private myOldDirtyStateManagerComponentObservingEnabled As StackedValue(Of Boolean)

    ''' <summary>
    ''' Wird ausgelöst, bevor eine Wertezuweisung zwischen ViewModel und View erfolgt.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValueAssigning(sender As Object, e As ValueAssigningEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, nachdem eine Wertezuweisung zwischen ViewModel und View erfolgt ist.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ValueAssigned(sender As Object, e As ValueAssignedEventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn ein Fehler beim Wertezuweisen aufgetreten ist, und die statische Eigenschaft 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event MvvmBindingException(sender As Object, e As MvvmBindingExceptionEventArgs)

    Public Event IsDirtyChanged(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn das entsprechende Viewmodel sein PropertyChanged ausgelöst hat, und dabei ist es 
    ''' unerheblich, ob diese ViewModel-Eigeschaft zuvor gebunden wurde, oder nicht.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' Mithilfe dieses Ereignisses lassen sich auch solche Ereignisse eines ViewModels aus WindowsForms heraus bearbeiten, 
    ''' für die normalerweise keine Bindungen möglich sind. 
    ''' </remarks>
    Public Event ViewmodelPropertyChanged(sender As Object, e As PropertyChangedEventArgs)

    Private myBindingManager As BindingManager
    Private myDirtyStateManagerComponent As DirtyStateManager

    Private myDataContextType As Type
    Private myPropertyStore As New MvvmBindingItems
    Private myViewToViewmodelAssignment As ViewToViewmodelAssignments
    Private myIsDirty As Boolean

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Komponente. Verwenden Sie die Toolbox im Designer, um eine Komponente dem Formular oder UserControl hinzuzufügen.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Standardeigenschaft setzen.
        Me.CurrentContextGuid = New Guid(DEFAULT_CONTEXT_GUID)
    End Sub

    Private Function GetItemFromPropertyStore(ctrl As Control) As MvvmBindingItem
        Return myPropertyStore.GetPropertyStoreItem(ctrl)
    End Function

    Protected Friend Overridable Sub OnViewmodelPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        RaiseEvent ViewmodelPropertyChanged(sender, e)
    End Sub

    ''' <summary>
    ''' Infrastrukturfunktion. Bestimmt, welche Steuerelemente Ziel des PropertyExtenders sind.
    ''' </summary>
    ''' <param name="extendee"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shadows Function CanExtend(extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        If GetType(Control).IsAssignableFrom(extendee.GetType) Then
            Return True
        End If
        Return False
    End Function

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
    Public ReadOnly Property MvvmBindings As MvvmBindingItems
        Get
            Return myPropertyStore
        End Get
    End Property

    ''' <summary>
    ''' Ermittelt die PropertyBinding-Eigenschaft für das entsprechende Steuerelement.
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DisplayName("PropertyBindings"),
     Category("MVVM"), Editor(GetType(PropertyBindingsUITypeEditor), GetType(UITypeEditor)),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Function GetPropertyBindings(ctrl As Control) As PropertyBindings Implements IMvvmManager.GetPropertyBindings
        Dim propBagItem = myPropertyStore.GetPropertyStoreItem(ctrl)
        If propBagItem.PropertyBindings Is Nothing Then
            propBagItem.PropertyBindings = New PropertyBindings
        End If
        Return propBagItem.PropertyBindings
    End Function

    ''' <summary>
    ''' Setzt die PropertyBinding-Eigenschaft für das entsprechende Steuerelement.
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub SetPropertyBindings(ctrl As Control, value As PropertyBindings)
        Dim propBagItem = myPropertyStore.GetPropertyStoreItem(ctrl)
        propBagItem.PropertyBindings = value
    End Sub

    ''' <summary>
    ''' Ermittelt die EventBindings-Eigenschaft für das entsprechende Steuerelement.
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DisplayName("EventBindings"),
     Category("MVVM"), Editor(GetType(EventBindingsUITypeEditor), GetType(UITypeEditor))>
    Public Function GetEventBindings(ctrl As Control) As ObservableBindingList(Of EventBindingItem)
        If myPropertyStore.Contains(ctrl) Then
            Return myPropertyStore(ctrl).Data.EventBindings
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Setzt die EventBindings-Eigenschaft für das entsprechende Steuerelement.
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub SetEventBindings(ctrl As Control, value As ObservableBindingList(Of EventBindingItem))
        If myPropertyStore.Contains(ctrl) Then
            myPropertyStore(ctrl).Data.EventBindings = value
        Else
            myPropertyStore.Add(New ExtenderProviderPropertyStoreItem(Of MvvmBindingItem) With {
                                .Control = ctrl,
                                .Data = New MvvmBindingItem With
                                {.EventBindings = value}})
        End If
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, welchen Typ ViewModel die MVVMManager-Komponente später zur Laufzeit verarbeiten soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Diese Eigenschaft ist wichtig zu setzen, BEVOR Zuweisungen über die PropertyBindings- bzw. EventBindings-Eigenschaften 
    ''' der entsprechenden Steuerelemente in der View gemacht werden können. Anhand dieser Eigenschaft erstellt die UI 
    ''' dieser Komponente eine Liste der bindbaren Eigenschaften und Ereignisse des ViewModels, und bietet diese im Dialog an, der 
    ''' aufgerufen wird, wenn im Eigenschaftenfenster auf die ...-Schaltfläche für die PropertyBindings-Eigenschaft bzw. 
    ''' der EventBindings-Eigenschaft geklickt wird.</remarks>
    <Editor(GetType(DataSourceTypeUIEditor), GetType(UITypeEditor)), Category("MVVM"),
    Description("Bestimmt oder ermittelt, welchen Typ ViewModel die MVVMManager-Komponente später zur Laufzeit verarbeiten soll.")>
    Property DataContextType As Type Implements IMvvmManager.DataContextType
        Get
            Return MyBase.DataSourceType
        End Get
        Set(value As Type)
            MyBase.DataSourceType = value
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt das ViewModel für die Binding an die View. Diese Eigenschaft sollte zur Laufzeit gesetzt werden, und erst nachdem 
    ''' die DataContextType-Eigenschaft zur Entwurfszeit gesetzt wurde und die Zuweisungen ViewModel/View über die PropertyBinding- und 
    ''' EventBinding-Eigenschaften der Steuerelemente der View erfolgt sind.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Public Property DataContext As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(value As Object)
            If Me.DesignMode Then
                Return
            End If

            If Not Object.Equals(value, MyBase.DataSource) Then

                If MyBase.DataSource IsNot Nothing Then
                    UnwireViewmodelEvents(MyBase.DataSource)
                End If

                If Me.DirtyStateManagerComponent IsNot Nothing Then
                    Me.DirtyStateManagerComponent.ResetIsDirty()
                    Me.DirtyStateManagerComponent.ObservingEnabled = False
                End If
                MyBase.DataSource = value

                If value IsNot Nothing Then

                    'We consider just those elements for which there are actual bindings.
                    Dim list = From propItem In Me.myPropertyStore
                               Where propItem.Data.PropertyBindings IsNot Nothing
                               Select New BindingItem With {.Control = propItem.Control,
                                                            .MvvmItem = propItem.Data}
                    If myBindingManager IsNot Nothing Then
                        'Hiermit werden auch alle Events entbunden.
                        Windows.WeakEventManager(Of BindingManager, MvvmBindingExceptionEventArgs).RemoveHandler(
                            myBindingManager, "MvvmBindingException", AddressOf myBindingManager_MvvmBindingException)

                        Windows.WeakEventManager(Of BindingManager, ValueAssigningEventArgs).RemoveHandler(
                            myBindingManager, "ValueAssigning", AddressOf myBindingManager_ValueAssigning)

                        Windows.WeakEventManager(Of BindingManager, ValueAssignedEventArgs).RemoveHandler(
                            myBindingManager, "ValueAssigned", AddressOf myBindingManager_ValueAssigned)

                        myBindingManager.Dispose()

                    End If

                    myBindingManager = New BindingManager(DirectCast(MyBase.DataSource, INotifyPropertyChanged),
                                                          New BindingItems(list), Me, updateControlsAfterInstatiating:=False)

                    Windows.WeakEventManager(Of BindingManager, MvvmBindingExceptionEventArgs).AddHandler(
                        myBindingManager, "MvvmBindingException", AddressOf myBindingManager_MvvmBindingException)

                    Windows.WeakEventManager(Of BindingManager, ValueAssigningEventArgs).AddHandler(
                        myBindingManager, "ValueAssigning", AddressOf myBindingManager_ValueAssigning)

                    Windows.WeakEventManager(Of BindingManager, ValueAssignedEventArgs).AddHandler(
                        myBindingManager, "ValueAssigned", AddressOf myBindingManager_ValueAssigned)

                    'Das hier müssen wir initial machen und zwar nicht schon durch den Konstruktor des BindingManagers,
                    'da anderenfalls die als WithEvent gebundenen Ereignisse, die durch UpdateAllControlsFromViewModel
                    'ausgelöst würden, nicht hier oben ankommen würden! (Siehe auch letzter optionaler Parameter im
                    'BindingManager-Konstruktor.
                    Dim suspendBindingIfInterfaceImplemented = TryCast(value, IMvvmViewModelNotifyBindingProcess)
                    If suspendBindingIfInterfaceImplemented IsNot Nothing Then
                        suspendBindingIfInterfaceImplemented.BeginBinding()
                    End If

                    myBindingManager.UpdateControlsFromViewModel()
                    myBindingManager.GetAllErrorsFromViewmodel()

                    If suspendBindingIfInterfaceImplemented IsNot Nothing Then
                        suspendBindingIfInterfaceImplemented.EndBinding()
                    End If
                    WireViewmodelEvents(value)
                Else
                    myBindingManager.UpdateControlsWithNothing()

                    Windows.WeakEventManager(Of BindingManager, MvvmBindingExceptionEventArgs).RemoveHandler(
                        myBindingManager, "MvvmBindingException", AddressOf myBindingManager_MvvmBindingException)

                    Windows.WeakEventManager(Of BindingManager, ValueAssigningEventArgs).RemoveHandler(
                        myBindingManager, "ValueAssigning", AddressOf myBindingManager_ValueAssigning)

                    Windows.WeakEventManager(Of BindingManager, ValueAssignedEventArgs).RemoveHandler(
                        myBindingManager, "ValueAssigned", AddressOf myBindingManager_ValueAssigned)
                    myBindingManager.Dispose()

                    myBindingManager = Nothing
                End If
            End If

            If Me.DirtyStateManagerComponent IsNot Nothing Then
                Me.DirtyStateManagerComponent.ObservingEnabled = True
            End If

        End Set
    End Property

    ''' <summary>
    ''' Aktualisiert das ViewModel auf Basis des aktuellen Zustands der View.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateViewModel()
        If myBindingManager IsNot Nothing Then
            myBindingManager.UpdateViewModelFromControls()
        End If
    End Sub

    ''' <summary>
    ''' Aktualisiert die Anzeige der Daten in der View auf Basis des aktuellen Zustands des ViewModels.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateView()
        If myBindingManager IsNot Nothing Then
            myBindingManager.UpdateControlsFromViewModel()
            myBindingManager.GetAllErrorsFromViewmodel()
        End If
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt die verwendete DirtyStateManager-Komponente für das Überwachen des IsDirty-Status einer View (Formular, UserControl)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' <para>Wenn innerhalb einer View die DirtyStateManager-Komponente verwendet wird, hat die View die Möglichkeit, darüber informiert zu werden,
    ''' ob der Anwender innerhalb der View Änderungen vorgenommen hat, damit sie beispielsweise einen Speichern-Button aktivieren kann. Zu diesem 
    ''' Zweck braucht es innerhalb der View neben dem MvvmManager auch eine DirtyStateManager-Komponente, die sämtlichen Steuerelementen in der 
    ''' View als Property Extender die Eigenschaften 'CausesIsDirtyChanged' sowie 'IsDirtyChangedCausingEvent' verleiht. Mit diesem Eigenschaften 
    ''' kann gesteuert werden, ob ein Steuerelement einen IsDirty-Zustand (eine Eigenschaft in der View wurde verändert) herstellen kann, 
    ''' und welches Ereignis des Steuerelementes (beispielsweise TextChange bei einer TextBox) das erreichen soll (IsDirtyChangedCausingEvent-Eigenschaft).</para> 
    ''' <para>Da der DirtyStateManager die Quelle der Eigenschaftenveränderungen (Benutzer, Programm) nicht unterscheiden kann, muss diese Rolle der 
    ''' MvvmMananger übernehmen - deswegen muss die DirtyStateManager-Komponente der MvvmManager-Komponente zugewiesen werden. Bevor der MvvmManager 
    ''' eine Eigenschaft in der View durch Updates im ViewModel verändert, schaltet er die DirtyStateManager-Komponente über ihre Enabled-Eigenschaft aus  
    ''' und direkt danach wieder an. Im Ergebnis löst der DirtyStateManager nur noch dann ein IsDirtyChanged-Ereignis aus, wenn der Anwender selbst in der View 
    ''' eine Änderung vorgenommen hat, da der DirtyStateManager nicht aktiv ist, wenn über das ViewModel Änderungen an der View vorgenommen wurden. 
    ''' Sollte der Entwickler selbst Änderungen an Eigenschaften von Steuerelementen der View vornehmen, muss er zuvor selbst die 
    ''' Enabled-Eigenschaft der DirtyStateManager-Komponent deaktivieren, um das gewünschte Ergebnis nicht zu verfälschen.</para>
    ''' </remarks>
    Public Property DirtyStateManagerComponent As DirtyStateManager
        Get
            Return myDirtyStateManagerComponent
        End Get

        Set(value As DirtyStateManager)
            If Not Object.Equals(myDirtyStateManagerComponent, value) Then
                If myDirtyStateManagerComponent IsNot Nothing Then
                    Windows.WeakEventManager(Of DirtyStateManager, IsDirtyChangedEventArgs).RemoveHandler(
                        myDirtyStateManagerComponent, "IsDirtyChanged", AddressOf myDirtyStateManagerComponent_IsDirtyChanged)
                End If

                myDirtyStateManagerComponent = value

                If myDirtyStateManagerComponent IsNot Nothing Then
                    Windows.WeakEventManager(Of DirtyStateManager, IsDirtyChangedEventArgs).AddHandler(
                        myDirtyStateManagerComponent, "IsDirtyChanged", AddressOf myDirtyStateManagerComponent_IsDirtyChanged)
                End If
            End If
        End Set
    End Property

    Private Sub WireViewmodelEvents(ds As Object)
        Dim datasource = TryCast(ds, IMvvmViewModel)
        If datasource Is Nothing Then Return
        'Hier werden die Events zum anfordern von Dialogen weak verdrahtet (weak damit die View entsorgt werden auch wenn das VM noch da ist, das Form aber geschlossen wurde)
        Windows.WeakEventManager(Of IMvvmViewModel, RequestMessageDialogEventArgs).AddHandler(
                        datasource, "RequestMessageDialog", AddressOf RequestMessageDialogEventProc)

        Windows.WeakEventManager(Of IMvvmViewModel, RequestViewEventArgs).AddHandler(
                        datasource, "RequestModalView", AddressOf RequestModalView)

        Windows.WeakEventManager(Of IMvvmViewModel, RequestViewEventArgs).AddHandler(
                        datasource, "RequestView", AddressOf RequestModalView)
    End Sub

    Private Sub UnwireViewmodelEvents(ds As Object)
        Dim datasource = TryCast(ds, IMvvmViewModel)
        If datasource Is Nothing Then Return

        Windows.WeakEventManager(Of IMvvmViewModel, RequestMessageDialogEventArgs).RemoveHandler(
                        datasource, "RequestMessageDialog", AddressOf RequestMessageDialogEventProc)

        Windows.WeakEventManager(Of IMvvmViewModel, RequestViewEventArgs).RemoveHandler(
                        datasource, "RequestModalView", AddressOf RequestModalView)

        Windows.WeakEventManager(Of IMvvmViewModel, RequestViewEventArgs).RemoveHandler(
                        datasource, "RequestView", AddressOf RequestModalView)
    End Sub

    Private Sub RequestMessageDialogEventProc(sender As Object, e As RequestMessageDialogEventArgs)
        Dim mButtons As MessageBoxButtons
        Select Case e.MessageBoxEventButtons
            Case MvvMessageBoxEventButtons.OK : mButtons = MessageBoxButtons.OK
            Case MvvMessageBoxEventButtons.OKCancel : mButtons = MessageBoxButtons.OKCancel
            Case MvvMessageBoxEventButtons.YesNo : mButtons = MessageBoxButtons.YesNo
            Case MvvMessageBoxEventButtons.YesNoCancel : mButtons = MessageBoxButtons.YesNoCancel
        End Select

        Dim mIcons As MessageBoxIcon
        Select Case e.MessageBoxIcon
            Case MvvmMessageBoxIcon.Error : mIcons = MessageBoxIcon.Error
            Case MvvmMessageBoxIcon.Information : mIcons = MessageBoxIcon.Information
            Case MvvmMessageBoxIcon.None : mIcons = MessageBoxIcon.None
            Case MvvmMessageBoxIcon.Stop : mIcons = MessageBoxIcon.Stop
            Case MvvmMessageBoxIcon.Warning : mIcons = MessageBoxIcon.Warning
        End Select

        Dim mDefaultButton As MessageBoxDefaultButton
        Select Case e.MessageBoxDefaultButton
            Case MvvmMessageBoxDefaultButton.Button1 : mDefaultButton = MessageBoxDefaultButton.Button1
            Case MvvmMessageBoxDefaultButton.Button2 : mDefaultButton = MessageBoxDefaultButton.Button2
            Case MvvmMessageBoxDefaultButton.Button3 : mDefaultButton = MessageBoxDefaultButton.Button3
        End Select

        Dim rValue = MessageBox.Show(e.MessageBoxText, e.MessageBoxTitle, mButtons, mIcons, mDefaultButton)

        Select Case rValue
            Case DialogResult.OK : e.MessageBoxReturnValue = MvvmMessageBoxReturnValue.OK
            Case DialogResult.Cancel : e.MessageBoxReturnValue = MvvmMessageBoxReturnValue.Cancel
            Case DialogResult.Yes : e.MessageBoxReturnValue = MvvmMessageBoxReturnValue.Yes
            Case DialogResult.No : e.MessageBoxReturnValue = MvvmMessageBoxReturnValue.No
        End Select
    End Sub

    Private Sub RequestModalView(sender As Object, e As RequestViewEventArgs)
        Dim modalableViewModel = DirectCast(e.ViewModel, IMvvmViewModelForModalDialog)
        If modalableViewModel IsNot Nothing Then
            Dim winFormsForm = GetViewFromViewModel(modalableViewModel)
            DirectCast(winFormsForm, IWinFormsMvvmView).GetMvvmController.DataContext = modalableViewModel

            Dim rValue = winFormsForm.ShowDialog()

            Select Case rValue
                Case DialogResult.OK : e.DialogResult = MvvmMessageBoxReturnValue.OK
                Case DialogResult.Cancel : e.DialogResult = MvvmMessageBoxReturnValue.Cancel
                Case DialogResult.Yes : e.DialogResult = MvvmMessageBoxReturnValue.Yes
                Case DialogResult.No : e.DialogResult = MvvmMessageBoxReturnValue.No
            End Select
        End If

    End Sub

    Private Sub RequestView(sender As Object, e As RequestViewEventArgs)
        Dim modalableViewModel = DirectCast(sender, IMvvmViewModelForModalDialog)
        If modalableViewModel IsNot Nothing Then
            Dim winFormsForm = GetViewFromViewModel(modalableViewModel)
            DirectCast(winFormsForm, IWinFormsMvvmView).GetMvvmController.DataContext = modalableViewModel

            winFormsForm.Show()
        End If
    End Sub

    Private Function GetViewFromViewModel(viewModel As IMvvmViewModel) As Form

        If viewModel Is Nothing Then
            Throw New ArgumentException("The viewmodel must not be null (nothing in VB) to retrieve its view. Please, pass in a valid instance implementing IMvvmViewModel. Thanks.")
        End If

        Dim attItems = (From attItem In viewModel.GetType.GetCustomAttributes(True)
                        Where GetType(MvvmViewAttribute).IsAssignableFrom(attItem.GetType) AndAlso
                         DirectCast(attItem, MvvmViewAttribute).ContextGuid = Me.CurrentContextGuid
                        Select DirectCast(attItem, MvvmViewAttribute)).ToList

        If attItems IsNot Nothing Then
            If attItems.Count > 0 And attItems.Count < 2 Then
                'An dieser Stelle dürfte nur noch ein Item übrig bleiben, sonst wurden doppelte ContextGuids vergeben!

                Dim attItem = attItems(0)
                'TODO: Feststellen, ob es ein UserControl ist, und im Bedarfsfall ein Wrapper-Form zurückliefern.
                Dim formType = Type.GetType(attItem.ViewTypeName & ", " & attItem.Assemblyname)
                Dim formToReturn = DirectCast(Activator.CreateInstance(formType), Form)
                Return formToReturn
            Else
                If attItems.Count > 1 Then
                    Throw New ArgumentException("More than one View-ViewModel assignment for one ContextGuid has been detected. Please check the MvvmViewAttribute assignments for your ViewModel Classes.")
                End If
            End If
        Else
            Throw New ArgumentException("The View for the ViewModel of type " & viewModel.GetType.ToString & " could not be found.")
        End If

        Throw New ArgumentException("GetViewFromViewModel could not retrieve a proper result.")

    End Function

    ''' <summary>
    ''' Für zukünftige Erweiterungen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Property CurrentContextGuid As Guid

    ''' <summary>
    ''' Infrastrukturfunktion. Verwenden Sie stattdessen bitte die DataContext-Eigenschaft.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property DataSource As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(value As Object)
            MyBase.DataSource = value
        End Set
    End Property

    ''' <summary>
    ''' Infrastrukturfunktion. Verwenden Sie stattdessen bitte die DataContextType-Eigenschaft.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
    Public Overrides Property DataSourceType As Type
        Get
            Return MyBase.DataSourceType
        End Get
        Set(value As Type)
            MyBase.DataSourceType = value
        End Set
    End Property

    Private Sub myDirtyStateManagerComponent_IsDirtyChanged(sender As Object, e As IsDirtyChangedEventArgs)
        MyBase.myIsFormDirty = myDirtyStateManagerComponent.IsDirty
        OnIsFormDirtyChanged(New IsFormDirtyChangedEventArgs(e.CausingControl))
    End Sub

    Private Sub myBindingManager_MvvmBindingException(bindingManager As Object, e As MvvmBindingExceptionEventArgs)
        OnMvvmBindingException(e)
    End Sub

    ''' <summary>
    ''' Löst das <see cref="MvvmBindingException">MvvmBindingException-Ereignis aus.</see>
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnMvvmBindingException(e As MvvmBindingExceptionEventArgs)
        RaiseEvent MvvmBindingException(GetType(MvvmManager), e)
    End Sub

    Private Sub myBindingManager_ValueAssigning(sender As Object, e As ValueAssigningEventArgs)
        If Me.DirtyStateManagerComponent IsNot Nothing Then
            Debug.Print("TRACE CALL: myBindingManager_ValueAssigning. DS_Manager-State:" & DirtyStateManagerComponent.ObservingEnabled &
                        " Sender: " & sender.ToString)
            If e.Target = Targets.Control Then
                myOldDirtyStateManagerComponentObservingEnabled.Value = DirtyStateManagerComponent.ObservingEnabled
                Me.DirtyStateManagerComponent.ObservingEnabled = False
            End If
        Else
            Debug.Print("TRACE CALL: myBindingManager_ValueAssigning. DS_Manager-State: null.")
        End If
        OnValueAssigning(e)
    End Sub

    ''' <summary>
    ''' Löst das <see cref="ValueAssigning">ValueAssigning-Ereignis aus.</see>
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValueAssigning(e As ValueAssigningEventArgs)
        RaiseEvent ValueAssigning(Me, e)
    End Sub

    Private Sub myBindingManager_ValueAssigned(sender As Object, e As ValueAssignedEventArgs)
        If Me.DirtyStateManagerComponent IsNot Nothing Then
            Debug.Print("TRACE CALL: myBindingManager_ValueAssigned. DS_Manager-State:" & DirtyStateManagerComponent.ObservingEnabled &
                        " Sender: " & sender.ToString)
            If e.Target = Targets.Control Then
                DirtyStateManagerComponent.ObservingEnabled = myOldDirtyStateManagerComponentObservingEnabled.Value
            End If
        Else
            Debug.Print("TRACE CALL: myBindingManager_ValueAssigned. DS_Manager-State: null.")
        End If
        OnValueAssigned(e)
    End Sub

    ''' <summary>
    ''' Löst das <see cref="ValueAssigned">ValueAssigned-Ereignis aus.</see>
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnValueAssigned(e As ValueAssignedEventArgs)
        RaiseEvent ValueAssigned(Me, e)
    End Sub
End Class

Public Class MvvmPropertyItemTypeConverter
    Inherits TypeConverter

    ' Overrides the CanConvertFrom method of TypeConverter.
    ' The ITypeDescriptorContext interface provides the context for the
    ' conversion. Typically, this interface is used at design time to 
    ' provide information about the design-time container.
    Public Overloads Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If sourceType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    ' Overrides the ConvertFrom method of TypeConverter.
    Public Overloads Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
        If TypeOf value Is String Then
            Dim sr As New StringReader(value.ToString)
            Try
                Return MvvmBindingItem.FromXmlStream(sr)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                sr.Close()
            End Try
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    ' Overrides the ConvertTo method of TypeConverter.
    Public Overloads Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) Then
            Dim sw As New StringWriter()
            DirectCast(value, MvvmBindingItem).Serialize(sw)
            sw.Flush()
            sw.Close()
            Return sw.ToString
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    Public Sub BeginInit()
        DirectCast(Me, ISupportInitialize).BeginInit()
    End Sub

    Public Sub EndInit()
        DirectCast(Me, ISupportInitialize).EndInit()
    End Sub

End Class

''' <summary>
''' Interface welches die benoetigte Funktionalitaet fuer das 
''' frmMvvmPropertyAssignment-Formular zum Bearbeiten der Bindungen kapselt
''' </summary>
''' <remarks></remarks>
Public Interface IMvvmManager

    ''' <summary>
    ''' Liefert alle Bindungen des Controls
    ''' </summary>
    ''' <param name="ctrl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetPropertyBindings(ctrl As Control) As PropertyBindings

    ''' <summary>
    ''' Datentyp des ViewModels, welches als Bindungsquelle verwendet werden soll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property DataContextType As Type
End Interface