using System;

namespace LuaBridge.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            return (long)dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
