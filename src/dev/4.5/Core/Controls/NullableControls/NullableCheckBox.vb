Imports System.Windows.Forms
Imports System.ComponentModel

''' <summary>
''' CheckBox-Steuerelement, das Null-Werte verarbeitet, eine vereinheitlichende Value-Eigenschaft bietet, 
''' Funktionen für Rechteverwaltung zur Verfügung stellt und von einer 
''' <see cref="FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet werden kann.
''' </summary>
''' <remarks></remarks>
Public Class NullableCheckBox
    Inherits CheckBox
    Implements INullableValueDataBinding, IPermissionManageableUIControlElement, IPermissionManageableUIContentElement

    Private myGroupName As String = "Default"
    Private myIsDirty As Boolean
    Private myIsLoading As HistoricalBoolean
    Dim myValueChangedByPropertySetter As Boolean

    Public Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs) Implements INullableValueDataBinding.IsDirtyChanged
    Public Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs) Implements INullableValueDataBinding.ValueChanged

    Public Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs) Implements INullableValueControl.RequestValidationFailedReaction

    Sub New()
        MyBase.New()
        Me.UIGuid = Guid.NewGuid
        Me.ExceptionBalloonDuration = 5000
    End Sub


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

    Public Function ShouldSerializeDatafieldName() As Boolean
        Return Not String.IsNullOrEmpty(DatafieldName)
    End Function

    Public Sub ResetDatafieldName()
        DatafieldName = Nothing
    End Sub

    ''' <summary>
    ''' Bestimmt oder Ermittelt den Text der ausgegeben werden soll, wenn der Anwender versucht ein Feld zu verlassen, 
    ''' dass keine Eingaben enthält. Ist der Text nicht definiert, kann das Feld verlassen werden.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Mit dieser Eigenschaft bestimmen Sie einerseits, ob ein Eingabefeld Null-Werte enthalten darf oder nicht, und 
    ''' andererseits wird gleichzeitig festgelegt, welche Fehlermeldung für den Benutzer ausgegeben werden soll, wenn er 
    ''' versucht, ein Eingabefeld zu verlassen, das keine Null-Werte akzeptiert, er aber keinen Wert eingegeben hat.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder Ermittelt den Text der ausgegeben werden soll, wenn der Anwender versucht ein Feld zu verlassen, dass keine Eingaben enthält."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property NullValueMessage As String Implements INullableValueDataBinding.NullValueMessage

    Public Function ShouldSerializeNullValueMessage() As Boolean
        Return Not String.IsNullOrEmpty(NullValueMessage)
    End Function

    Public Sub ResetNullValueMessage()
        NullValueMessage = Nothing
    End Sub

    Protected Overrides Sub OnCheckStateChanged(ByVal e As System.EventArgs)
        MyBase.OnCheckStateChanged(e)

        'Wird geladen - keine Change-Ereignisse auslösen!
        If Not IsLoading.Value Then

            'Hier entscheiden wir, ob der Setter (siehe ValueAsObject) oder der Benutzer 
            'für ValueChanged verantwortlich war.
            Dim evc As New ValueChangedEventArgs
            If myValueChangedByPropertySetter Then
                evc.ValueChangedCause = ValueChangedCauses.PropertySetter
            Else
                evc.ValueChangedCause = ValueChangedCauses.User
            End If

            'Flag zurücksetzen: wir wissen Bescheid!
            myValueChangedByPropertySetter = False

            'ValueChanged auslösen.
            OnValueChanged(evc)

            'IsDirty-Handling.
            If Not myIsDirty Then
                IsDirty = True
            End If
        End If
    End Sub

    Protected Overridable Sub OnValueChanged(ByVal e As ValueChangedEventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnIsDirtyChanged(ByVal e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Private Property ValueAsObject As Object Implements INullableValueDataBinding.Value
        Get
            If Me.Checked Then
                Return True
            ElseIf Me.CheckState = Windows.Forms.CheckState.Indeterminate Then
                Return Nothing
            Else
                Return False
            End If
        End Get

        Set(ByVal value As Object)
            'Rausfinden, ob hier überhaupt ein ValueChange stattfindet:
            If value Is Nothing And Me.Value Is Nothing Then
                'nichts geändert, und tschüss
                Return
            End If

            If value IsNot Nothing AndAlso Me.Value IsNot Nothing Then
                If value.Equals(Me.Value) Then
                    'auch nichts geändert, tschüss
                    Return
                End If
            End If

            'Property-Setter ist die Ursache für den Wertewechsel - deswegen das entsprechende Flag setzen,
            'damit OnCheckState gleich Bescheid weiß.
            myValueChangedByPropertySetter = True
            If value Is Nothing Then
                Me.CheckState = Windows.Forms.CheckState.Indeterminate
            Else
                'Versuch zu casten
                Dim tmpValue As BooleanEx
                Try
                    tmpValue = New BooleanEx(value)
                Catch ex As Exception
                    Dim up As New TypeMismatchException("The value property of NullableCheckBox accepts only nullables of type BooleanEx and compatible types.")
                    Throw up
                End Try
                Me.CheckState = If(tmpValue, CheckState.Checked, CheckState.Unchecked)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt den Wert, den dieses Steuerelement repräsentiert.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt den Wert, den dieses Steuerelement repräsentiert."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True)>
    Public Property Value As Nullable(Of BooleanEx)
        Get
            Return New BooleanEx(Me.ValueAsObject)
        End Get
        Set(ByVal value As Nullable(Of BooleanEx))
            Me.ValueAsObject = value
        End Set
    End Property

    ''' <summary>
    ''' Ermittelt ob sich der Value-Wert seit der letzten (ersten) Zuweisung geändert hat, und ein Datensatz deswegen aktualisiert werden muss.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
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
            If myIsLoading Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            End If
            Return myIsLoading
        End Get
        Set(ByVal value As HistoricalBoolean)
            If value Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            Else
                myIsLoading = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Setzt den Status zurück, dass dieses Feld vom Anwender geändert wurde, und sein Value in der Datenquelle aktualisiert werden sollte.
    ''' </summary>
    ''' <remarks>Diese Methode wird von der Infrastruktur verwendet, und sollte nicht direkt angewendet werden.</remarks>
    Public Sub ResetIsDirty() Implements INullableValueDataBinding.ResetIsDirty
        myIsDirty = False
    End Sub

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
    Public Property AssignedManagerComponent As FormToBusinessClassManager Implements INullableValueControl.AssignedManagerControl

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

    Public ReadOnly Property ElementType As PermissionManageableUIElementType Implements IPermissionManageableComponent.ElementType
        Get
            Return PermissionManageableUIElementType.Control
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsManageable As Boolean Implements IPermissionManageableComponent.IsManageable

    ''' <summary>
    ''' Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(GetType(ContentPresentPermissions), "ContentPresentPermissions.Normal")>
    Public Property ContentPresentPermission As ContentPresentPermissions Implements IPermissionManageableUIContentElement.ContentPresentPermission

    ''' <summary>
    ''' Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Dass diese Eigenschaft UIGuid und nicht PermissionGuid lautet, hat historische Gründe. Diese Eigenschaft implementiert 
    ''' IPermissionManageableUIContentElement.PermissionGuid.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property UIGuid As System.Guid Implements IPermissionManageableComponent.IdentificationGuid

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property PermissionReason As String Implements IPermissionManageableComponent.PermissionReason

    Public Property ExecuteCallback As System.Action(Of Object) Implements IPermissionManageableUIControlElement.ExecuteCallback

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

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(5000)>
    Public Property ExceptionBalloonDuration As Integer Implements INullableValueControl.ExceptionBalloonDuration

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
    Public Property IsKeyField As Boolean Implements IKeyFieldProvider.IsKeyField
End Class
