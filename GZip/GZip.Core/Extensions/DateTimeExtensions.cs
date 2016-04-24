using System;

namespace VBessonov.GZip.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(dateTime.ToUniversalTime() - epoch).TotalSeconds;
        }

        public static DateTime FromUnixTimeSeconds(long seconds)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

            dateTime = dateTime.AddSeconds(seconds).ToLocalTime();

            return dateTime;
        }
    }
}
