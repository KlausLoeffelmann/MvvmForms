Imports System.Diagnostics.Tracing

<EventSource(Name:="MvvmFormsCore")>
Public Class MvvmFormsEtw
    Inherits EventSource

    Sub New()
        MyBase.New
        LoggingMode = LoggingModes.EtwAndOutputWindow
    End Sub

    Private myLoggingMode As LoggingModes
    Private myOutputWindowLoggingSuspended As Boolean
    Private myLoggingAction As Action(Of Integer, String)

    Public Class Keywords
        Public Const ControlStateChange As EventKeywords = CType(1, EventKeywords)
        Public Const ControlMisc As EventKeywords = CType(2, EventKeywords)
        Public Const MvvmInitializing As EventKeywords = CType(4, EventKeywords)
        Public Const MvvmRuntimeBinding As EventKeywords = CType(8, EventKeywords)
        Public Const Diagnostic As EventKeywords = CType(128, EventKeywords)
        Public Const Perf As EventKeywords = CType(256, EventKeywords)
        Public Const General As EventKeywords = CType(512, EventKeywords)
    End Class

    Public Class Tasks
        Public Const ControlEnteredState As EventTask = CType(1, EventTask)
        Public Const ControlLeftState As EventTask = CType(2, EventTask)
        Public Const MvvmBindingInitialize As EventTask = CType(4, EventTask)
        Public Const MvvmAssignControlValue As EventTask = CType(8, EventTask)
        Public Const MvvmAssignViewmodelValue As EventTask = CType(16, EventTask)
    End Class 'Tasks

    <[Event](1, Message:="SERIOUS FAILURE: {0}", Level:=EventLevel.Error, Keywords:=Keywords.Diagnostic)>
    Public Sub Failure(ByVal message As String)
        myLoggingAction.Invoke(1, message)
    End Sub

    <[Event](2, Message:="TRACING: {0}", Keywords:=Keywords.Perf, Level:=EventLevel.Verbose)>
    Public Sub Trace(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(2, message)
        End If
    End Sub

    <[Event](3, Message:="INFO: {0}", Keywords:=Keywords.Perf, Level:=EventLevel.Informational)>
    Public Sub Info(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(3, message)
        End If
    End Sub

    <[Event](4, Message:="MVVM: {0}", Keywords:=Keywords.MvvmRuntimeBinding,
             Task:=Tasks.MvvmAssignControlValue, Level:=EventLevel.Verbose)>
    Public Sub ControlBindingInfo(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(4, message)
        End If
    End Sub

    <[Event](5, Message:="MVVM: {0}", Keywords:=Keywords.MvvmRuntimeBinding,
             Task:=Tasks.MvvmAssignViewmodelValue, Level:=EventLevel.Verbose)>
    Public Sub ViewModelBindingInfo(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(5, message)
        End If
    End Sub


    <[Event](6, Message:="MVVM: {0}", Keywords:=Keywords.MvvmInitializing, Level:=EventLevel.Informational)>
    Public Sub BindingSetup(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(6, message)
        End If
    End Sub

    <[Event](7, Message:="CONTROLS: {0}", Keywords:=Keywords.ControlStateChange,
             Task:=Tasks.ControlEnteredState, Level:=EventLevel.Verbose)>
    Public Sub ControlEnterState(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(7, message)
        End If
    End Sub

    <[Event](8, Message:="CONTROLS: {0}", Keywords:=Keywords.ControlStateChange,
             Task:=Tasks.ControlLeftState, Level:=EventLevel.Verbose)>
    Public Sub ControlLeaveState(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(8, message)
        End If
    End Sub

    <[Event](9, Message:="CONTROLS: {0}", Keywords:=Keywords.ControlMisc, Level:=EventLevel.Verbose)>
    Public Sub ControlTrace(message As String)
        If IsEnabled() Then
            myLoggingAction.Invoke(9, message)
        End If
    End Sub

    Public Shared Log As New MvvmFormsEtw

    Public Property LoggingMode As LoggingModes
        Get
            Return myLoggingMode
        End Get
        Set(value As LoggingModes)
            If Object.Equals(value, myLoggingMode) Then
                myLoggingAction = Sub(ID, message)
                                  End Sub

                If value = LoggingModes.ETW Then
                    myLoggingAction =
                        Sub(ID, message)
                            WriteEvent(ID, message)
                        End Sub
                End If

                If value = LoggingModes.OutputWindow Then
                    myLoggingAction =
                        Sub(ID, message)
                            If Not myOutputWindowLoggingSuspended Then
                                Console.WriteLine(DateTime.Now.ToString("dd. HH:mm:ss.fff " & ": " & message))
                            End If
                        End Sub
                End If

                If value = LoggingModes.OutputWindow + LoggingModes.ETW Then
                    myLoggingAction =
                        Sub(ID, message)
                            If Not myOutputWindowLoggingSuspended Then
                                WriteEvent(ID, message)
                                Console.WriteLine(DateTime.Now.ToString("dd. HH:mm:ss.fff " & ": " & message))
                            End If
                        End Sub
                End If
            End If
        End Set
    End Property
End Class
