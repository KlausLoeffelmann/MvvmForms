Imports System.Runtime.CompilerServices
Imports System.Drawing
Imports System.Windows.Forms

Public Module GroupFunctionalityExtender

    ''' <summary>
    ''' Versucht die Eigenschaft eines bestimmten Steuerelementes dynamisch zu setzen.
    ''' </summary>
    ''' <typeparam name="valueType">Typ des neuen Wertes der Eigenschaft.</typeparam>
    ''' <param name="control">Steuerelement, dessen Eigenschaft gesetzt werden soll.</param>
    ''' <param name="PropertyName">Name der Eigenschaft als Zeichenkette.</param>
    ''' <param name="value">Neuer Wert der Eigenschaft.</param>
    ''' <returns>True, falls die Methode erfolgreich ausgeführt werden konnte.</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TrySetProperty(Of valueType)(ByVal control As IAssignableFormToBusinessClassManager, PropertyName As String, value As valueType) As Boolean
        If control Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim propInfo = control.GetType.GetProperty(PropertyName)
        If propInfo Is Nothing Then
            Return False
        End If

        Try
            propInfo.SetValue(control, value, Nothing)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Versucht die Eigenschaft, die über einen Integer-Parameter (Indexer) verfügt, eines bestimmten Steuerelementes dynamisch zu setzen.
    ''' </summary>
    ''' <typeparam name="valueType">Typ des neuen Wertes der Eigenschaft.</typeparam>
    ''' <param name="control">Steuerelement, dessen Eigenschaft gesetzt werden soll.</param>
    ''' <param name="PropertyName">Name der Eigenschaft als Zeichenkette.</param>
    ''' <param name="value">Neuer Wert der Eigenschaft.</param>
    ''' <param name="Index">Der Wert für den Index der Eigenschaft.</param>
    ''' <returns>True, falls die Methode erfolgreich ausgeführt werden konnte.</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TrySetProperty(Of valueType)(ByVal control As IAssignableFormToBusinessClassManager, PropertyName As String, value As valueType, Index As Integer) As Boolean
        If control Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim propInfo = control.GetType.GetProperty(PropertyName)
        If propInfo Is Nothing Then
            Return False
        End If

        Try
            propInfo.SetValue(control, value, New Object() {Index})
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Versucht die Eigenschaften mit dem angegebenen Namen aller Steuerelement in der angegebenen Gruppe auf den entsprechenden Wert zu setzen.
    ''' </summary>
    ''' <typeparam name="valueType">Typ des neuen Wertes der Eigenschaft.</typeparam>
    ''' <param name="containerControl">Container-Steuerelement, dass die Steuerelemente der angegebenen Gruppe enthält.</param>
    ''' <param name="GroupName">Name der Gruppe der Steuerelemente, deren Eigenschaft auf denselben Wert gesetzt werden soll.</param>
    ''' <param name="PropertyName">Name der Eigenschaft, der für alle Steuerelemente der Gruppe einen neuen Wert erhalten soll.</param>
    ''' <param name="value">Wert, der den Eigenschaften der Steuerelemente der Gruppe neu zugewiesen werden soll.</param>
    ''' <returns>True, falls die Methode erfolgreich ausgeführt werden konnte.</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TrySetProperties(Of valueType)(containerControl As ContainerControl, GroupName As String, PropertyName As String, value As valueType) As Boolean
        For Each item In GetGroupableControls(containerControl, GroupName)
            Try
                item.TrySetProperty(PropertyName, value)
            Catch ex As Exception
                Return False
            End Try
        Next
        Return True
    End Function

    ''' <summary>
    ''' Ermittelt alle Steuerelemente in einem ContainerControl, die über die entsprechende Eigenschaften und dort den angegebenen Wert verfügen.
    ''' </summary>
    ''' <typeparam name="valuetype"></typeparam>
    ''' <param name="containerControl"></param>
    ''' <param name="PropertyName"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function GetGroupableControlsByPropertyValue(Of valuetype)(containerControl As ContainerControl, PropertyName As String, value As valuetype) As List(Of Control)

        Dim retControlList As New List(Of Control)

        For Each item As Control In GetGroupableControls(containerControl)

            Dim propInfo = item.GetType.GetProperty(PropertyName)
            If propInfo IsNot Nothing Then
                'Hat die Eigenschaft - jetzt testen wir den Wert:
                Dim valueOfProperty = propInfo.GetValue(item, Nothing)
                If valueOfProperty.Equals(value) Then
                    retControlList.Add(item)
                End If
            End If
        Next

        Return retControlList
    End Function

    ''' <summary>
    ''' Versucht das Ereignis eines Steuerelementes dynamisch an eine Ereignisbehandlungsroutine zu binden.
    ''' </summary>
    ''' <typeparam name="eventArgsType"></typeparam>
    ''' <param name="control">Steuerelement, dass das Ereignis auslöst, an das gebunden werden soll.</param>
    ''' <param name="EventName">Name des Ereignisses als Zeichenkette.</param>
    ''' <param name="EventProc">EventHandler auf die Methode mit entsprechender Signatur, die das Ereignis behandelt.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TryBindEvent(Of eventArgsType As EventArgs)(ByVal control As Control,
                                                                    EventName As String,
                                                                    EventProc As EventHandler(Of eventArgsType)) As Boolean

        If control Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim eventInfo = control.GetType.GetEvent(EventName)
        If eventInfo Is Nothing Then
            Return False
        End If

        Try
            'Dynamisch den richtigen Typen basteln:
            If eventInfo.EventHandlerType.IsGenericType Then
                If eventInfo.EventHandlerType Is GetType(EventHandler(Of )) Then
                    eventInfo.AddEventHandler(control, EventProc)
                Else
                    Throw New NotSupportedException("Dieser generische Event-Handler ist für dynamisches Binden nicht unterstützt. Bitte verwenden Sie nur 'EventHandler<t:eventargs>")
                End If
            Else
                'Jetzt wird's lustig: Wir müssen einen neuen EventHandler-Delegaten definieren und die übergebene Methode umbiegen.
                Dim eventDelegate As [Delegate]

                'Nur statische Methoden können direkt gebunden werden. Für Instanz-Methoden...
                If EventProc.Method.IsStatic Then
                    eventDelegate = [Delegate].CreateDelegate(eventInfo.EventHandlerType, EventProc.Method)
                Else
                    '...benötigen wir die Instant der Methode, und die steht in der Target-Eigenschaft des Delegaten.
                    eventDelegate = [Delegate].CreateDelegate(eventInfo.EventHandlerType, EventProc.Target, EventProc.Method)
                End If
                eventInfo.AddEventHandler(control, eventDelegate)
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Versucht das als Zeichenkette angegebene Ereignis aller Steuerelemente einer Gruppe im Container-Steuerelement an eine Ereignisbehandlungsmethode zu binden.
    ''' </summary>
    ''' <typeparam name="eventArgsType">Typ der EventArgs-Parameter, durch die die genaue Signatur der Ereignisbehandlungsmethode bestimmt werden kann.</typeparam>
    ''' <param name="containerControl"></param>
    ''' <param name="GroupName">Bestimmt die Gruppe der Steuerelemente, für die die Ereignisbindung eingerichtet werden soll.</param>
    ''' <param name="Eventname">Name des Ereignisses als Zeichenkette, dessen Ereignisbehandlung eingerichtet werden soll.</param>
    ''' <param name="EventProc">Delegat der Methode, die das Ereignis behandelt.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TryBindEvents(Of eventArgsType As EventArgs)(ByVal containerControl As ContainerControl,
                                                        GroupName As String,
                                                        Eventname As String,
                                                        EventProc As EventHandler(Of eventArgsType)) As Boolean

        For Each item As Control In GetGroupableControls(containerControl, GroupName)
            Try
                item.TryBindEvent(Eventname, EventProc)
            Catch ex As Exception
                Return False
            End Try
        Next
        Return True
    End Function

    ''' <summary>
    ''' Hebt die Bindung eines Ereignisses, die durch <see cref="TryBindEvent">TryBindEvent</see> erfolgte, wieder auf.
    ''' </summary>
    ''' <typeparam name="eventArgsType"></typeparam>
    ''' <param name="control">Steuerelement, dass das Ereignis auslöst, von dem die Ereignisbehandlungsmethode gelöst werden soll.</param>
    ''' <param name="EventName">Name des Ereignisses als Zeichenkette.</param>
    ''' <param name="EventProc">EventHandler auf die Methode mit entsprechender Signatur, die das Ereignis behandelt.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TryUnbindEvent(Of eventArgsType As EventArgs)(ByVal control As Control,
                                                                    EventName As String,
                                                                    EventProc As EventHandler(Of eventArgsType)) As Boolean
        If control Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim eventInfo = control.GetType.GetEvent(EventName)
        If eventInfo Is Nothing Then
            Return False
        End If

        Try
            'Dynamisch den richtigen Typen basteln:
            If eventInfo.EventHandlerType.IsGenericType Then
                If eventInfo.EventHandlerType Is GetType(EventHandler(Of )) Then
                    eventInfo.AddEventHandler(control, EventProc)
                Else
                    Throw New NotSupportedException("Dieser generische Event-Handler ist für dynamisches Binden nicht unterstützt. Bitte verwenden Sie nur 'EventHandler<t:eventargs>")
                End If
            Else
                'Selbe wir bei TryBind-Event, nur mit RemoveEventHandler im Ergebnis.
                Dim eventDelegate As [Delegate]
                If EventProc.Method.IsStatic Then
                    eventDelegate = [Delegate].CreateDelegate(eventInfo.EventHandlerType, EventProc.Method)
                Else
                    eventDelegate = [Delegate].CreateDelegate(eventInfo.EventHandlerType, EventProc.Target, EventProc.Method)
                End If
                eventInfo.RemoveEventHandler(control, eventDelegate)
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Hebt die Bindung eines Ereignisses, die durch <see cref="TryBindEvent">TryBindEvent</see> erfolgte, wieder auf.
    ''' </summary>
    ''' <param name="control">Steuerelement, dass das Ereignis auslöst, von dem die Ereignisbehandlungsmethode gelöst werden soll.</param>
    ''' <param name="EventName">Name des Ereignisses als Zeichenkette.</param>
    ''' <param name="EventProc">EventHandler auf die Methode mit entsprechender Signatur, die das Ereignis behandelt.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TryUnbindEvent(ByVal control As Object,
                                   EventName As String,
                                   EventProc As [Delegate]) As Boolean
        If control Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim eventInfo = control.GetType.GetEvent(EventName)
        If eventInfo Is Nothing Then
            Return False
        End If

        Try
            eventInfo.RemoveEventHandler(control, EventProc)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Hebt die Bindung des Ereignisses aller Steuerelemente im Container-Steuerelement wieder auf.
    ''' </summary>
    ''' <typeparam name="eventArgsType"></typeparam>
    ''' <param name="containerControl"></param>
    ''' <param name="GroupName">Bestimmt die Gruppe der Steuerelemente, für die die Ereignisbindung aufgehoben werden soll.</param>
    ''' <param name="Eventname">Name des Ereignisses als Zeichenkette, dessen Ereignisbehandlung aufgehoben werden soll.</param>
    ''' <param name="EventProc">Delegat der Methode, die das Ereignis behandelt.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function TryUnbindEvents(Of eventArgsType As EventArgs)(ByVal containerControl As ContainerControl,
                                                        GroupName As String,
                                                        Eventname As String,
                                                        EventProc As EventHandler(Of eventArgsType)) As Boolean

        For Each item In GetGroupableControls(containerControl, GroupName)
            Try
                item.TryUnbindEvent(Eventname, EventProc)
            Catch ex As Exception
                Return False
            End Try
        Next
        Return True
    End Function

    ''' <summary>
    ''' Ermittelt eine Liste aller NullableValueControls auf dem ContainerControl.
    ''' </summary>
    ''' <param name="containerControl">ContainerControl, das die NullableValueControls enthält.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function GetGroupableControls(containerControl As ContainerControl) As List(Of IAssignableFormToBusinessClassManager)
        Return FromContainerControlInternal(New List(Of IAssignableFormToBusinessClassManager), Nothing, containerControl)
    End Function


    ''' <summary>
    ''' Ermittelt eine Liste der NullableValueControls auf dem ContainerControl, die über ihre GroupName-Eigenschaft der entsprechenden Gruppierung zugeordnet sind.
    ''' </summary>
    ''' <param name="containerControl">ContainerControl, das die NullableValueControls enthält.</param>
    ''' <param name="GroupName">Name der Gruppe der Steuerelemente, die im Container-Steuerelement ermittelt werden sollen.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function GetGroupableControls(containerControl As ContainerControl, GroupName As String) As List(Of IAssignableFormToBusinessClassManager)
        Return FromContainerControlInternal(New List(Of IAssignableFormToBusinessClassManager), New List(Of String) From {GroupName}, containerControl)
    End Function

    ''' <summary>
    ''' Ermittelt eine Liste der NullableValueControls auf dem ContainerControl, die über ihre GroupName-Eigenschaft den angegebenen Gruppierungen zugeordnet sind.
    ''' </summary>
    ''' <param name="containerControl">ContainerControl, das die NullableValueControls enthält.</param>
    ''' <param name="GroupNames">Namen der Gruppen der Steuerelemente, die im Container-Steuerelement ermittelt werden sollen.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function GetGroupableControls(containerControl As ContainerControl, GroupNames As List(Of String)) As List(Of IAssignableFormToBusinessClassManager)
        Return FromContainerControlInternal(New List(Of IAssignableFormToBusinessClassManager), GroupNames, containerControl)
    End Function


    Private Function FromContainerControlInternal(ByVal assignableControls As List(Of IAssignableFormToBusinessClassManager), GroupNames As List(Of String),
                                                  ByVal cControl As Control) As List(Of IAssignableFormToBusinessClassManager)

        For Each c As Control In cControl.Controls
            If GetType(IAssignableFormToBusinessClassManager).IsAssignableFrom(c.GetType) Then
                If GroupNames Is Nothing OrElse GroupNames.Count = 0 OrElse GroupNames.Contains(DirectCast(c, IAssignableFormToBusinessClassManager).GroupName) Then
                    assignableControls.Add(DirectCast(c, IAssignableFormToBusinessClassManager))
                End If
            End If
            If c.HasChildren Then
                FromContainerControlInternal(assignableControls, GroupNames, DirectCast(c, Control))
            End If
        Next
        Return assignableControls
    End Function
End Module
