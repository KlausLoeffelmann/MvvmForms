'*****************************************************************************************
'                                         DirtyStateManager.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing.Design

''' <summary>
''' Komponente, die für das Überwachen des IsDirty-Status einer View (Formular, UserControl) zuständig ist.
''' </summary>
''' <remarks>
''' <para>Als 'Dirty' bezeichnet man ein Formular oder UserControl, das zur Eingabe von Daten dient, wenn nach dem Anzeigen von Daten der 
''' Anwender Änderungen irgendwo im Formular oder im UserControl vorgenommen hat, sodass diese Änderungen entweder gespeichert werden sollten 
''' oder der Anwender in der Lage ist, die Änderungen wieder rückgängig zu machen. Möchte ein Programm also anzeigen, dass wenn der Anwender Eingaben 
''' in einem Eingabeformular gemacht hat, es speicherbare Änderungen gibt, indem es beispielsweise eine Speicher-Schaltfläche aktiviert, muss es 
''' über diese Statusänderung informiert werden (<see cref="DirtyStateManager.IsDirty">IsDirty</see> ändert sich auf True, 
''' das <see cref="DirtyStateManager.IsDirtyChanged">IsDirtyChange-Ereignis</see> wird ausgelöst).</para> 
''' <para>Wenn innerhalb einer View die DirtyStateManager-Komponente verwendet wird, hat die View die Möglichkeit, darüber informiert zu werden,
''' ob der Anwender innerhalb der View Änderungen vorgenommen hat. Zu diesem Zweck braucht es innerhalb 
''' der View neben dem MvvmManager auch eine DirtyStateManager-Komponente, die sämtlichen Steuerelementen in der 
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
<ProvideProperty("CausesIsDirtyChanged", GetType(Control)),
ProvideProperty("IsDirtyChangedCausingEvent", GetType(Control))> _
Public Class DirtyStateManager
    Inherits Component
    Implements IExtenderProvider
    Implements ISupportInitialize

    ''' <summary>
    ''' Ereignis, das ausgelöst wird, wenn die Komponente eine Veränderung an einer relevanten Eigenschaft im Formular feststellt. 
    ''' Die <see cref="DirtyStateManager.ObservingEnabled">ObservingEnabled-Eigenschaft</see> muss dazu aktiviert sein.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event IsDirtyChanged(sender As Object, e As IsDirtyChangedEventArgs)

    Private myIsDirty As Boolean
    Private myIsDirtyChangedEventArgs As IsDirtyChangedEventArgs
    Private WithEvents myDirtyManagerPropertyStore As New ExtenderProviderPropertyStore(Of IsDirtyManagerPropertyStoreItem)

    Private myDeferredControlValueList As New List(Of Tuple(Of Control, String))
    Private myObservingEnabled As Boolean

    <Category("MVVM"), DisplayName("CausesIsDirtyChanged"), DefaultValue(True)> _
    Public Function GetCausesIsDirtyChanged(ByVal ctrl As Control) As Boolean
        Return Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).CausesIsDirtyChanged
    End Function

    Public Sub SetCausesIsDirtyChanged(ByVal ctrl As Control, ByVal value As Boolean)
        Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).CausesIsDirtyChanged = value
    End Sub

    <Category("MVVM"), DisplayName("IsDirtyChangedCausingEvent")> _
    Public Function GetIsDirtyChangedCausingEvent(ByVal ctrl As Control) As String
        Return Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent
    End Function

    'Müssen wir getrennt implementieren, damit, falls der MvvmManager alphabetisch/namenstechnisch *nach* dem
    'IsDirty-Manager in InitializeComponent des Containers initialisiert wird, er also nach dem IsDirty-Manager...
    Public Sub SetIsDirtyChangedCausingEvent(ByVal ctrl As Control, ByVal value As String)
        If (ctrl Is Nothing) Then
            Throw New ArgumentException("Control must not NULL (nothing in VB).")
        End If

        If ((Me.myDirtyManagerPropertyStore.Contains(ctrl) AndAlso
             Not Object.Equals(Me.myDirtyManagerPropertyStore.Item(ctrl).Data.IsDirtyChangedCausingEvent, value)) AndAlso
         Not String.IsNullOrWhiteSpace(Me.myDirtyManagerPropertyStore.Item(ctrl).Data.IsDirtyChangedCausingEvent)) Then
            ctrl.TryUnbindEvent(Of EventArgs)(Me.myDirtyManagerPropertyStore.Item(ctrl).Data.IsDirtyChangedCausingEvent,
                                              New EventHandler(Of EventArgs)(AddressOf Me.GenericControlsEventHandler))
        End If

        If String.IsNullOrWhiteSpace(value) Then
            If Not Me.myDirtyManagerPropertyStore.Contains(ctrl) Then
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent = value
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).CausesIsDirtyChanged = True
            Else
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent = value
            End If
        Else
            If (ctrl.GetType.GetEvent(value) Is Nothing) Then
                Throw New ArgumentException(String.Concat(New String() {"The event '", value, "' does not exist for control of type '", ctrl.GetType.Name, "'."}))
            End If
            If Not Me.myDirtyManagerPropertyStore.Contains(ctrl) Then
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent = value
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).CausesIsDirtyChanged = True
                Me.BindEvent(ctrl)
            Else
                Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent = value
            End If
        End If
    End Sub

    '...erst all seine Steuerelemente erhält, wir dennoch alle Steuerelementzuweisungen ermitteln können.
    'Deswegen auch die Implementierung von BeginInit und EndInit (s.u.) über das entsprechende Interface.
    Private Sub ProcessDeferredControlValueList()
        For Each item In myDeferredControlValueList

            Dim ctrl = item.Item1
            Dim value = item.Item2

        Next
    End Sub

    Private Sub BeginInit() Implements ISupportInitialize.BeginInit
        'We don't have to do anything here.
    End Sub

    'Müssen wir implementieren, damit, falls der MvvmManager alphabetisch/namenstechnisch nach dem
    'IsDirty-Manager kommt, er also nach dem IsDirty-Manager erst all seine Steuerelemente
    'erhält, wir dennoch alle Steuerelementzuweisungen ermitteln können.
    Private Sub EndInit() Implements ISupportInitialize.EndInit
        'ProcessDeferredControlValueList()
    End Sub

    Public Function ShouldSerializeIsDirtyChangedCausingEvent(ByVal ctrl As Control) As Boolean
        If (ctrl Is Nothing) Then
            Return False
        End If
        Return True
    End Function

    Public Function CanExtend(ByVal extendee As Object) As Boolean Implements IExtenderProvider.CanExtend
        If GetType(Control).IsAssignableFrom(extendee.GetType) Then
            Dim ctrlTemp As Control = DirectCast(extendee, Control)
            If Not Me.myDirtyManagerPropertyStore.Contains(ctrlTemp) Then
                Dim tmpStoreItem As IsDirtyManagerPropertyStoreItem = Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrlTemp)
                tmpStoreItem.IsDirtyChangedCausingEvent = Me.GetDefaultIsDirtyChangedCausingEventForControl(ctrlTemp)
                tmpStoreItem.CausesIsDirtyChanged = True
            End If
            Return True
        End If
        Return False
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            Dim sItem As ExtenderProviderPropertyStoreItem(Of IsDirtyManagerPropertyStoreItem)
            For Each sItem In Me.myDirtyManagerPropertyStore
                sItem.Control.TryUnbindEvent(Of EventArgs)(sItem.Data.IsDirtyChangedCausingEvent, New EventHandler(Of EventArgs)(AddressOf Me.GenericControlsEventHandler))
            Next
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub GenericControlsEventHandler(ByVal sender As Object, ByVal e As EventArgs)
        If (Me.ObservingEnabled AndAlso Not Me.IsDirty) Then
            If (Me.myIsDirtyChangedEventArgs Is Nothing) Then
                Me.myIsDirtyChangedEventArgs = New IsDirtyChangedEventArgs
            End If
            Me.myIsDirtyChangedEventArgs.CausingControl = DirectCast(sender, Control)

            'Wir dürfen hier pauschal IsDirty auf True setzen, mit einer einzigen Ausnahme:
            'Wenn die IsDirty-Eigenschaft sich selbst geändert hat, dann darf sie natürlich
            'nicht beim Zurücksetzen dafür verantwortlich sein, dass der IsDirty-Zustand
            'sich wieder in Dirty ändert. Der IsDirty-Event muss also gefiltert, bzw.
            'gefunden und besonders überprüft werden.

            'Wichtig: Falls IsDirty eines solchen Controls NICHT das Ereignis war,
            'das zum IsDirty-Zustand geführt hat, dann muss das Control beim Zurücksetzen der
            'IsDirty-auslösenden Eigenschaft selbständig dafür sorgen, dass sein IsDirty-Zustand
            'entsprechend synchronisiert auch wieder zurückgesetzt wird.
            Dim dirtyAwareControl = TryCast(sender, IIsDirtyChangedAware)
            If dirtyAwareControl IsNot Nothing AndAlso Not dirtyAwareControl.IsDirty Then
                Me.IsDirty = False
            Else
                Me.IsDirty = True
            End If
        End If
    End Sub

    Protected Overridable Function GetDefaultIsDirtyChangedCausingEventForControl(ByVal ctrl As Control) As String
        If GetType(IIsDirtyChangedAware).IsAssignableFrom(ctrl.GetType) Then
            Return "IsDirtyChanged"
        End If
        If GetType(TextBoxBase).IsAssignableFrom(ctrl.GetType) Then
            Return "TextChanged"
        End If
        If GetType(CheckBox).IsAssignableFrom(ctrl.GetType) Then
            Return "CheckedChanged"
        End If
        If GetType(RadioButton).IsAssignableFrom(ctrl.GetType) Then
            Return "CheckedChanged"
        End If
        If GetType(DateTimePicker).IsAssignableFrom(ctrl.GetType) Then
            Return "ValueChanged"
        End If
        If GetType(MonthCalendar).IsAssignableFrom(ctrl.GetType) Then
            Return "ValueChanged"
        End If
        If GetType(TrackBar).IsAssignableFrom(ctrl.GetType) Then
            Return "ValueChanged"
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Setzt den IsDirty-Zustand zurück.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetIsDirty()
        If Me.IsDirty Then
            Me.myIsDirtyChangedEventArgs.CausingControl = Nothing
            Me.IsDirty = False

            'Alle Controls zurücksetzen, die INullableValueDataBinding implementieren,
            'und damit einen eigenen IsDirty-Zustand überwachen können.
            For Each sItem In Me.myDirtyManagerPropertyStore
                Dim IsControlWithWhichWeHaveToResetTheIsDirtyState = TryCast(sItem.Control, IIsDirtyChangedAware)
                'Wenn ein INullableValueDateBinding-Control dazu führte, dass (direkt bei der ersten Eingabe) sich der
                'Status geändert hat, müssen wir seinen IsDirtyStatus sofort wieder zurücksetzen.
                If IsControlWithWhichWeHaveToResetTheIsDirtyState IsNot Nothing Then
                    IsControlWithWhichWeHaveToResetTheIsDirtyState.ResetIsDirty()
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Erzwingt den IsDirty-Zustand für Zustände manuell, die von dieser Komponente nicht selbst entdeckt werden können.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ForceIsDirty(causingControl As Control)
        myIsDirtyChangedEventArgs = New IsDirtyChangedEventArgs With {.CausingControl = causingControl}
        Me.IsDirty = True
    End Sub

    Public Sub ResetIsDirtyChangedCausingEvent(ByVal ctrl As Control)
        Me.SetIsDirtyChangedCausingEvent(ctrl, Me.GetDefaultIsDirtyChangedCausingEventForControl(ctrl))
    End Sub

    ''' <summary>
    ''' Ermittelt, ob die Komponente für eine View den IsDirty-Zustand festgestellt hat (eine relevante Eigenschaft eines Controls der View wurde verändert).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)>
    Property IsDirty As Boolean
        Get
            Return Me.myIsDirty
        End Get
        Private Set(ByVal value As Boolean)
            If (Me.myIsDirty <> value) Then
                Me.myIsDirty = value
                Me.OnIsDirtyChanged(Me.myIsDirtyChangedEventArgs)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Löst das <see cref="IsDirtyChanged">IsDirtyChanged-Ereignis</see>  aus.
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnIsDirtyChanged(e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    'Wird von außen gesteuert!
    <Browsable(False)>
    Friend Property ObservingEnabled As Boolean
        Get
            Return myObservingEnabled
        End Get
        Set(value As Boolean)
            If myObservingEnabled <> value Then
                myObservingEnabled = value
            End If
        End Set
    End Property

    Private Sub BindEvent(ByVal ctrl As Control)
        Dim retValue As Boolean = ctrl.TryBindEvent(Of EventArgs)(Me.myDirtyManagerPropertyStore.GetPropertyStoreItem(ctrl).IsDirtyChangedCausingEvent,
                                                                  New EventHandler(Of EventArgs)(AddressOf Me.GenericControlsEventHandler))
    End Sub
End Class

Public Class IsDirtyManagerPropertyStoreItem
    Public Property CausesIsDirtyChanged As Boolean
    Public Property IsDirtyChangedCausingEvent As String
End Class
