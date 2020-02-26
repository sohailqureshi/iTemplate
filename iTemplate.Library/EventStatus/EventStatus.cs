using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTemplate.Library
{
  public class EventStatus
  {
    public Exception Exception { get; set; }
    public bool IsSuccess { get { return this.Exception == null; } }

    public EventStatus()
    {
      this.Exception = null;
    }

    public EventStatus(Exception exception)
    {
      this.Exception = exception;
    }

    public static EventStatus Empty()
    {
      return new EventStatus();
    }
  }
}
