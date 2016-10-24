using System;
using OpenEhr.Attributes;

namespace OpenEhr.AssumedTypes
{
    /// <summary>
    /// Definitions for date/time classes. 
    /// </summary>
    [Serializable]
    [RmType("openEHR", "SUPPORT", "TIME_DEFINITIONS")]
    public class TimeDefinitions
    {
        public const int secondsInMinute = 60;
        public const int minutesInHour = 60;
        public const int hoursInDay = 24;
        public const float nominalDaysInMonth = 30.42f;
        public const int maxDaysInMonth = 31;
        public const int daysInYear = 365;
        public const float nominalDaysInYear = 365.24f;
        public const int daysInLeapYear = 366;
        public const int maxDaysInYear = daysInLeapYear;
        public const int daysInWeek = 7;
        public const int monthsInYear = 12;

        /// <summary>
        /// True is the given year digit is greater than 0.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool ValidYear(int y)
        {
            //return y > 0;
            return y >= 0;
        }
        /// <summary>
        /// True if the given month digit is greater than or equals to 1, 
        /// and less than or equals to 12.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool ValidMonth(int m)
        {
            return m >= 1 && m <= monthsInYear;
        }

        /// <summary>
        /// True if d>=1 and d <= daysInMonth(m, y)
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool ValidDay(int y, int m, int d)
        {
            int daysInMonth = DateTime.DaysInMonth(y, m);
            return d >= 1 && d <= daysInMonth;

        }
        /// <summary>
        /// True if (h>=0 and h<hoursInDay) or(h=hoursInDays and m=0 and s=0)
        /// </summary>
        /// <param name="h"></param>
        /// <param name="m"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValidHour(int h, int m, int s)
        {
            if ((h >= 0 && h < hoursInDay) || (h == hoursInDay && m == 0 && s == 0))
                return true;
            else
                return false;

        }
        /// <summary>
        /// True if m>=0 and m<MinutesInHour
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool ValidMinute(int m)
        {
            return m >= 0 && m < minutesInHour;
        }
        /// <summary>
        /// True if s >=0 and s less than seconds in minute.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValidSecond(int s)
        {
            return s >= 0 && s < secondsInMinute;
        }
        /// <summary>
        /// True if fs>=0.0 and fs less than 1.0
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static bool ValidFractionalSecond(double fs)
        {
            return fs >= 0.0 && fs < 1.0;
        }

        protected int GetTotalDaysOfYear(int year)
        {
            DesignByContract.Check.Require(year > 0, "year must be greater than 0");

            int numberOf400s = year / 400;
            int numberOf100s = year / 100;
            int numberOf4s = year / 4;

            int numberOfleapYears = numberOf4s - (numberOf100s - numberOf400s);

            return year * 365 + numberOfleapYears;
        }

        protected int GetTotalDaysOfMonthInAYear(int year, int month)
        {
            DesignByContract.Check.Require(month > 0, "month must be greater than 0");
            int totalDays = 0;
            int index = 1;
            while (index < month)
            {
                totalDays += DateTime.DaysInMonth(year, index);
                index++;
            }

            return totalDays;
        }

    }
}
