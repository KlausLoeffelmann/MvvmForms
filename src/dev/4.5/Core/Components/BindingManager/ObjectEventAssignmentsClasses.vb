'*****************************************************************************************
'                                          ObjectEventAssignments
'                                          ======================
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

''' <summary>
''' Encapsulates the functionality for binding the property change events of the controls.
''' </summary>
''' <remarks>
''' DE-INFO:
''' Kapselt die Funktionalität zur Ereignisbindung der PropertyChanged-Events der Steuerelemente.
''' </remarks>
Public Class ObjectEventsAssignments
    Implements IEnumerable(Of ObjectEventsAssignment)

    Private myObjects As New Dictionary(Of Object, List(Of ObjectEvent))

    Public Function Add(eventProvidingObject As Object) As Boolean

        Dim returnValue As Boolean = True

        If myObjects.ContainsKey(eventProvidingObject) Then
            For Each item In myObjects(eventProvidingObject)
                'wenn einer in die Hose ging, ist der ganze fehlgeschlagen.
                returnValue = returnValue And UnbindEventDynamically(eventProvidingObject, item.EventName, item.EventTarget)
            Next
            myObjects.Remove(eventProvidingObject)
        End If
        myObjects.Add(eventProvidingObject, New List(Of ObjectEvent))
        Return returnValue
    End Function

    Public Function Add(eventProvidingObject As Object, objectEvent As ObjectEvent) As Boolean

        'Falls der EventRaiser bereits in der Liste ist, ...
        If myObjects.ContainsKey(eventProvidingObject) Then

            Dim prevObjectEvent As ObjectEvent

            prevObjectEvent = (From item In myObjects(eventProvidingObject)
                            Where item.EventName = objectEvent.EventName).SingleOrDefault

            '...das entsprechende Event aber noch nicht gebunden wurde...
            If prevObjectEvent Is Nothing Then

                '...dann Event binden,...
                If BindEventDynamically(eventProvidingObject, objectEvent.EventName, objectEvent.EventTarget, objectEvent.TraceInfo) Then
                    '...Event merken...
                    myObjects(eventProvidingObject).Add(objectEvent)
                    '...und fertig.
                    Return True
                End If
            Else
                'Wenn alter und neuer Delegat gleich sind, dann fertig.
                If Object.Equals(prevObjectEvent.EventTarget, objectEvent.EventTarget) Then
                    Return True
                End If

                'Falls es den EventRaiser bereits gab UND auch das Event, dann dessen Index im Array ermitteln...
                Dim objIndex = myObjects(eventProvidingObject).IndexOf(prevObjectEvent)

                '...das alte Event unbinden...
                If UnbindEventDynamically(eventProvidingObject, prevObjectEvent.EventName, prevObjectEvent.EventTarget) Then
                    '...das neue wieder binden...
                    If BindEventDynamically(eventProvidingObject, objectEvent.EventName, objectEvent.EventTarget, objectEvent.TraceInfo) Then
                        '...und alte EventInfo durch neue ersetzen.
                        myObjects(eventProvidingObject)(objIndex) = objectEvent
                        Return True
                    End If
                End If
            End If
        Else
            'Falls der EventRaiser noch GAR nicht in der Liste existierter, muss hier der neue Event gebunden werden.
            myObjects.Add(eventProvidingObject, New List(Of ObjectEvent) From {objectEvent})
            If BindEventDynamically(eventProvidingObject, objectEvent.EventName, objectEvent.EventTarget, objectEvent.TraceInfo) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function Remove(eventProvidingObject As Object, objectEventName As String) As Boolean

        Dim returnValue As Boolean

        If myObjects.ContainsKey(eventProvidingObject) Then
            Dim exObEvent = (From item In myObjects(eventProvidingObject)
                            Where item.EventName = objectEventName).SingleOrDefault
            If exObEvent IsNot Nothing Then
                UnbindEventDynamically(eventProvidingObject, objectEventName, exObEvent.EventTarget)
                returnValue = myObjects(eventProvidingObject).Remove(exObEvent)
            End If
        End If

        Return returnValue
    End Function

    Public Function Remove(eventProvidingObject As Object, objectEvent As ObjectEvent) As Boolean

        Dim returnValue As Boolean = False

        If myObjects.ContainsKey(eventProvidingObject) Then
            If myObjects(eventProvidingObject).Contains(objectEvent) Then
                UnbindEventDynamically(eventProvidingObject, objectEvent.EventName, objectEvent.EventTarget)
                returnValue = myObjects(eventProvidingObject).Remove(objectEvent)
            End If
        End If

        Return returnValue

    End Function

    Private Function BindEventDynamically(eventProvidingObject As Object, eventName As String, EventProc As [Delegate],
                                          Optional traceInfo As String = Nothing) As Boolean
        If traceInfo IsNot Nothing Then
            TraceEx.TraceInformation(traceInfo)
        End If

        If eventProvidingObject Is Nothing Then
            Throw New NullReferenceException("Die übergebene Steuerelement-Variable ist null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim eventInfo = eventProvidingObject.GetType.GetEvent(eventName)
        If eventInfo Is Nothing Then
            TraceEx.TraceInformation("FAILED: Binding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'. EventInfo failed to be retrieved.")
            Return False
        End If

        Try
            'Dynamisch den richtigen Typen basteln:
            If eventInfo.EventHandlerType.IsGenericType Then
                If eventInfo.EventHandlerType Is GetType(EventHandler(Of )) Then
                    eventInfo.AddEventHandler(eventProvidingObject, EventProc)
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
                eventInfo.AddEventHandler(eventProvidingObject, eventDelegate)
            End If
            TraceEx.TraceInformation("SUCCEEDED: Binding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'.")
        Catch ex As Exception
            TraceEx.TraceInformation("FAILED: Binding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'. " & ex.Message)
            Return False
        End Try
        Return True

    End Function

    Private Function UnbindEventDynamically(eventProvidingObject As Object, eventName As String, EventProc As [Delegate],
                                            Optional traceInfo As String = Nothing) As Boolean

        If traceInfo IsNot Nothing Then
            TraceEx.TraceInformation(traceInfo)
        End If

        If eventProvidingObject Is Nothing Then
            Throw New NullReferenceException("The control variable null (Nothing in VB). Stellen Sie sicher, dass sie auf eine gültige Instanz zeigt.")
        End If

        Dim eventInfo = eventProvidingObject.GetType.GetEvent(eventName)
        If eventInfo Is Nothing Then
            TraceEx.TraceInformation("FAILED: Unbinding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'. EventInfo failed to be retrieved.")
            Return False
        End If

        Try
            'Dynamisch den richtigen Typen basteln:
            If eventInfo.EventHandlerType.IsGenericType Then
                If eventInfo.EventHandlerType Is GetType(EventHandler(Of )) Then
                    eventInfo.AddEventHandler(eventProvidingObject, EventProc)
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
                eventInfo.RemoveEventHandler(eventProvidingObject, eventDelegate)
            End If
            TraceEx.TraceInformation("SUCCEEDED: Unbinding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'.")
        Catch ex As Exception
            TraceEx.TraceInformation("FAILED: Unbinding event '" & eventName & "' for instance of type '" & eventProvidingObject.GetType.Name & "'. " & ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ObjectEventsAssignment) Implements System.Collections.Generic.IEnumerable(Of ObjectEventsAssignment).GetEnumerator
        'Not needed
        Throw New NotImplementedException("GetEnumerator is not supposed to be used.")
    End Function

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        'Not needed
        Throw New NotImplementedException("GetEnumerator is not supposed to be used.")
    End Function
End Class

Public Class ObjectEventsAssignment
    Property EventProvidingObject As Object
    Property ObjectEvents As List(Of ObjectEvent)
End Class

Public Class ObjectEvent
    Property EventName As String
    Property EventTarget As [Delegate]
    Property TraceInfo As String
End Class

Public Class ObjectEventAssignment
    Property EventProvidingObject As Object
    Property ObjectEvent As ObjectEvent
End Class
