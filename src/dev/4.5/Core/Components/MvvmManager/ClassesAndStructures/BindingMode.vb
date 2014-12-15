<Serializable>
Public Structure BindingSetting

    Sub New(bindingMode As MvvmBindingModes, sourceTrigger As UpdateSourceTriggerSettings)
        Me.BindingMode = bindingMode
        Me.UpdateSourceTrigger = sourceTrigger
    End Sub

    Sub New(bindingMode As Integer, sourceTrigger As Integer)
        Me.BindingMode = CType(bindingMode, MvvmBindingModes)
        Me.UpdateSourceTrigger = CType(sourceTrigger, UpdateSourceTriggerSettings)
    End Sub

    Property BindingMode As MvvmBindingModes

    Property UpdateSourceTrigger As UpdateSourceTriggerSettings

    Public Overrides Function ToString() As String
        Return BindingMode.ToString & " on " & UpdateSourceTrigger.ToString
    End Function
End Structure

<Flags>
Public Enum MvvmBindingModes
    NotSet = 0
    TwoWay = 1
    OneWay = 2
    OneTime = 4
    OneWayToSource = 8
    ValidatesOnNotifyDataErrors = 32768
End Enum

Public Enum UpdateSourceTriggerSettings
    LostFocus
    PropertyChangedImmediately
    PropertyChangedShortlyDelayed
    PropertyChangedMediumDelayed
    PropertyChangedNoticablyDelayed
    Explicit
End Enum
