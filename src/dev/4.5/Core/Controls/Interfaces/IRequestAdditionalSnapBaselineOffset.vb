''' <summary>
''' Für die Designer-Infrastruktur für von 
''' <see cref="NullableValueBase(Of NullableType, ControlType)">NullableValueBase(Of NullableType, ControlType)</see> abgeleitete Steuerelemente.
''' </summary>
''' <remarks>Schnittstelle, die eine Anforderung definiert, bei der ein zusätzlicher Offset (von oben)
''' für das Platzieren der Base-Snap-Line benötigt wird, da das Steuerelement ein konstituierendes
''' Steuerelement ist, und sich die Offsets nach oben verschieben.</remarks>
Public Interface IRequestAdditionalSnapBaselineOffset
    Function AdditionalSnapBaselineOffset() As Integer
End Interface
