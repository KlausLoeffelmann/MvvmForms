'*****************************************************************************************
'                                          BindingManager
'                                          ==============
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
'    CONTACT INFO:
'    Klaus Loeffelmann, C/O ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.Windows.Forms
Imports System.Collections.ObjectModel

Imports ActiveDevelop.EntitiesFormsLib
Imports System.Reflection
Imports System.ComponentModel
Imports System.Windows.Data
Imports System.Globalization

Public Delegate Sub BindingChangedEventHandler(sender As Object, e As BindingPropertyChangedEventArgs)

Public Class BindingManager
    Implements IDisposable

    Private mySourceObjects As New ObjectEventsAssignments
    Private myViewModel As INotifyPropertyChanged
    Private mySupressTargetUpdating As Boolean

    Sub New(viewModel As INotifyPropertyChanged, bindingItems As BindingItems,
            mvvmManager As MvvmManager,
            Optional throwExceptionOnUnassignableBindingProperties As Boolean = True,
            Optional updateControlsAfterInstatiating As Boolean = False)
        MyBase.New()
        throwExceptionOnUnassignableBindingProperties = throwExceptionOnUnassignableBindingProperties
        Me.MvvmManager = mvvmManager
        myViewModel = viewModel
        Me.BindingItems = bindingItems
        AddControlsPropertyChangeEvents()
        If updateControlsAfterInstatiating Then
            UpdateControlsFromViewModel()
        End If
    End Sub

    Public Event MvvmBindingException(bindingManagerSender As Object, e As MvvmBindingExceptionEventArgs)

    Public Event ValueAssigning(sender As Object, e As ValueAssigningEventargs)
    Public Event ValueAssigned(sender As Object, e As ValueAssignedEventArgs)

    Private Sub RemoveControlsPropertyChangeEvents()
        For Each controlItem In BindingItems

            Dim controlToBind = TryCast(controlItem.Control, Control)

            For Each propBindingItem In controlItem.MvvmItem.PropertyBindings
                'Wenn nur ViewModel --> Control gebunden ist, ODER Binding auf Explicit steht,
                'dann werden keine Control auslösenden Ereignisse gebunden
                If propBindingItem.BindingSetting.BindingMode.HasFlag(MvvmBindingModes.OneWay) Then
                    'Diese Zeile bindet alle notwendigen Events in Richtung ViewModel-->Control.
                    'In diesem Fall OneWayBinding.
                    Continue For
                End If

                'Bei Explicit darf nix gebunden werden. Dann gehen wir gleich weiter zum nächsten Binding.
                If propBindingItem.BindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.Explicit Then
                    'Dann gibt es kein Ereignis, das entbunden werden muss.
                    Continue For
                End If

                Dim eventName = propBindingItem.ControlProperty.PropertyName & "Changed"

                If propBindingItem.BindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus Then
                    Dim traceInfo = "Unbinding Leave (LostFocus) Event for Property " & propBindingItem.ControlProperty.PropertyName &
                                            " as " & propBindingItem.ControlProperty.PropertyType.Name & " for control " & controlToBind.Name &
                                            " (" & controlToBind.GetType.Name & ")."
                    MvvmFormsEtw.Log.BindingSetup(traceInfo)
                    mySourceObjects.Remove(controlToBind, eventName)
                    mySourceObjects.Remove(controlToBind, "Leave")
                Else

                    'An dieser Stelle müssen wir versuchen, die Changing-Ereignisse zu binden:
                    Dim traceInfo = "Unbinding ChangedEvent for Property " & propBindingItem.ControlProperty.PropertyName &
                                           " as " & propBindingItem.ControlProperty.PropertyType.Name & " for control " & controlToBind.Name &
                                           " (" & controlToBind.GetType.Name & ")."
                    MvvmFormsEtw.Log.BindingSetup(traceInfo)
                    mySourceObjects.Remove(controlToBind, eventName)
                End If
            Next
        Next
    End Sub

    Private Sub AddControlsPropertyChangeEvents()

        For Each controlItem In BindingItems

            Dim controlToBind = TryCast(controlItem.Control, Control)

            For Each propBindingItem In controlItem.MvvmItem.PropertyBindings
                'Wenn nur ViewModel --> Control gebunden ist, ODER Binding auf Explicit steht,
                'dann werden keine Control auslösenden Ereignisse gebunden
                If propBindingItem.BindingSetting.BindingMode.HasFlag(MvvmBindingModes.OneWay) Then
                    'Diese Zeile bindet alle notwendigen Events in Richtung ViewModel-->Control.
                    'In diesem Fall OneWayBinding.
                    propBindingItem.PropertyBindingManager = New PropertyBindingManager(ViewModel, controlItem.Control, propBindingItem, Me)
                    Continue For
                End If

                'Bei Explicit darf nix gebunden werden. Dann gehen wir gleich weiter zum nächsten Binding.
                If propBindingItem.BindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.Explicit Then
                    'Nur in diesem Fall wird 
                    Continue For
                End If

                'Außer für OneWayToSource-Bindings (Control --> ViewModel) wird für alle anderen Fälle 
                'auf jeden Fall ViewModel an Control gebunden.
                If Not propBindingItem.BindingSetting.BindingMode.HasFlag(MvvmBindingModes.OneWayToSource) Then
                    'Diese Zeile bindet alle notwendigen Events in Richtung ViewModel-->Control.
                    'In diesem Fall Zwei-Wege-Binding.
                    propBindingItem.PropertyBindingManager = New PropertyBindingManager(ViewModel, controlItem.Control, propBindingItem, Me)
                End If

                Dim eventName = propBindingItem.ControlProperty.PropertyName & "Changed"

                If propBindingItem.BindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus Then
                    Dim traceInfo = "Binding Leave (LostFocus) Event for Property " & propBindingItem.ControlProperty.PropertyName &
                                            " as " & propBindingItem.ControlProperty.PropertyType.Name & " for control " & controlToBind.Name &
                                            " (" & controlToBind.GetType.Name & ")."
                    MvvmFormsEtw.Log.BindingSetup(traceInfo)

                    If mySourceObjects.Add(controlToBind, New ObjectEvent With {.EventName = eventName,
                                                    .EventTarget = New PropertyChangedEventHandler(AddressOf propBindingItem.ControlPropertyChangedEventHandler),
                                                    .TraceInfo = traceInfo}) Then
                        propBindingItem.ControlPropertyChangedEventHandlerTarget = AddressOf OnControlPropertyChanged
                    End If

                    If mySourceObjects.Add(controlToBind, New ObjectEvent With {.EventName = "Leave",
                                                .EventTarget = New PropertyChangedEventHandler(AddressOf ControlLostFocusEventHandler),
                                                .TraceInfo = traceInfo}) Then
                    End If
                Else

                    'An dieser Stelle müssen wir versuchen, die Changing-Ereignisse zu binden:
                    Dim traceInfo = "Binding Changed Event for Property " & propBindingItem.ControlProperty.PropertyName &
                                           " as " & propBindingItem.ControlProperty.PropertyType.Name & " for control " & controlToBind.Name &
                                           " (" & controlToBind.GetType.Name & ")."
                    MvvmFormsEtw.Log.BindingSetup(traceInfo)
                    If mySourceObjects.Add(controlToBind, New ObjectEvent With {.EventName = eventName,
                                                    .EventTarget = New PropertyChangedEventHandler(AddressOf propBindingItem.ControlPropertyChangedEventHandler),
                                                    .TraceInfo = traceInfo}) Then
                        propBindingItem.ControlPropertyChangedEventHandlerTarget = AddressOf OnControlPropertyChanged
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub ControlLostFocusEventHandler(sender As Object, e As EventArgs)
        'Alle Eigenschaften ermitteln, die IsDirty sind und die LostFocus als SourceTrigger haben.
        'Deren PropertyChange-Ereignisse lösen wir dann aus.
        Dim sourceAsControl = TryCast(sender, Control)
        If sourceAsControl IsNot Nothing Then
            Dim bindingInfoItems = (From bindingInfoItem In Me.BindingItems(sourceAsControl).MvvmItem.PropertyBindings
                                Where bindingInfoItem.BindingSetting.UpdateSourceTrigger = UpdateSourceTriggerSettings.LostFocus AndAlso
                                      bindingInfoItem.IsDirty = True).ToList

            For Each item In bindingInfoItems
                item.ControlPropertyChangedEventHandlerTarget.Invoke(New BindingPropertyChangedEventArgs() With
                                                                     {.Converter = item.ConverterInstance,
                                                                      .EventProperty = item.ControlProperty.PropertyName,
                                                                      .OriginalSource = sourceAsControl})
            Next
        End If
    End Sub

    Protected Overridable Sub OnControlPropertyChanged(e As BindingPropertyChangedEventArgs)
        UpdateControlToViewModel(e)
    End Sub

    Protected Overridable Sub UpdateControlToViewModel(e As BindingPropertyChangedEventArgs)

        'Wenn gerade Eigenschaften vom ViewModel geschrieben werden, dann werden alle Updates von
        'Steuerelementen ignoriert.

        'Das Steuerelement, das das Ereignis ausgelöst hast.
        Dim sourceAsControl = TryCast(e.OriginalSource, Control)

        'Die Bindinginfos, aus der Eigenschaft des Steuerelementes, das das Ereignis ausgelöst hat.
        '(sind mehere, da ja eine Eigenschaft an mehrere Eigenschaften des ViewModels gebunden sein kann).
        Dim bindingInfos = (From bindingInfoItem In Me.BindingItems(sourceAsControl).MvvmItem.PropertyBindings
                            Where bindingInfoItem.ControlProperty.PropertyName = e.EventProperty).ToList

        For Each bindingInfo In bindingInfos

            'Wenn das Steuerelement durch einen Trigger-Vorgang gerade aktualisiert wird,
            'dann entsprechend
            If bindingInfo.UpdatingControlInProgress Then Continue For

            Try
                bindingInfo.UpdatingViewmodelInProgress = True
                Dim targetAsViewModel = Me.ViewModel

                'Werte im Ziel aktualisieren.
                UpdateTargetPropertyInternal(sourceAsControl, targetAsViewModel, bindingInfo, sender:=Me)
            Catch ex As Exception
                Throw
            Finally
                bindingInfo.UpdatingViewmodelInProgress = False
            End Try
        Next
    End Sub

    Public Property ViewModel As INotifyPropertyChanged
        Get
            Return myViewModel
        End Get
        Set(value As INotifyPropertyChanged)
            If Not Object.Equals(value, myViewModel) Then
                If value Is Nothing Then
                    'Wenn Nothing, dann...
                    If Not mySupressTargetUpdating Then
                        '...nur dann das Ziel mit Nulls updaten, wenn es nicht von Dispose kam.
                        UpdateControlsWithNothing()
                    End If
                End If


                'Alle ViewModel-->Control-Bindungen lösen...
                For Each bindingItem In Me.BindingItems
                    For Each propBindingsItem In bindingItem.MvvmItem.PropertyBindings
                        If Not propBindingsItem.BindingSetting.BindingMode.HasFlag(MvvmBindingModes.OneWayToSource) Then
                            propBindingsItem.PropertyBindingManager.Dispose()
                        End If
                    Next
                Next

                '...alle Control-->ViewModel bindungen lösen...
                RemoveControlsPropertyChangeEvents()

                'Neues ViewModel zuweisen
                myViewModel = value

                If value IsNot Nothing Then
                    'Neue Ereignisse binden.
                    AddControlsPropertyChangeEvents()

                    'Initialwerte schreiben bei der Zuweisung.
                    UpdateControlsFromViewModel()
                End If
            End If
        End Set
    End Property

    Friend Property BindingItems As BindingItems

    ''' <summary>
    ''' Aktualisiert das Target (ViewModel) mti dem Wert einer Eigenschaft des Source (Control) auf Basis eines PropertyBindingItem.
    ''' </summary>
    ''' <param name="source">Quell-Objekt, in der Regel ein Steuerelement.</param>
    ''' <param name="target">Zielobjekt, in der Regel ein ViewModel.</param>
    ''' <param name="bindingInfo">Bestimmt, welche Eigenschaften auf beiden Seiten verwendet werden sollen.</param>
    ''' <remarks></remarks>
    Friend Shared Sub UpdateTargetPropertyInternal(source As Object, target As Object,
                                                   bindingInfo As PropertyBindingItem,
                                                   Optional currentCulture As CultureInfo = Nothing,
                                                   Optional sender As BindingManager = Nothing)

        If currentCulture Is Nothing Then
            currentCulture = CultureInfo.CurrentCulture
        End If

        Dim temp As Tuple(Of Object, PropertyInfo)

        temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(target, bindingInfo.ViewModelProperty.PropertyName)

        If temp.Item2 Is Nothing Then
            'Hier gab es kein Ergebnis, dann brechen wir ab.
            Dim traceinfo = "Aquiring Value for property " & bindingInfo.ViewModelProperty.PropertyName & " caused a NullReference-Exception."
            Dim ex = HandleBindingException(traceinfo, sender)
            If Not ex.Handled Then
                Throw ex
            End If

            Return
        End If

        If temp IsNot Nothing AndAlso (temp.Item1 Is Nothing And temp.Item2 Is Nothing) Then
            Dim traceInfo = "Binding exception: " &
                            "The property path '" & bindingInfo.ViewModelProperty.PropertyName &
                            "' to the viewmodel of type is not valid."
            Dim ex = HandleBindingException(traceInfo, sender)
            If Not ex.Handled Then
                Throw ex
            End If

            Return
        End If

        Dim sourcePropertyInfo = source.GetType.GetProperty(bindingInfo.ControlProperty.PropertyName)
        Dim targetPropertyInfo As PropertyInfo = temp.Item2
        Dim targetObject = temp.Item1
        Dim sourceObject = source

        Dim sourceValue As Object = Nothing
        Dim usedConverter As IValueConverter = Nothing

        If bindingInfo.Converter Is Nothing Then
            'String to StringValueConverter automatisch einsetzen!
            If targetPropertyInfo.PropertyType Is GetType(String) AndAlso (sourcePropertyInfo.PropertyType Is GetType(StringValue) Or
                                                                           sourcePropertyInfo.PropertyType Is GetType(StringValue?)) Then
                usedConverter = StringValueToStringConverter.GetInstance
            ElseIf targetPropertyInfo.PropertyType Is GetType(String) Then
                usedConverter = ObjectToStringConverter.GetInstance

            ElseIf (targetPropertyInfo.PropertyType Is GetType(Boolean) OrElse
                    targetPropertyInfo.PropertyType Is GetType(Boolean?)) AndAlso
                                                        (GetType(BooleanEx).IsAssignableFrom(sourcePropertyInfo.PropertyType) Or
                                                        GetType(BooleanEx?).IsAssignableFrom(sourcePropertyInfo.PropertyType)) Then
                usedConverter = BooleanToBooleanExConverter.GetInstance
            End If
        Else
            usedConverter = DirectCast(Activator.CreateInstance(bindingInfo.Converter), IValueConverter)
        End If

        Try
            If usedConverter IsNot Nothing Then
                Dim valuetoconvert = sourcePropertyInfo.GetValue(sourceObject, Nothing)
                MvvmFormsEtw.Log.ViewModelBindingInfo("Got value for property " & sourcePropertyInfo.Name & "; using " & usedConverter.GetType.Name & ".")
                If valuetoconvert IsNot Nothing Then
                    sourceValue = usedConverter.ConvertBack(valuetoconvert,
                                                        targetPropertyInfo.PropertyType, bindingInfo.ConverterParameter,
                                                        currentCulture)
                Else
                    sourceValue = valuetoconvert
                End If
            Else
                sourceValue = sourcePropertyInfo.GetValue(sourceObject, Nothing)
                MvvmFormsEtw.Log.ViewModelBindingInfo("Got value for property " & sourcePropertyInfo.Name & " without using a converter.")
            End If

        Catch innerEx As Exception
            Dim traceInfo = "Binding exception: Couldn't retreive value from source object."
            Dim ex = HandleBindingException(traceInfo, sender,
                                            innerEx, sourcePropertyInfo, sourceObject)
            If Not ex.Handled Then
                Throw ex
            End If
        End Try

        Try
            Dim assigningE As ValueAssigningEventArgs = Nothing
            If sender IsNot Nothing Then
                assigningE = New ValueAssigningEventArgs With
                               {.Cancel = False,
                                .Control = targetObject,
                                .ControlPropertyName = sourcePropertyInfo.Name,
                                .Target = Targets.Control,
                                .Value = sourceValue,
                                .ViewModelObject = source,
                                .ViewModelPropertyName = targetPropertyInfo.Name}
                sender.OnValueAssigning(assigningE)
                If assigningE.Cancel Then Return
            End If

            If targetObject IsNot Nothing Then
                targetPropertyInfo.SetValue(targetObject, sourceValue, Nothing)
                MvvmFormsEtw.Log.ViewModelBindingInfo("Assigned value for property " & targetPropertyInfo.Name & ".")
            End If

            If sender IsNot Nothing Then
                sender.OnValueAssigned(New ValueAssignedEventArgs(assigningE))
            End If

        Catch innerEx As Exception
            Dim traceInfo = "Binding exception: Couldn't set value (" &
                                            If(sourceValue Is Nothing, "NULL", sourceValue.ToString & " of " &
                                               sourceValue.GetType.Name & ") in target object.")
            Dim ex = HandleBindingException(traceInfo, sender,
                                            innerEx, targetPropertyInfo, targetObject)
            If Not ex.Handled Then
                Throw ex
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Aktualisiert den Source (Control) mit dem Wert einer Eigenschaft des Target (ViewModel) auf Basis eines PropertyBindingItem.
    ''' </summary>
    ''' <param name="source">Quell-Objekt, in der Regel ein Steuerelement.</param>
    ''' <param name="target">Zielobjekt, in der Regel ein ViewModel.</param>
    ''' <param name="bindingInfo">Bestimmt, welche Eigenschaften auf beiden Seiten verwendet werden sollen.</param>
    ''' <param name="UpdatePartPathOfProperty">True, wenn es sich um eine Teilaktualisierung handelt, und target nicht das komplette ViewModel 
    ''' sondern nur eine Eigenschaft davon bildet, die seinerseits ein Objekt mit Eigenschaften bildet. Nur der Eigentliche PropertyName wird 
    ''' dann verwendet, nicht der Property-Pfad. Wird verwendet, wenn sich nur ein Teilpfad der gesamten Bindung geändert hat.</param>
    ''' <remarks></remarks>
    Friend Shared Sub UpdateSourcePropertyInternal(source As Object, target As Object, bindingInfo As PropertyBindingItem,
                                                   Optional UpdatePartPathOfProperty As Boolean = False,
                                                   Optional currentCulture As CultureInfo = Nothing,
                                                   Optional sender As BindingManager = Nothing,
                                                   Optional isAssigning As Boolean = False)

        If currentCulture Is Nothing Then
            currentCulture = CultureInfo.CurrentCulture
        End If

        'An dieser Stelle bekommen wir im target u.U. schon das Teilobjekt des Bindungsziels (bei einem Pfad)
        'aber immer noch den vollständigen Propertypfad, von dem wir dann nur den eigentlichen letzten Propertynamen
        'benötigen, da der sich auf target bezieht.
        Dim propertyShortName = bindingInfo.ViewModelProperty.PropertyName

        Dim dotPos = propertyShortName.LastIndexOf("."c)
        Dim temp As Tuple(Of Object, PropertyInfo) = Nothing

        'Sonderprüfung, für Doppelnamen bspw. SomeViewModel.StringValue und .StringValue;
        'Abfangen, dass bei SomeViewModel = Null die unterschiedlichen "StringValue"-Werte 
        'richtig erkannt werden, und nicht die eine, losgelöste Eigenschaft fälkschlicherweise
        'Bindungen überschreibt, für die die erste ausschließlich zuständig war.

        'Nur, wenn es tiefe Eigenschaften gibt, und nicht Null zum Löschen zugewiesen werden soll,
        'UND es sich nicht um ein initiales zuweisen handelt.
        If dotPos > -1 AndAlso target IsNot Nothing AndAlso (Not isAssigning) Then
            Try
                temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(target, propertyShortName)
                'Wir brechen nur dann ab, wenn die Bindung an sich gültig war, aber ein Objekt auf dem
                'Pfad zur Zieleigenschaft Null war; dann handelt es sich um ein Objekt, das zwar PropertyChange
                'auslöst, allerdings das im Kontext nicht gültig ist, da ein auf dem Pfad zur Eigenschaft
                'vorheriges Objekt null war.

                'Das gilt allerdings NICHT für ein initiales Assignment, denn hier wissen wir, dass die bindingInfo
                'eindeutig war; wenn dann irgendetwas auf dem Pfad zur eigentlichen, werteliefernde Property schon
                'Null/Nothing war, ist der zugewiesene Wert eben ebenfalls Null/Nothing.
                'Das ist der Grund, weswegen der Optionale Parameter isAssigning im Nachgang noch eingeführt wurde.
                If temp IsNot Nothing AndAlso temp.Item2 Is Nothing Then
                    'Pfad ist nothing - wir müssen abbrechen.
                    Return
                End If
            Catch ex As Exception
                Return
            End Try
        End If

        If UpdatePartPathOfProperty Then
            'Wenn wir hier landen, dann wird nur ein "Teilstück" aktualisiert.
            'In diesem Fall benötigen wir nicht den kompletten Bindungspfad, was den Eigenschaftennamen
            'anbelangt, sondern lediglich den wirklichen Eigenschaftennamen, da das Objekt, das wir beschreiben
            'werden, auch nicht einen Pfad bildet, sondern schon direkt das Ziel bildet.
            'Wird immer dann verwendet, wenn ein Teil eines Eigenschaftenpfades sich geändert hat,
            'also beispielsweise: bei Employee.Contact.Address.Name hat sich Contact oder Address geändert.
            If dotPos > -1 Then
                'Propertynamen extrahieren
                Try
                    propertyShortName = propertyShortName.Substring(dotPos + 1)
                Catch exep As Exception
                    Dim traceInfo = "Binding exception: " &
                                                    "The property path '" & bindingInfo.ViewModelProperty.PropertyName &
                                                    "' to the viewmodel of type is not valid."
                    Dim ex = HandleBindingException(traceInfo, sender)
                    If Not ex.Handled Then
                        Throw ex
                    End If
                End Try
            End If
        End If

        Try
            temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(target, propertyShortName)
        Catch ex As Exception
        End Try

        Dim sourcePropertyInfo = source.GetType.GetProperty(bindingInfo.ControlProperty.PropertyName)
        Dim targetPropertyInfo As PropertyInfo = If(temp IsNot Nothing, temp.Item2, Nothing)
        Dim targetObject = If(temp IsNot Nothing, temp.Item1, Nothing)
        Dim sourceObject = source

        Dim targetValue As Object = Nothing
        Dim usedConverter As IValueConverter = Nothing

        If targetPropertyInfo IsNot Nothing Then
            If bindingInfo.Converter Is Nothing Then

                'String to StringValueConverter automatisch einsetzen!
                If targetPropertyInfo.PropertyType Is GetType(String) AndAlso (sourcePropertyInfo.PropertyType Is GetType(StringValue) Or
                                                                               sourcePropertyInfo.PropertyType Is GetType(StringValue?)) Then
                    usedConverter = StringValueToStringConverter.GetInstance
                ElseIf targetPropertyInfo.PropertyType Is GetType(String) Then
                    usedConverter = ObjectToStringConverter.GetInstance
                ElseIf (targetPropertyInfo.PropertyType Is GetType(Boolean) OrElse
                    targetPropertyInfo.PropertyType Is GetType(Boolean?)) AndAlso (sourcePropertyInfo.PropertyType Is GetType(BooleanEx) Or
                                                                               sourcePropertyInfo.PropertyType Is GetType(BooleanEx?)) Then
                    usedConverter = BooleanToBooleanExConverter.GetInstance
                End If
            Else
                usedConverter = DirectCast(Activator.CreateInstance(bindingInfo.Converter), IValueConverter)
            End If

            Try
                If usedConverter IsNot Nothing Then
                    Dim valueToConvert As Object
                    If targetObject Is Nothing Then
                        valueToConvert = Nothing
                    Else
                        valueToConvert = targetPropertyInfo.GetValue(targetObject, Nothing)
                        MvvmFormsEtw.Log.ControlBindingInfo("Got value for property " & targetPropertyInfo.Name & "; using converter " & usedConverter.GetType.ToString & ".")
                    End If

                    If valueToConvert IsNot Nothing Then
                        targetValue = usedConverter.Convert(valueToConvert,
                                                            sourcePropertyInfo.PropertyType,
                                                            bindingInfo.ConverterParameter,
                                                            currentCulture)
                    Else
                        targetValue = valueToConvert
                    End If
                Else
                    If targetObject Is Nothing Then
                        targetValue = Nothing
                    Else
                        targetValue = targetPropertyInfo.GetValue(targetObject, Nothing)
                        MvvmFormsEtw.Log.ControlBindingInfo("Got value for property " & targetPropertyInfo.Name & " without using a converter.")
                    End If
                End If

            Catch innerEx As Exception
                Dim traceInfo = "Binding exception: Couldn't retreive value from target object."
                Dim ex = HandleBindingException(traceInfo, sender,
                                                innerEx, targetPropertyInfo, targetObject)
                If Not ex.Handled Then
                    Throw ex
                End If
            End Try
        End If

        Try
            Dim assigningE As ValueAssigningEventArgs = Nothing
            If sender IsNot Nothing Then
                assigningE = New ValueAssigningEventArgs With
                               {.Cancel = False,
                                .Control = source,
                                .ControlPropertyName = If(sourcePropertyInfo Is Nothing, "", sourcePropertyInfo.Name),
                                .Target = Targets.Control,
                                .Value = targetValue,
                                .ViewModelObject = targetObject,
                                .ViewModelPropertyName = If(targetPropertyInfo Is Nothing, "", targetPropertyInfo.Name)}
                sender.OnValueAssigning(assigningE)
                If assigningE.Cancel Then Return
            End If

            bindingInfo.UpdatingControlInProgress = True
            If targetPropertyInfo Is Nothing Then
                sourcePropertyInfo.SetValue(sourceObject, Nothing, Nothing)
                MvvmFormsEtw.Log.ControlBindingInfo("Assigning Nothing (null/default in CSharp) for property " & sourcePropertyInfo.Name & ".")
            Else
                sourcePropertyInfo.SetValue(sourceObject, targetValue, Nothing)
                MvvmFormsEtw.Log.ControlBindingInfo("Assigning value for property " & sourcePropertyInfo.Name & ".")
            End If
            bindingInfo.UpdatingControlInProgress = False

            If sender IsNot Nothing Then
                sender.OnValueAssigned(New ValueAssignedEventArgs(assigningE))
            End If

        Catch innerEx As Exception
            Dim traceInfo = "Binding exception: Couldn't set value (" &
                            If(targetValue Is Nothing, "NULL", targetValue.ToString & " of " &
                               targetValue.GetType.Name & ") in source object for property " &
                               sourcePropertyInfo.Name & ".")

            Dim ex = HandleBindingException(traceInfo, sender, innerEx,
                                            targetPropertyInfo, targetObject)
            If Not ex.Handled Then
                Throw ex
            End If
        End Try
    End Sub

    Friend Shared Sub HandleFullPathDataErrorsChangedInternal(source As Object, target As Object,
                                                              bindingInfo As PropertyBindingItem,
                                                   Optional UpdatePartPathOfProperty As Boolean = False,
                                                   Optional currentCulture As CultureInfo = Nothing,
                                                   Optional sender As BindingManager = Nothing,
                                                   Optional IsAssigning As Boolean = False)

        If currentCulture Is Nothing Then
            currentCulture = CultureInfo.CurrentCulture
        End If

        Dim propertyShortName = bindingInfo.ViewModelProperty.PropertyName

        Dim dotPos = propertyShortName.LastIndexOf("."c)
        Dim temp As Tuple(Of Object, PropertyInfo) = Nothing

        'Zur Erklärung der Vorgehensweisen siehe UpdateSourceProperty 
        If dotPos > -1 AndAlso target IsNot Nothing AndAlso Not IsAssigning Then
            Try
                temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(target, propertyShortName)
                If temp IsNot Nothing AndAlso temp.Item2 Is Nothing Then
                    Return
                End If
            Catch ex As Exception
                Return
            End Try
        End If

        If UpdatePartPathOfProperty Then
            If dotPos > -1 Then
                'Propertynamen extrahieren
                Try
                    propertyShortName = propertyShortName.Substring(dotPos + 1)
                Catch exep As Exception
                    Dim traceInfo = "Binding exception: " &
                                    "The property path '" & bindingInfo.ViewModelProperty.PropertyName &
                                    "' to the viewmodel of type is not valid."
                    Dim ex = HandleBindingException(traceInfo, sender)
                    If Not ex.Handled Then
                        Throw ex
                    End If
                End Try
            End If
        End If

        Try
            temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(target, propertyShortName)
        Catch ex As Exception
        End Try

        Dim sourcePropertyInfo = source.GetType.GetProperty(bindingInfo.ControlProperty.PropertyName)
        Dim targetPropertyInfo As PropertyInfo = If(temp IsNot Nothing, temp.Item2, Nothing)

        Dim sourceAsControl = TryCast(source, Control)
        Dim targetAsViewmodel = TryCast(If(temp IsNot Nothing, temp.Item1, Nothing), INotifyDataErrorInfo)

        If targetAsViewmodel Is Nothing OrElse Not targetAsViewmodel.HasErrors Then
            Return
        End If

        Dim errors = targetAsViewmodel.GetErrors(targetPropertyInfo.Name)
        Dim errorString As String = ""
        Dim first = True
        If errors IsNot Nothing Then
            For Each item In errors
                If Not first Then
                    errorString &= vbNewLine
                Else
                    first = False
                End If
                errorString &= item.ToString
            Next
        End If

        If Not String.IsNullOrWhiteSpace(errorString) Then
            If sender.MvvmManager IsNot Nothing Then
                sender.MvvmManager.SetError(sourceAsControl, errorString)
            End If
        Else
            If sender.MvvmManager IsNot Nothing Then
                sender.MvvmManager.SetError(sourceAsControl, "")
            End If
        End If
    End Sub

    Protected Overridable Sub OnValueAssigning(e As ValueAssigningEventArgs)
        RaiseEvent ValueAssigning(Me, e)
    End Sub

    Protected Overridable Sub OnValueAssigned(e As ValueAssignedEventArgs)
        RaiseEvent ValueAssigned(Me, e)
    End Sub

    ''' <summary>
    ''' Aktualisiert alle gebundenen Steuerelemente auf Basis des aktuellen Zustand des ViewModels.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateControlsFromViewModel()
        MvvmFormsEtw.Log.Trace("CALLED: UpdateControlsFromViewModel")
        For Each item In Me.BindingItems
            For Each bindingItem In item.MvvmItem.PropertyBindings
                UpdateSourcePropertyInternal(item.Control, Me.ViewModel, bindingItem,
                                             sender:=Me,
                                             isAssigning:=True)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Ermittelt die initialen Fehler aller gebundenen Steuerelemente auf Basis des aktuellen Zustands des ViewModels
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GetInitialErrorInfo()
        MvvmFormsEtw.Log.Trace("CALLED: GetInitialErrorInfo")
        For Each item In Me.BindingItems
            For Each bindingItem In item.MvvmItem.PropertyBindings
                HandleFullPathDataErrorsChangedInternal(item.Control, Me.ViewModel, bindingItem,
                                                        sender:=Me)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Füllt alle gebundenen Steuerelemente auf Basis des aktuellen Zustands mit Nothing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateControlsWithNothing()
        MvvmFormsEtw.Log.Trace("CALLED: UpdateControlsWithNothing")

        'Fehler in der View zurücksetzen, falls INotifyDataError-Unterstützung eingeschaltet wurde.
        Dim clearErrors As Boolean = Me.MvvmManager IsNot Nothing
        For Each item In Me.BindingItems
            For Each bindingItem In item.MvvmItem.PropertyBindings
                UpdateSourcePropertyInternal(item.Control, Nothing, bindingItem,
                                             sender:=Me, isAssigning:=True)
                If clearErrors Then
                    MvvmManager.SetError(item.Control, String.Empty)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Aktualisiert das ViewModel auf Basis aller seiner gebundenen Steuerelemente.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateViewModelFromControls()
        MvvmFormsEtw.Log.Trace("CALLED: UpdateViewModelFromControls")
        For Each item In Me.BindingItems
            For Each bindingItem In item.MvvmItem.PropertyBindings
                UpdateTargetPropertyInternal(item.Control, Me.ViewModel, bindingItem, sender:=Me)
            Next
        Next
    End Sub

    Public Sub GetAllErrorsFromViewmodel()
        MvvmFormsEtw.Log.Trace("CALLED: GetAllErrorsFromViewModel")
        For Each item In Me.BindingItems
            For Each bindingItem In item.MvvmItem.PropertyBindings
                If bindingItem.BindingSetting.BindingMode.HasFlag(MvvmBindingModes.ValidatesOnNotifyDataErrors) Then
                    HandleFullPathDataErrorsChangedInternal(item.Control, Me.ViewModel,
                                                            bindingItem, sender:=Me,
                                                            IsAssigning:=True)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Aktualisiert die Eigenschaften eines Controls auf Basis des gebundenen ViewModels.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="bindingInfos"></param>
    ''' <remarks></remarks>
    Public Sub UpdateControlProperties(control As Object, bindingInfos As IEnumerable(Of PropertyBindingItem))
        For Each bindingInfoItem In bindingInfos
            UpdateSourcePropertyInternal(control, Me.ViewModel, bindingInfoItem, sender:=Me)
        Next
    End Sub

    ''' <summary>
    ''' Aktualisiert die Eigenschaften des ViewModels auf Basis der Eigenschaften eines der gebundenen Controls.
    ''' </summary>
    ''' <param name="control"></param>
    ''' <param name="bindingInfos"></param>
    ''' <remarks></remarks>
    Public Sub UpdateViewModelProperties(control As Object, bindingInfos As IEnumerable(Of PropertyBindingItem))
        For Each bindingInfoItem In bindingInfos
            UpdateTargetPropertyInternal(Me.ViewModel, control, bindingInfoItem, sender:=Me)
        Next
    End Sub

    Private Shared Function HandleBindingException(Message As String, sender As BindingManager,
                                            Optional innerException As Exception = Nothing,
                                            Optional propInfo As PropertyInfo = Nothing,
                                            Optional obj As Object = Nothing) As MvvmBindingException

        Dim fullText As String
        If innerException Is Nothing AndAlso propInfo Is Nothing AndAlso obj Is Nothing Then
            fullText = Message
        Else
            fullText = Message & vbNewLine & "Target property: " &
                         If(propInfo IsNot Nothing, propInfo.Name, "") & " - Target type: " &
                         If(obj IsNot Nothing, obj.GetType.Name, "") &
                         vbNewLine & " (" & innerException.Message & ")"
        End If

        Dim ex As New MvvmBindingException(fullText, innerException)

        MvvmFormsEtw.Log.Failure("BINDING FAILURE: " & fullText)

        If (Not ThrowExceptionOnUnassignableBindingProperties) AndAlso (Not RaiseEventOnUnassignableBindingProperties) Then
            ex.Handled = True
            Return ex
        End If

        If RaiseEventOnUnassignableBindingProperties Then
            Dim eArgs As New MvvmBindingExceptionEventArgs(ex)
            sender.OnMvvmBindingExeption(eArgs)
        End If

        If ThrowExceptionOnUnassignableBindingProperties Then
            ex.Handled = False
        Else
            ex.Handled = True
        End If
        Return ex
    End Function

    Protected Overridable Sub OnMvvmBindingExeption(e As MvvmBindingExceptionEventArgs)
        RaiseEvent MvvmBindingException(Me, e)
    End Sub

    Public Property CurrentCulture As CultureInfo = CultureInfo.CurrentCulture

    Public Shared Property ThrowExceptionOnUnassignableBindingProperties As Boolean = True
    Public Shared Property RaiseEventOnUnassignableBindingProperties As Boolean = False
    Public Shared Property TraceExceptionOnUnassignableBindingProperties As Boolean = True

    ''' <summary>
    ''' ONLY FOR DEBUGGING PURPOSES!!! Enthält die Liste aller Ereignisverknüpfungen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend ReadOnly Property SourceObject As ObjectEventsAssignments
        Get
            Return mySourceObjects
        End Get
    End Property

    ''' <summary>
    ''' Reference to the Parent-MvvmManager
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>The reference to the parent MvvmManager ist needed for the 
    ''' DataErrors able to be set. Since MvvmManager is inherited from ErrorProvider, 
    ''' the ErrorProvider's functionality can be used to indicate errors in the 
    ''' Views which have been set in the ViewModel by the INotifyDataError mechanism.</remarks>
    Public Property MvvmManager As MvvmManager

#Region "IDisposable Support"
    Private myDisposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.myDisposedValue Then
            If disposing Then
                mySupressTargetUpdating = True
                Me.ViewModel = Nothing
            End If
        End If
        Me.myDisposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

Public Class BindingItems
    Inherits KeyedCollection(Of Control, BindingItem)

    Sub New()
        MyBase.New()
    End Sub

    Sub New(list As IEnumerable(Of BindingItem))
        MyBase.New()
        For Each bItem In list
            Me.Add(bItem)
        Next
    End Sub

    Protected Overrides Function GetKeyForItem(item As BindingItem) As Control
        Return item.Control
    End Function
End Class

Public Class BindingItem
    Property Control As Control
    Property MvvmItem As MvvmBindingItem

    Public Overrides Function ToString() As String
        Return If(Control Is Nothing, "Control: - - -", Control.ToString) & "  ----  " &
               If(MvvmItem Is Nothing, "MvvmItem: - - -", MvvmItem.ToString)
    End Function
End Class

Public Enum BindingContext
    SourceToTarget
    TargetToSource
    Converter
End Enum

Public Class MvvmBindingException
    Inherits Exception

    Sub New(Message As String, InnerException As Exception)
        MyBase.New(Message, InnerException)
    End Sub

    Public Property Handled As Boolean
End Class

Public Class MvvmBindingExceptionEventArgs
    Inherits EventArgs

    Sub New()
        MyBase.New()
    End Sub

    Sub New(bindingException As MvvmBindingException)
        MyBase.New()
        Me.BindingException = bindingException
    End Sub

    Property BindingException As MvvmBindingException
End Class
