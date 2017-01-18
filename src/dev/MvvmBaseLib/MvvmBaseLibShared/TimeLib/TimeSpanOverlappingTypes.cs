namespace ActiveDevelop.MvvmBaseLib
{
    /// <summary>
    /// Overlappung modes for comparing TimeSpans with start and end point.
    /// </summary>
    public enum TimeSpanOverlappingTypes {
        /// <summary>
        /// Zeitspanne ist nicht definiert, da sie keinen Start- und keinen Endzeitpunkt hat.
        /// </summary>
        NotDefinable,

        /// <summary>
        /// Liegt außerhalb - endet vorher.
        /// </summary>
        EndsBefore,

        /// <summary>
        /// Liegt außerhalb - startet anschließend.
        /// </summary>
        StartsAfter,

        /// <summary>
        /// Die verglichene Zeitspanne umhüllt komplett die vergleichenden Zeitspanne. (Gegenteil von IsInside)
        /// </summary>
        IncludesCompletely,

        /// <summary>
        /// Die Zeitspanne befindet sich komplett innerhalb der vergleichenden. (Gegenteil von IncludesCompletely)
        /// </summary>
        IsInside,

        /// <summary>
        /// Die Zeitspanne beginnt außerhalb endet aber innerhalb der vergleichenden.
        /// </summary>
        EndsInside,

        /// <summary>
        /// Die Zeitspanne beginnt innen endet aber außerhalb der vergleichenden.
        /// </summary>
        StartsInside,

        /// <summary>
        /// Die Zeitspanne hat keinen Startzeitpunkt und kann nicht auf Überschneidung vergleichen werden.
        /// </summary>
        OpenStart,

        /// <summary>
        /// Die Zeitspanne hat keinen Endzeitpunkt und kann nicht auf Überschneidung vergleichen werden.
        /// </summary>
        OpenEnd,
    }
}
