using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;

namespace iTemplate.Web.Services
{
  public class EmailService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {
      var sentFrom = "yourAccount@outlook.com";

      // Configure the client for outlook:
      //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");
      System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

#if DEBUG
          client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory;
      
#else
      // Credentials:
      var credentialUserName = "yourAccount@outlook.com";
      var pwd = "yourApssword";


      // Credentials:
      //var sendGridUserName = "yourSendGridUserName";
      //var sentFrom = "whateverEmailAdressYouWant";
      //var sendGridPassword = "YourSendGridPassword";

      // Configure the client:
      client = new System.Net.Mail.SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

      client.Port = 587;
      client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
      client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory;
      client.UseDefaultCredentials = false;

      // Create the credentials:
      System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);

      client.EnableSsl = true;
      client.Credentials = credentials;
#endif

      // Create the message:
      var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);

      mail.Subject = message.Subject;
      mail.Body = message.Body;
      mail.IsBodyHtml = true;

      // Send:
      return client.SendMailAsync(mail);
    }
  }


}
