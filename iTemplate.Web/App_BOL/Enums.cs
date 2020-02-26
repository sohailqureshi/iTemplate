namespace iTemplate.Web.Constants
{
  public class Enums
  {
    public enum SiteRoles
    {
      Administrator = 255,
      Tenant = 1,
      Landlord = 2,
      Agent = 4,
      ServiceProvider = 8
    }

    public enum RegistrationMethods
    {
      AutoRegistration = 0,
      EmailConfirmation = 1,
      ManualConfirmation = 2
    }
  }
}