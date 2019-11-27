using System;
using System.Collections.Generic;
using System.Text;

namespace Super.Core.Infrastruct.Extensions
{
    public static class TimeExtensions
    {
        public static DateTime ToDatetime(this long unixTime)
        {
            DateTime start = new DateTime(1970, 1, 1);
            DateTime dt = start.AddMilliseconds(unixTime);
            return dt;
        }

        public static long ToLong(this DateTime dt)
        {
            long unixTime = (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1,0,0,0, DateTimeKind.Utc)).TotalMilliseconds;
            return unixTime;
        }

        public static int ToInt(this DateTime dt)
        {
            return (int)(dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

    }
}
