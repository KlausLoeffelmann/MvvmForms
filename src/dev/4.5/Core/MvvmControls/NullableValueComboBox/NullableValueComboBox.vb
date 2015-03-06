Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Threading
Imports ActiveDevelop.EntitiesFormsLib
Imports System.ComponentModel
Imports System.Windows.Forms

''' <summary>
''' Bindungsfähige ComboBox 
''' </summary>
''' <remarks>Basiert von inneren auf eine WPF-CBO mit UI-Elementen aus der EFL</remarks>
Public Class NullableValueComboBox
    Implements INullableValueDataBinding

    Private _defaultColor As Media.Brush
    Private _focusedColor As Media.Brush

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der Wert im Steuerelement geändert hat, um einen einbindenden Formular oder 
    ''' User Control die Möglichkeit zu geben, den Benutzer zu informieren, dass er Änderungen speichern muss.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs) Implements INullableValueDataBinding.IsDirtyChanged

    ''' <summary>
    ''' Wird ausgelöst, nachdem sich der Wert der Value-Eigenschaft geändert hat.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter vom Typ ValueChangedEventArgs, mit denen der Grund für die Werteänderung ermittelt werden kann.</param>
    ''' <remarks></remarks>
    Public Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs) Implements INullableValueDataBinding.ValueChanged

    'Der ist nur zur Interface-Richtlinien-Erfüllung da und wird hier nie benutzt, da es keinen Zustand gibt,
    'bei dem dieses Steuerelement den Focus behalten könnte.
    'TODO: Überprüfen, ob es nicht doch Fälle gibt, bei dem der Fokus erzwungener Maßen erhalten bleiben soll.
    Public Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs) Implements INullableValueDataBinding.RequestValidationFailedReaction

    ''' <summary>
    ''' Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Wenn ein bindbares Feld in einem Tabellenschema beispielsweise den Namen ZipCity trägt, könnte 
    ''' diese Eigenschaft "Postleitzahl/Ort" lauten. Diese Eigenschaft spiegelt also die Bezeichnung des 
    ''' Datenfeldnamens in echter Sprache wider, sodass sie verständlich für den Endanwender wird. Bei automatisierter 
    ''' Validierung einer Eingabemaske kann dann in der UI mit diesen Namen Bezug auf das entsprechende 
    ''' Datenfeld genommen werden. Die Bezeichnung einer Datenfeldes in der UI beispielsweise durch ein Label sollte 
    ''' daher genauso lauten, wie diese Eigenschaft. Diese Eigenschaft sollte bei Verwendung von automatischer 
    ''' Validierung auch auf jedenfall gesetzt sein, weil anderenfalls eine Ausnahme ausgelöst werden kann.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den ausgeschriebenen/lolkalisierten Namen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DatafieldDescription As String Implements INullableValueDataBinding.DatafieldDescription

    Public Function ShouldSerializeDatafieldDescription() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldDescription)
    End Function

    Public Sub ResetDatafieldDescription()
        DatafieldDescription = Nothing
    End Sub

    Public Property NullValueMessage As String Implements INullableValueDataBinding.NullValueMessage
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
            'TODO: noch implementieren
        End Set
    End Property


    Private myDataFieldname As String

    ''' <summary>
    ''' Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den Datenquellen-Feldnamen des Feldes, mit dem dieses Steuerelement verknüpft werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property DatafieldName As String Implements INullableValueDataBinding.DatafieldName
        Get
            Return myDataFieldname
        End Get
        Set(ByVal value As String)
            If value <> myDataFieldname Then
                myDataFieldname = value
            End If
        End Set
    End Property

    Public Function ShouldSerializeDatafieldName() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldName)
    End Function

    Public Sub ResetDatafieldName()
        DatafieldName = Nothing
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        'Farben
        _defaultColor = Me.WpfComboBoxWrapper1.InnerComboBox.Background
        _focusedColor = New SolidColorBrush(Colors.Yellow)

        AddHandler Me.WpfComboBoxWrapper1.InnerComboBox.SelectionChanged, AddressOf InnerComboBox_SelectionChanged
    End Sub

    Private _isItemsSourceSetting As Boolean = False

    ''' <summary>
    ''' DataSource der CBO
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property ItemSource As IEnumerable
        Get
            Return Me.WpfComboBoxWrapper1.InnerComboBox.ItemsSource
        End Get
        Set(ByVal value As IEnumerable)
            If Not Object.Equals(Me.WpfComboBoxWrapper1.InnerComboBox.ItemsSource, value) Then
                Try
                    _isItemsSourceSetting = True
                    Me.WpfComboBoxWrapper1.InnerComboBox.ItemsSource = value
                Finally
                    _isItemsSourceSetting = False
                End Try
                OnItemSourceChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event ItemSourceChanged As EventHandler

    Protected Overridable Sub OnItemSourceChanged(e As EventArgs)
        RaiseEvent ItemSourceChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Aktuell selektierte Item
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property SelectedItem As Object Implements INullableValueDataBinding.Value
        Get
            Return Me.WpfComboBoxWrapper1.InnerComboBox.SelectedItem
        End Get
        Set(ByVal value As Object)
            If Not Object.Equals(Me.WpfComboBoxWrapper1.InnerComboBox.SelectedItem, value) Then

                Dim ds = TryCast(Me.ItemSource, IList)

                If ds IsNot Nothing AndAlso ds.Count > 0 AndAlso ((value Is Nothing) OrElse (Not ds.Contains(value))) Then
                    'Wenn neuer Wert null ist und eine DataSource gesetzt ist und diese Werte hat
                    'immer den ersten auswählen
                    Me.WpfComboBoxWrapper1.InnerComboBox.SelectedItem = ds(0)
                Else
                    'Normales Basisverhalten
                    Me.WpfComboBoxWrapper1.InnerComboBox.SelectedItem = value
                End If

                OnSelectedItemChanged(EventArgs.Empty)
                Me.IsDirty = True
            End If
        End Set
    End Property

    Public Event SelectedItemChanged As EventHandler

    Protected Overridable Sub OnSelectedItemChanged(e As EventArgs)
        RaiseEvent SelectedItemChanged(Me, e)
    End Sub



    Private Sub InnerComboBox_SelectionChanged(sender As Object, e As Windows.Controls.SelectionChangedEventArgs)

        If Me.SelectedItem IsNot Nothing Then
            OnSelectedItemChanged(e)
        ElseIf Not (Me.ValueNotFoundBehavior = ValueNotFoundBehavior.KeepFocus OrElse _
                      Me.ValueNotFoundBehavior = ValueNotFoundBehavior.SelectFirst) Then
            'Wenn SelctedItem Nothing ist und ein unbestimmter Wert nicht angegeben werden darf, darf das PropChanged nicht geworfen werden (da der Benutzer gezwungen wird ein validen Wert später beim 
            'Leave auszuwählen bzw automatisch ausgewählt)
            OnSelectedItemChanged(e)
        End If

    End Sub

    Private _leaveBehavior As ValueNotFoundBehavior = EntitiesFormsLib.ValueNotFoundBehavior.KeepFocus

    ''' <summary>
    ''' Verhalten was passieren soll, wenn ein Wert eingegben wurde, welcher nicht in der DataSource nachgeschlagen werden kann
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property ValueNotFoundBehavior As ValueNotFoundBehavior
        Get
            Return _leaveBehavior
        End Get
        Set(ByVal value As ValueNotFoundBehavior)
            If Not Object.Equals(_leaveBehavior, value) Then
                _leaveBehavior = value
                OnValueNotFoundBehaviorChanged(EventArgs.Empty)

                Select Case value
                    Case ValueNotFoundBehavior.KeepFocus
                        Me.WpfComboBoxWrapper1.InnerComboBox.IsEditable = True

                    Case ValueNotFoundBehavior.IsNotEditable
                        Me.WpfComboBoxWrapper1.InnerComboBox.IsEditable = False

                    Case ValueNotFoundBehavior.SelectFirst
                        Me.WpfComboBoxWrapper1.InnerComboBox.IsEditable = True

                End Select

            End If
        End Set
    End Property

    Public Event ValueNotFoundBehaviorChanged As EventHandler

    Protected Overridable Sub OnValueNotFoundBehaviorChanged(e As EventArgs)
        RaiseEvent ValueNotFoundBehaviorChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Pfad zur Property für die Anzeige
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Durch den MvvmManager bindbare View-Property</remarks>
    Public Property DisplayMemberPath As String
        Get
            Return Me.WpfComboBoxWrapper1.InnerComboBox.DisplayMemberPath
        End Get
        Set(ByVal value As String)
            If Not Object.Equals(Me.WpfComboBoxWrapper1.InnerComboBox.DisplayMemberPath, value) Then
                Me.WpfComboBoxWrapper1.InnerComboBox.DisplayMemberPath = value
                OnDisplayMemberPathChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Public Event DisplayMemberPathChanged As EventHandler

    Protected Overridable Sub OnDisplayMemberPathChanged(e As EventArgs)
        RaiseEvent DisplayMemberPathChanged(Me, e)
    End Sub


    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)

        'Nachschauen was passieren soll, wenn ein Wert eingegeben wurde, der nicht vorhanden ist (oder Nothing)
        If Me.WpfComboBoxWrapper1.InnerComboBox.SelectedItem Is Nothing _
            AndAlso Me.WpfComboBoxWrapper1.InnerComboBox.SelectedValue Is Nothing Then
            'Wert konnte nicht gefunden werden

            If Me.ValueNotFoundBehavior = ValueNotFoundBehavior.KeepFocus Then
                'Control darf nicht verlassen werden

                Me.WpfComboBoxWrapper1.InnerComboBox.Dispatcher.BeginInvoke(DispatcherPriority.Input, New Action(AddressOf ResetFocus))
            ElseIf Me.ValueNotFoundBehavior = ValueNotFoundBehavior.SelectFirst Then
                'Ersten auswählen
                If Me.ItemSource IsNot Nothing Then
                    Try
                        Me.SelectedItem = Me.ItemSource(0)
                    Catch ex As IndexOutOfRangeException
                        Throw New InvalidOperationException("Erste Eintrag kann nicht ausgewählt werden weil momentan keine Einträge in der ItemsSource vorhanden sind!")
                    End Try
                End If
            End If
        End If

    End Sub

    Private Sub ResetFocus()
        Me.WpfComboBoxWrapper1.InnerComboBox.Focus()
    End Sub

    Private myIsDirty As Boolean

    <Browsable(False)>
    Public Property IsDirty As Boolean Implements INullableValueDataBinding.IsDirty
        Get
            Return myIsDirty
        End Get
        Private Set(value As Boolean)
            If value <> myIsDirty Then
                myIsDirty = value
                OnIsDirtyChanged(New IsDirtyChangedEventArgs(Me))
            End If
        End Set
    End Property
    Protected Overridable Sub OnIsDirtyChanged(e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Maske, die das Steuerelement enthält, gerade die Steuerelemente mit Daten befüllt, oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Das Setzen dieser Eigenschaft kann nur über die Schnittstelle (in der Regel von der Maskensteuerung) vorgenommen werden.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property IsLoading As HistoricalBoolean Implements INullableValueDataBinding.IsLoading
        Get
            Return New HistoricalBoolean() With {.Value = _isItemsSourceSetting}
        End Get
        Set(ByVal value As HistoricalBoolean)
            'TODO: Was soll ich denn hier setzen? (Später vielleicht wenn die Lisete bei EIngaeb keliner werden soll...)
        End Set
    End Property

    Public Sub ResetIsDirty() Implements INullableValueDataBinding.ResetIsDirty
        myIsDirty = False
    End Sub

    Private myGroupName As String = NullableControlManager.GetInstance.GetDefaultGroupName(Me, "Default")

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

    Protected Overrides Sub [Select](directed As Boolean, forward As Boolean)
        MyBase.[Select](directed, forward)

        Me.WpfComboBoxWrapper1.Focus()
        Me.WpfComboBoxWrapper1.InnerComboBox.Focus()
    End Sub

    Public Property AssignedManagerControl As FormToBusinessClassManager Implements IAssignableFormToBusinessClassManager.AssignedManagerControl

    Public Property ProcessingPriority As Integer Implements IAssignableFormToBusinessClassManager.ProcessingPriority

    Public Property SelectedForProcessing As Boolean Implements IAssignableFormToBusinessClassManager.SelectedForProcessing

    Public Property IsKeyField As Boolean Implements IKeyFieldProvider.IsKeyField

    Public Property ExceptionBalloonDuration As Integer Implements INullableValueControl.ExceptionBalloonDuration

End Class

''' <summary>
''' Verhalten-Flag zur Steuerung des BindableComboBox
''' </summary>
''' <remarks></remarks>
Public Enum ValueNotFoundBehavior
    ''' <summary>
    ''' Verbieten das Verlassen des Controls solange ein Wert enthalten ist, welcher nicht nachgeschlagen werden konnte
    ''' </summary>
    ''' <remarks></remarks>
    KeepFocus

    ''' <summary>
    ''' Wählt immer den ersten Wert im Controls aus, solange etwas eingegeben wurde, was nicht in der ItemsSource enthalten ist
    ''' </summary>
    ''' <remarks></remarks>
    SelectFirst

    ''' <summary>
    ''' Control verbietet die Eingabe von anderen Werten
    ''' </summary>
    ''' <remarks></remarks>
    IsNotEditable

    ''' <summary>
    ''' Erlaubt die Eingabe eines Wertes welcher nicht in der ItemsSource nachgeschlagen werden konnte
    ''' </summary>
    ''' <remarks></remarks>
    PreserveInput
End Enum