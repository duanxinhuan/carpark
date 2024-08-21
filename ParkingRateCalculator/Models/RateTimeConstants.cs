namespace ParkingRateCalculator.Model;
public static class RateTimeConstants
{
    public static readonly TimeSpan EarlyBirdEntryStart = new TimeSpan(6, 0, 0);
    public static readonly TimeSpan EarlyBirdEntryEnd = new TimeSpan(9, 0, 0);
    public static readonly TimeSpan EarlyBirdExitStart = new TimeSpan(15, 30, 0);
    public static readonly TimeSpan EarlyBirdExitEnd = new TimeSpan(23, 30, 0);

    public static readonly TimeSpan NightRateEntryStart = new TimeSpan(18, 0, 0);
    public static readonly TimeSpan NightRateEntryEnd = new TimeSpan(23, 59, 59);
    public static readonly TimeSpan NightRateExitEnd = new TimeSpan(6, 0, 0);

    public static readonly TimeSpan StandardRate1Hour = new TimeSpan(1, 0, 0);
    public static readonly TimeSpan StandardRate2Hours = new TimeSpan(2, 0, 0);
    public static readonly TimeSpan StandardRate3Hours = new TimeSpan(3, 0, 0);
}
