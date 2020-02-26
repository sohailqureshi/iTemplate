using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace iTemplate.Library
{
  public class Emailer
  {
    SmtpClient mailClient;

    /// <summary>
    /// Send email...
    /// </summary>
    /// <param name="message"></param>
    /// <param name="confirmationToken"></param>
    /// <returns></returns>
    public static EventStatus SendMail(MailMessage message)
    {
      message.BodyEncoding = Encoding.UTF8;
      message.IsBodyHtml = true;
      SmtpClient mailClient = new SmtpClient();

      try
      {
        mailClient.Send(message);
      }
      catch (Exception ex)
      {
        return new EventStatus(ex);
      }

      return EventStatus.Empty();
    }
  }
}
