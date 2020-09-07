using System;

namespace WfGaming.Utils
{
    class Time
    {
        public static DateTime Origin = new DateTime(1970, 1, 1, 9, 0, 0);

        public static long GetTimestamp()
        {
            TimeSpan timestamp = DateTime.Now - Origin;
            return (long)(timestamp.TotalMilliseconds * 1000);
        }
    }
}
