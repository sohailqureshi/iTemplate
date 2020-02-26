using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTemplate.Library
{
  public class Humanize
  {
    public static string TimeSpan(DateTime dt)
    {
      TimeSpan span = new TimeSpan(dt.Ticks);

      int years =DateTime.Now.Year - dt.Year;
      int months =DateTime.Now.Month - dt.Month;
      int days = DateTime.Now.Day - dt.Day;
      int hours = DateTime.Now.Hour - dt.Hour;
      int mins =DateTime.Now.Minute - dt.Minute;
      int secs = DateTime.Now.Second - dt.Second;

      if (years > 0)
      {
        return String.Format("{0:dd-MMM-yy}", dt);
      }

      if (months > 0)
      {
        return String.Format("{0:MMM dd}", dt);
      }

      if (days > 0)
      {
        return String.Format("{0:MMM dd}", dt);
      }

      if (hours >= 1)
      {
        return String.Format("{0}{1} {2}{3} ago ", hours, hours == 1 ? "hour" : "hours", mins, mins == 1 ? "min" : "mins");
      }

      if (mins >= 0)
      {
        return String.Format("{0}{1} ago", mins, mins == 1 ? "minute" : "minutes");
      }

      return String.Format("{0}{1} ago", secs, secs == 1 ? "second" : "seconds");
    }
  }
}
