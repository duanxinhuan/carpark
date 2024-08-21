using System;
using ParkingRateCalculator.Model;
using Xunit;

namespace ParkingRateCalculator.Tests
{
    public class RateCalculatorTests
    {
        [Fact]
        public void CalculateRate_ShouldReturnEarlyBirdRate_WhenEntryAndExitTimesAreValid()
        {
            var patron = new Patron
            {
                EntryTime = new DateTime(2024, 8, 21, 7, 0, 0),  // 7:00 AM
                ExitTime = new DateTime(2024, 8, 21, 16, 0, 0)   // 4:00 PM
            };

            var (rateType, price) = RateCalculator.CalculateRate(patron);

            Assert.Equal(RateType.EarlyBird, rateType);
            Assert.Equal(13.00m, price);
        }

        [Fact]
        public void CalculateRate_ShouldReturnNightRate_WhenEntryAndExitTimesAreValid()
        {
            var patron = new Patron
            {
                EntryTime = new DateTime(2024, 8, 21, 19, 0, 0),  // 7:00 PM
                ExitTime = new DateTime(2024, 8, 22, 5, 30, 0)   // 5:30 AM (next day)
            };

            var (rateType, price) = RateCalculator.CalculateRate(patron);

            Assert.Equal(RateType.NightRate, rateType);
            Assert.Equal(6.50m, price);
        }

        [Fact]
        public void CalculateRate_ShouldReturnWeekendRate_WhenEntryAndExitDaysAreWeekend()
        {
            var patron = new Patron
            {
                EntryTime = new DateTime(2024, 8, 24, 10, 0, 0),  // Saturday
                ExitTime = new DateTime(2024, 8, 25, 12, 0, 0)   // Sunday
            };

            var (rateType, price) = RateCalculator.CalculateRate(patron);

            Assert.Equal(RateType.WeekendRate, rateType);
            Assert.Equal(10.00m, price);
        }

        [Fact]
        public void CalculateRate_ShouldReturnStandardRate_WhenNoOtherRateApplies()
        {
            var patron = new Patron
            {
                EntryTime = new DateTime(2024, 8, 21, 14, 0, 0),  // 2:00 PM
                ExitTime = new DateTime(2024, 8, 21, 16, 0, 0)   // 4:00 PM
            };

            var (rateType, price) = RateCalculator.CalculateRate(patron);

            Assert.Equal(RateType.StandardRate, rateType);
            Assert.Equal(10.00m, price);
        }

        [Fact]
        public void CalculateRate_ShouldReturnStandardRate_WhenEntryAndExitTimesExceedWeekendLimit()
        {
            var patron = new Patron
            {
                EntryTime = new DateTime(2024, 8, 24, 8, 0, 0),   // Saturday
                ExitTime = new DateTime(2024, 8, 28, 10, 0, 0)   // Wednesday
            };

            var (rateType, price) = RateCalculator.CalculateRate(patron);

            Assert.Equal(RateType.StandardRate, rateType);
            Assert.Equal(100.00m, price); // 5 calendar days, 100.0m
        }
    }
}
