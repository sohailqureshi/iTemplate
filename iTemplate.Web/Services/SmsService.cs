using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Twilio;

namespace iTemplate.Web.Services
{
  public class SmsService : IIdentityMessageService
  {
    // Plug in your sms service here to send a text message.
    public Task SendAsync(IdentityMessage message)
    {
      string AccountSid = "YourTwilioAccountSID";
      string AuthToken = "YourTwilioAuthToken";
      string twilioPhoneNumber = "YourTwilioPhoneNumber";

      var twilio = new TwilioRestClient(AccountSid, AuthToken);

      twilio.SendSmsMessage(twilioPhoneNumber, message.Destination, message.Body);

      // Twilio does not return an async Task, so we need this:
      return Task.FromResult(0);
    }
  }
}
