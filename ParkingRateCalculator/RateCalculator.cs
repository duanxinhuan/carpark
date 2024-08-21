namespace ParkingRateCalculator.Model;
// TODO: we can have an interface for CalculateRate called IRatecalculator if we want to use DI, I did not use it, so I did not create one
public static class RateCalculator
{
    public static bool CheckEarlyBird(Patron patron)
    {
        var entryTimeSpan = patron.EntryTime.TimeOfDay;
        var leaveTimeSpan = patron.ExitTime.TimeOfDay;

        bool isSameDate = patron.EntryTime.Date == patron.ExitTime.Date;
        bool isEntryTimeValid = entryTimeSpan >= RateTimeConstants.EarlyBirdEntryStart &&
               entryTimeSpan <= RateTimeConstants.EarlyBirdEntryEnd;
        bool isExitTimeValid = leaveTimeSpan >= RateTimeConstants.EarlyBirdExitStart &&
               leaveTimeSpan <= RateTimeConstants.EarlyBirdExitEnd;

        return isSameDate && isEntryTimeValid && isExitTimeValid;
    }

    public static bool CheckNight(Patron patron)
    {
        var entryTime = patron.EntryTime.TimeOfDay;
        var exitTime = patron.ExitTime.TimeOfDay;
        var isEntryInTime = entryTime >= RateTimeConstants.NightRateEntryStart &&
            entryTime <= RateTimeConstants.NightRateEntryEnd;
        var isExitInTime = exitTime <= RateTimeConstants.NightRateExitEnd;
        var dateGap = (patron.ExitTime.Date - patron.EntryTime.Date).Days;

        // threr should be only 1 day gap for night rate
        bool isExitThenSecondDay = dateGap == 1;

        return isEntryInTime && isExitInTime && isExitThenSecondDay;
    }

    public static bool CheckWeekend(Patron patron)
    {
        var entryDate = patron.EntryTime.Date;
        var exitDate = patron.ExitTime.Date;
        var dateGap = (exitDate - entryDate).Days;

        // Driver cound be enter car park at this weekend and leaving the next weekend, so check the day gap between enter and exit is neccessary
        bool isDateGapValid = dateGap >= 0 && dateGap <= 3;

        return !IsWeekday(patron.EntryTime.DayOfWeek) && !IsWeekday(patron.ExitTime.DayOfWeek) && isDateGapValid;
    }
    public static (RateType RateType, decimal Price) CalculateRate(Patron patron)
    {
        if (IsWeekday(patron.EntryTime.DayOfWeek) && CheckEarlyBird(patron))
        {
            return (RateType.EarlyBird, 13.00m);
        }

        if (CheckNight(patron) &&
            IsWeekday(patron.EntryTime.DayOfWeek))
        {
            return (RateType.NightRate, 6.50m);
        }

        if (CheckWeekend(patron))
        {
            return (RateType.WeekendRate, 10.00m);
        }

        decimal standardRate = CalculateStandardRate(patron.EntryTime, patron.ExitTime);
        return (RateType.StandardRate, standardRate);
    }

    private static bool IsWeekday(DayOfWeek day)
    {
        return day != DayOfWeek.Saturday && day != DayOfWeek.Sunday;
    }

    private static decimal CalculateStandardRate(DateTime entry, DateTime exit)
    {
        TimeSpan duration = exit - entry;
        if (duration <= RateTimeConstants.StandardRate1Hour) return 5.00m;
        if (duration <= RateTimeConstants.StandardRate2Hours) return 10.00m;
        if (duration <= RateTimeConstants.StandardRate3Hours) return 15.00m;

        // Set entry to the start of the day (00:00:00)
        var adjustedEntry = entry.Date;

        // Set exit to the end of the day (23:59:59)
        var adjustedExit = exit.Date.AddDays(1).AddTicks(-1);

        //Calculate calendar days
        TimeSpan calendaDuration = adjustedExit - adjustedEntry;
        return 20.00m * (decimal)Math.Ceiling(calendaDuration.TotalDays);
    }
}
