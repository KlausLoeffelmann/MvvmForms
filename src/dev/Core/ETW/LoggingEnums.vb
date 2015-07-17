<Flags>
Public Enum LoggingModes
    None = 0
    Etw = 1
    OutputWindow = 2
    EtwAndOutputWindow = 3
End Enum

<Flags>
Public Enum LoggingSettings
    None = 0
    Diagnostic = 1
    Information = 2
    Verbose = 4
    BindingSetupInfo = 8
    RuntimeBindingInfo = 16
    ControlChangeStateInfo = 32
    ControlTracing = 64
    [Default] = ControlTracing Or LoggingSettings.ControlChangeStateInfo Or
                LoggingSettings.RuntimeBindingInfo Or LoggingSettings.BindingSetupInfo Or
                LoggingSettings.Information Or LoggingSettings.Diagnostic
End Enum
