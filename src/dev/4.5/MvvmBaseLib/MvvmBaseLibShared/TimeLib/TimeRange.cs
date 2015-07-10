using System;
using System.Collections.Generic;

namespace ActiveDevelop.MvvmBaseLib
{
    // <summary>
    // Stellt Funktionalitäten für Zeitbereiche 
    // und das Aufteilen und Abfragen von Zeitpunkten oder Zeitbereichen in Zeitbereichen zur Verfügung.
    // </summary>
    // 
    public class TimeRange
        : IComparable
    {

        // <summary>
        // Erstellt eine Instanz dieser Klasse aus Start- und Endzeitpunkt.
        // </summary>
        // <param name="startTime">Der Startzeitpunkt dieser Zeitspanne.</param>
        // <param name="endTime">Der Endzeitpunkt dieser Zeitspanne.</param>
        public TimeRange(DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        // <summary>
        // Bestimmt oder ermittelt den Startzeitpunkt.
        // </summary>
        public DateTimeOffset? StartTime { get; set; }

        // <summary>
        // Bestimmt oder ermittelt den Endzeitpunkt.
        // </summary>
        public DateTimeOffset? EndTime { get; set; }

        // <summary>
        // Ermittelt die Dauer dieser Zeitspanne.
        // </summary>
        public TimeSpan? TimeSpan
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        // <summary>
        // Ermittelt, ob der Endzeitpunkt vor dem Startzeitpunkt liegt.
        // </summary>
        public Boolean? IsEndTimePriorToStartTime
        {

            get
            {
                if (!EndTime.HasValue || !StartTime.HasValue)
                {
                    return null;
                }

                return EndTime < StartTime;
            }
        }

        // <summary>
        // Ermittelt, ob ein Zeitpunkt innerhalb dieser Zeitspanne liegt.
        // </summary>
        // <param name="pointOfTime">Der Zeitpunkt, der überprüft werden soll.</param>
        // <returns>Wahr, wenn der Zeitpunkt innerhalb dieser Zeitspanne gelegen hat, sonst falsch.</returns>
        public Boolean IsIn(DateTimeOffset pointOfTime)
        {
            if (!EndTime.HasValue || !StartTime.HasValue)
            {
                return false;
            }

            return (pointOfTime >= StartTime.Value && pointOfTime <= EndTime.Value);
        }

        // <summary>
        // Ermittelt eine Liste aus Zeitspannen, die sich ergeben, wenn ein Zeitpunkt eine Zeitspanne teilen soll.
        // </summary>
        // <param name="pointOfTime">Der Zeitpunkt, der die Zeitspanne teilt.</param>
        // <returns></returns>
        public List<TimeRange> InsertOrCloseSimpleEvent(DateTimeOffset pointOfTime)
        {
            var retTimeSpans = new List<TimeRange>();

            if (!StartTime.HasValue && !EndTime.HasValue)
            {
                return null;
            }

            // Closing: Endzeit wird ergänzt
            if (StartTime.HasValue && !EndTime.HasValue && pointOfTime > StartTime.Value)
            {
                retTimeSpans.Add(new TimeRange(StartTime, pointOfTime));
                return retTimeSpans;

            }

            // Closing: Startzeit wird ergänzt
            if (!StartTime.HasValue && EndTime.HasValue && pointOfTime < EndTime.Value)
            {
                retTimeSpans.Add(new TimeRange(pointOfTime, EndTime));
                return retTimeSpans;
            }


            // Ist nicht in der Zeitspanne --> ergibt nichts.
            if (!IsIn(pointOfTime))
            {
                return null;
            }

            // Zeitpunkt ist einer der Grenzwerte, dann gibt es nur ein Element zurück.
            if (pointOfTime == StartTime.Value || pointOfTime == EndTime.Value)
            {
                retTimeSpans.Add(this);
                return retTimeSpans;
            }

            // Sonst wird es in zwei Elemente aufgeteilt
            retTimeSpans.Add(new TimeRange(StartTime, pointOfTime));
            retTimeSpans.Add(new TimeRange(pointOfTime, EndTime));
            return retTimeSpans;
        }

        // <summary>
        // Ermittelt ob und in wie fern sich zwei Zeitbereiche überschneiden.
        // </summary>
        // <param name="timePeriod">Der Zeitbereich der mit diesem Zeitbereich verglichen werden soll.</param>
        // <returns></returns>
        public OverlappingSpanInfo OverlappingSpanInfo(TimeRange timePeriod)
        {
            // die zu vergleichende Zeitspanne darf nicht null sein.
            if (timePeriod == null)
            {
                throw new ArgumentException("Overlapping minutes can't be calculated if instance is null!");
            }

            // die zu vergleichende Zeitspanne darf nicht null sein.
            if (!timePeriod.StartTime.HasValue && !timePeriod.EndTime.HasValue)
            {
                throw new ArgumentException("Overlapping minutes can't be calculated if instance is null!");
            }

            // Wenn diese Instanz nur einen offnen Wert hat, kann nicht verglichen werden
            if (!StartTime.HasValue || !EndTime.HasValue)
            {
                return new OverlappingSpanInfo(null, null, TimeSpanOverlappingTypes.NotDefinable);
            }

            if (!timePeriod.StartTime.HasValue)
            {
                return new OverlappingSpanInfo(null, null, TimeSpanOverlappingTypes.OpenStart);
            }

            if (!timePeriod.EndTime.HasValue)
            {
                return new OverlappingSpanInfo(null, null, TimeSpanOverlappingTypes.OpenEnd);
            }

            if (timePeriod.IsEndTimePriorToStartTime.Value || IsEndTimePriorToStartTime.Value)
            {
                throw new ArgumentOutOfRangeException("Endtime can't be prior to Starttime!");
            }

            // Zu vergleichende Zeitspanne endet vor dieser Zeitspanne --> keine Überlappung.
            if (timePeriod.EndTime.Value <= StartTime.Value)
            {
                return new OverlappingSpanInfo(null, null,
                                               TimeSpanOverlappingTypes.EndsBefore);
            }

            // Zu vergleichende Zeitspanne beginnt nach dieser Zeitspanne --> keine Überlappung.
            if (timePeriod.StartTime.Value >= EndTime.Value)
            {
                return new OverlappingSpanInfo(null, null,
                                               TimeSpanOverlappingTypes.StartsAfter);
            }

            // Zu vergleichende Zeitspanne liegt genau in dieser Zeitspanne --> Überlappung ist also zu vergleichende Zeitspanne
            if (timePeriod.StartTime.Value >= StartTime.Value && timePeriod.EndTime.Value <= EndTime.Value)
            {
                var tmpTotalInner = timePeriod.EndTime.Value - timePeriod.StartTime.Value;
                return new OverlappingSpanInfo(tmpTotalInner,
                                               (EndTime.Value - StartTime.Value) - tmpTotalInner,
                                               TimeSpanOverlappingTypes.IsInside);
            }

            // Zu vergleichende Zeitspanne beginnt in dieser Zeitspanne, endet aber außerhalb --> Überlappung!
            if (timePeriod.StartTime.Value >= StartTime.Value && timePeriod.StartTime.Value <= EndTime.Value)
            {
                var tmpOverlapping = EndTime.Value - timePeriod.StartTime.Value;
                return new OverlappingSpanInfo(tmpOverlapping,
                                             timePeriod.EndTime.Value - EndTime.Value,
                                             TimeSpanOverlappingTypes.StartsInside);
            }

            // Zu vergleichende Zeitspanne endet in dieser Zeitspanne, beginnt aber außerhalb --> Überlappung!
            if (timePeriod.EndTime.Value <= EndTime.Value && timePeriod.EndTime.Value >= StartTime.Value)
            {
                var tmpOverlapping = timePeriod.EndTime.Value - StartTime.Value;
                return new OverlappingSpanInfo(tmpOverlapping,
                                             StartTime.Value - timePeriod.StartTime.Value,
                                             TimeSpanOverlappingTypes.EndsInside);
            }

            // zu vergleichende Zeitspanne klammert diese Zeitspanne ein
            if (timePeriod.StartTime.Value < StartTime.Value && timePeriod.EndTime.Value > EndTime.Value)
            {
                var tmpTotalInner = EndTime.Value - StartTime.Value;
                return new OverlappingSpanInfo(tmpTotalInner,
                                                   (timePeriod.EndTime.Value - timePeriod.StartTime.Value) - tmpTotalInner,
                                                   TimeSpanOverlappingTypes.IncludesCompletely);
            }

            throw new ArgumentOutOfRangeException("This case should actually never be reached! :-)");
        }

        //#Region "IComparable Member"

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(TimeSpan?) && obj.GetType() != typeof(TimeRange))
                throw new ArgumentException("Argument must be of type Nullable<System.TimeSpan> or EventTimeSpan", "obj");

            var tmpTimeSpan = new TimeSpan?();

            if (obj.GetType() == typeof(TimeSpan?))
                tmpTimeSpan = (TimeSpan?)obj;
            else
                tmpTimeSpan = ((TimeRange)obj).TimeSpan;

            if (tmpTimeSpan.HasValue && TimeSpan.HasValue)
            {
                return TimeSpan.Value.CompareTo(tmpTimeSpan.Value);
            }

            if (TimeSpan.HasValue && !tmpTimeSpan.HasValue)
                return 1;

            if (!TimeSpan.HasValue && tmpTimeSpan.HasValue)
                return -1;

            // Beide !HasValue
            return 0;
        }

        //# End Region

    }

    public class OverlappingSpanInfo
    {

        public OverlappingSpanInfo(TimeSpan? overlappingTime,
                       TimeSpan? nonOverlappingTime,
                       TimeSpanOverlappingTypes timeSpanOverlappingType)
        {
            OverlappingSpan = overlappingTime;
            NonOverlappingSpan = nonOverlappingTime;
            TimeSpanOverlappingType = timeSpanOverlappingType;

        }

        public TimeSpan? OverlappingSpan { get; set; }

        public TimeSpan? NonOverlappingSpan { get; set; }

        public TimeSpanOverlappingTypes TimeSpanOverlappingType { get; set; }

        public override string ToString()
        {
            return String.Format("Overlapping: {0}; Nonoverlapping: {1}; Type: {2}",
                                  OverlappingSpan, NonOverlappingSpan, TimeSpanOverlappingType);
        }
    }
}