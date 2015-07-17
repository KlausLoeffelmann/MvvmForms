Imports System.ComponentModel

<ToolboxItem(False)>
Public Class UpDownButton
    Inherits MultiPurposeButtonBase

    Protected Overrides Function GetButtonType() As UpDownComboButtonType
        Return UpDownComboButtonType.SpinUpDownCombined
    End Function
End Class

<ToolboxItem(False)>
Public Class ComboButton
    Inherits MultiPurposeButtonBase

    Protected Overrides Function GetButtonType() As UpDownComboButtonType
        Return UpDownComboButtonType.Combo
    End Function
End Class

<ToolboxItem(False)>
Public Class SimpleButton
    Inherits MultiPurposeButtonBase

    Protected Overrides Function GetButtonType() As UpDownComboButtonType
        Return UpDownComboButtonType.SimpleButton
    End Function
End Class